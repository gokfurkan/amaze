using Game.Dev.Scripts.Scriptables;
using Template.Scripts;
using UnityEngine;

namespace Game.Dev.Scripts
{
    public class SwipeController : MonoBehaviour
    {
        private PlayerOptions playerOptions;
        private Vector2 startPosition;
        private Vector2 endPosition;
        
        private void Awake()
        {
            playerOptions = InfrastructureManager.instance.gameSettings.playerOptions;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPosition = Input.mousePosition;
                endPosition = Input.mousePosition;
            }

            if (!playerOptions.detectSwipeOnlyAfterRelease && Input.GetMouseButton(0))
            {
                endPosition = Input.mousePosition;
                DetectSwipe();
            }

            if (Input.GetMouseButtonUp(0))
            {
                endPosition = Input.mousePosition;
                DetectSwipe();
            }
        }

        private void DetectSwipe()
        {
            if (SwipeDistanceCheckMet())
            {
                SwipeDirection swipeDirection;
                
                if (IsVerticalSwipe())
                {
                    swipeDirection = (startPosition.y - endPosition.y > 0) ? SwipeDirection.Forward : SwipeDirection.Back;
                }
                else
                {
                    swipeDirection = (startPosition.x - endPosition.x > 0) ? SwipeDirection.Left : SwipeDirection.Right;
                }

                startPosition = endPosition;
                BusSystem.CallDetectSwipe(swipeDirection);
            }
        }

        private bool SwipeDistanceCheckMet()
        {
            return VerticalMovementDistance() > playerOptions.minDistanceForSwipe || HorizontalMovementDistance() > playerOptions.minDistanceForSwipe;
        }

        private float VerticalMovementDistance()
        {
            return Mathf.Abs(startPosition.y - endPosition.y);
        }

        private float HorizontalMovementDistance()
        {
            return Mathf.Abs(startPosition.x - endPosition.x);
        }

        private bool IsVerticalSwipe()
        {
            return VerticalMovementDistance() > HorizontalMovementDistance();
        }
    }
}