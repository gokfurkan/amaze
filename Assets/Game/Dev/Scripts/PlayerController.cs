using System.Collections.Generic;
using DG.Tweening;
using Game.Dev.Scriptables;
using Sirenix.OdinInspector;
using Template.Scripts;
using UnityEngine;

namespace Game.Dev
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerOptions playerOptions;

        [Space(10)]
        [ReadOnly] public bool hasMoving;
        [ReadOnly] public int lastTargetGridAmount;

        private void OnEnable()
        {
            BusSystem.OnDetectSwipe += OnDetectSwipe;
        }

        private void OnDisable()
        {
            BusSystem.OnDetectSwipe -= OnDetectSwipe;
        }

        private void OnDetectSwipe(SwipeDirection direction)
        {
            if (hasMoving) return;
            hasMoving = true;

            RotateObjectOnSwipe(direction);
        }

        private void RotateObjectOnSwipe(SwipeDirection direction)
        {
            Vector3 targetRotation = direction switch
            {
                SwipeDirection.Left => playerOptions.leftRotateTarget,
                SwipeDirection.Right => playerOptions.rightRotateTarget,
                SwipeDirection.Forward => playerOptions.forwardRotateTarget,
                SwipeDirection.Back => playerOptions.backRotateTarget,
                _ => Vector3.zero
            };

            transform.DOLocalRotate(targetRotation, playerOptions.rotateDuration).OnComplete(PerformRaycastAndCountHits);
        }

        private void PerformRaycastAndCountHits()
        {
            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, playerOptions.rayCastMaxDistance, playerOptions.rayCastLayerMask);

            lastTargetGridAmount = 0;

            foreach (RaycastHit hit in hits)
            {
                if (ExtensionsMethods.IsInLayerMask(hit.collider.gameObject.layer , playerOptions.gridLayerMask))
                {
                    lastTargetGridAmount++;
                }
            }
            
            MoveToTarget();
        }

        private void MoveToTarget()
        {
            var moveDistance = lastTargetGridAmount * playerOptions.moveAmountPerGrid;
            var moveDuration = lastTargetGridAmount * playerOptions.moveDurationPerGrid;

            transform.DOLocalMove(transform.localPosition + transform.forward * moveDistance, moveDuration)
            .SetEase(playerOptions.moveEase)    
            .OnComplete(() =>
            {
                hasMoving = false;
            });
        }
    }
}
