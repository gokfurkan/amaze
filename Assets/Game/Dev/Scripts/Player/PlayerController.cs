using System.Linq;
using DG.Tweening;
using Game.Dev.Scripts.Scriptables;
using Template.Scripts;
using UnityEngine;

namespace Game.Dev.Scripts.Player
{
    public class PlayerController : Singleton<PlayerController>
    {
        public Transform raycastOrigin;

        [Space(10)]
        [HideInInspector] public bool hasMoving;
        [HideInInspector] public int remainingMoveGridAmount;

        private PlayerOptions playerOptions;

        private void OnEnable()
        {
            BusSystem.OnDetectSwipe += OnDetectSwipe;
            BusSystem.OnSetPlayerStartPos += SetStartPos;
        }

        private void OnDisable()
        {
            BusSystem.OnDetectSwipe -= OnDetectSwipe;
            BusSystem.OnSetPlayerStartPos -= SetStartPos;
        }

        protected override void Initialize()
        {
            base.Initialize();
            
            playerOptions = InfrastructureManager.instance.gameSettings.playerOptions;
        }

        private void SetStartPos(Vector3 startPos)
        {
            transform.position = startPos;
        }

        private void OnDetectSwipe(SwipeDirection direction)
        {
            if (GameManager.instance.gameStatus.hasLevelEnd) return;
            
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
            RaycastHit[] hits = Physics.RaycastAll(raycastOrigin.position, raycastOrigin.forward, playerOptions.rayCastMaxDistance, playerOptions.rayCastLayerMask)
                .OrderBy(hit => Vector3.Distance(raycastOrigin.position, hit.point))
                .ToArray();

            remainingMoveGridAmount = 0;

            foreach (RaycastHit hit in hits)
            {
                if (!ExtensionsMethods.IsInLayerMask(hit.collider.gameObject.layer, playerOptions.gridLayerMask))
                {
                    break;
                }

                remainingMoveGridAmount++;
            }

            MoveToTarget();
        }

        private void MoveToTarget()
        {
            var moveDistance = remainingMoveGridAmount * playerOptions.moveAmountPerGrid;
            var moveDuration = remainingMoveGridAmount * playerOptions.moveDurationPerGrid;

            DOTween.Sequence()
                .Append(transform.DOLocalMove(transform.localPosition + transform.forward * moveDistance, moveDuration)
                .SetEase(playerOptions.moveEase))
                .OnComplete(() => hasMoving = false);
        }
    }
}
