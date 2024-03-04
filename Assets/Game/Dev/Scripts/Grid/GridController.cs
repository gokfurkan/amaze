using System;
using Game.Dev.Scripts.Interfaces;
using Game.Dev.Scripts.Scriptables;
using Template.Scripts;
using UnityEngine;

namespace Game.Dev.Scripts.Grid
{
    public class GridController : MonoBehaviour , Interacteable
    {
        public MeshRenderer meshRenderer;
        
        [HideInInspector] public bool hasStartGrid;
        private bool canInteract = true;
        
        private Material gridActiveMaterial;
        private Material gridPassiveMaterial;
        private LevelOptions levelOptions;

        public void InitGrid()
        {
            levelOptions = InfrastructureManager.instance.gameSettings.levelOptions;
            
            gridActiveMaterial = levelOptions.GetEnvironmentOption().activeGridMaterial;
            gridPassiveMaterial = levelOptions.GetEnvironmentOption().passiveGridMaterial;
            
            if (hasStartGrid)
            {
                canInteract = false;
                meshRenderer.material = gridActiveMaterial;
            }
            else
            {
                meshRenderer.material = gridPassiveMaterial;
            }
        }

        public void Interact()
        {
            if (!canInteract) return;

            canInteract = false;
            meshRenderer.material = gridActiveMaterial;
            BusSystem.CallActivateGrid(gameObject);
        }

        public void ResetGrid()
        {
            canInteract = true;
            hasStartGrid = false;
            if (levelOptions != null)
            {
                levelOptions = null;
                gridActiveMaterial = null;
                gridPassiveMaterial = null;
            }
        }
    }
}