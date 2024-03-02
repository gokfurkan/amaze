using Template.Scripts;
using UnityEngine;

namespace Game.Dev
{
    public class SwipeController : MonoBehaviour
    {
        [SerializeField] private bool detectSwipeOnlyAfterRelease = false;
        [SerializeField] private float minDistanceForSwipe = 20f;

        private Vector2 startPosition;
        private Vector2 endPosition;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPosition = Input.mousePosition;
                endPosition = Input.mousePosition;
            }

            if (!detectSwipeOnlyAfterRelease && Input.GetMouseButton(0))
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
            return VerticalMovementDistance() > minDistanceForSwipe || HorizontalMovementDistance() > minDistanceForSwipe;
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