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
        public Transform raycastOrigin;

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

            transform.DOLocalRotate(targetRotation, playerOptions.rotateDuration).OnComplete(PerformRaycast);
        }

        private void PerformRaycast()
        {
            var hits = PerformRaycastAndCountHits(raycastOrigin.position, transform.forward, playerOptions.gridLayerMask);

            lastTargetGridAmount = hits.Length;
    
            hasMoving = false;
        }

        private RaycastHit[] PerformRaycastAndCountHits(Vector3 origin, Vector3 direction, LayerMask layerMask)
        {
            return Physics.RaycastAll(origin, direction, playerOptions.rayCastMaxDistance, layerMask);
        }
    }
}
