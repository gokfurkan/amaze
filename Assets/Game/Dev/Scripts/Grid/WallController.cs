using Game.Dev.Scripts.Scriptables;
using Template.Scripts;
using UnityEngine;

namespace Game.Dev.Scripts.Grid
{
    public class WallController : MonoBehaviour
    {
        public MeshRenderer meshRenderer;
        
        private Material wallMaterial;
        private LevelOptions levelOptions;
        
        public void InitWall()
        {
            levelOptions = InfrastructureManager.instance.gameSettings.levelOptions;
            
            wallMaterial = levelOptions.GetEnvironmentOption().wallMaterial;
            meshRenderer.material = wallMaterial;
        }
        
        public void ResetWall()
        {
            if (levelOptions != null)
            {
                meshRenderer.material = null;
                levelOptions = null;
            }
        }
    }
}