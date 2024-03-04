using Game.Dev.Scripts.Interfaces;
using Template.Scripts;
using UnityEngine;

namespace Game.Dev.Scripts.Grid
{
    public class GridController : MonoBehaviour , Interacteable
    {
        public bool hasStartGrid;

        [Space(10)]
        public MeshRenderer meshRenderer;
        public Material activeMaterial;
        public Material passiveMaterial;

        private bool canInteract = true;

        public void InitGrid()
        {
            if (hasStartGrid)
            {
                canInteract = false;
                meshRenderer.material = activeMaterial;
            }
            else
            {
                meshRenderer.material = passiveMaterial;
            }
        }

        public void Interact()
        {
            if (!canInteract) return;

            canInteract = false;
            meshRenderer.material = activeMaterial;
            BusSystem.CallActivateGrid();
        }

        public void ResetGrid()
        {
            canInteract = true;
            hasStartGrid = false;
            meshRenderer.material = passiveMaterial;
        }
    }
}