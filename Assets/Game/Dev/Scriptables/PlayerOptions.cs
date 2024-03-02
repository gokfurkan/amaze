using UnityEngine;

namespace Game.Dev.Scriptables
{
    [CreateAssetMenu(fileName = "PlayerOptions", menuName = "ScriptableObjects/PlayerOptions")]
    public class PlayerOptions : ScriptableObject
    {
        [Header("Rotate")] 
        public float rotateDuration;

        [Space(10)]
        public Vector3 leftRotateTarget;
        public Vector3 rightRotateTarget;
        public Vector3 forwardRotateTarget;
        public Vector3 backRotateTarget;
        
        [Header("RayCast")]
        public float rayCastMaxDistance;
        
        [Space(10)]
        public LayerMask gridLayerMask;
        public LayerMask wallLayerMask;
      
    }
}