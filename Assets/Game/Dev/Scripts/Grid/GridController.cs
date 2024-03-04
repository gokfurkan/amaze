using System;
using Game.Dev.Scripts.Interfaces;
using Game.Dev.Scripts.Scriptables;
using Template.Scripts;
using UnityEngine;

namespace Game.Dev.Scripts.Grid
{
    public class GridController : MonoBehaviour , Interacteable
    {
        public bool hasStartGrid;

        [Space(10)]
        public MeshRenderer meshRenderer;

        private bool canInteract = true;

        private Color gridActiveColor;
        private Color gridPassiveColor;
        private LevelDataOption levelData;

        public void InitGrid()
        {
            levelData = InfrastructureManager.instance.gameSettings.levelOptions.GetLevelDataOption();
            
            gridActiveColor = levelData.gridActiveColor;
            gridPassiveColor = levelData.gridPassiveColor;
            
            if (hasStartGrid)
            {
                canInteract = false;
                meshRenderer.material.color = gridActiveColor;
            }
            else
            {
                meshRenderer.material.color = gridPassiveColor;
            }
        }

        public void Interact()
        {
            if (!canInteract) return;

            canInteract = false;
            meshRenderer.material.color = gridActiveColor;
            BusSystem.CallActivateGrid(gameObject);
        }

        public void ResetGrid()
        {
            canInteract = true;
            hasStartGrid = false;
            if (levelData != null)
            {
                meshRenderer.material.color = gridPassiveColor;
                
                levelData = null;
                gridActiveColor = Color.clear;
                gridPassiveColor = Color.clear;
            }
        }
    }
}