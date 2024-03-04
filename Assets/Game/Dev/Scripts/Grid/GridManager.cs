using Game.Dev.Scripts.Player;
using Game.Dev.Scripts.Scriptables;
using Template.Scripts;
using UnityEngine;

namespace Game.Dev.Scripts.Grid
{
    public class GridManager : Singleton<GridManager>
    {
        public Transform startPos;
        
        private int activatedGridAmount;
        private int totalGridOnLevel;
        private Color gridParticleColor;
        private LevelOptions levelOptions;

        private void OnEnable()
        {
            BusSystem.OnActivateGrid += OnActivateGrid;
        }

        private void OnDisable()
        {
            BusSystem.OnActivateGrid -= OnActivateGrid;
        }

        protected override void Initialize()
        {
            base.Initialize();
            
            levelOptions = InfrastructureManager.instance.gameSettings.levelOptions;
            gridParticleColor = levelOptions.GetLevelDataOption().gridParticleColor;
            
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            int[,] gridValues = levelOptions.GetGridValues();

            Vector3 startPosition = startPos.position;
            Vector3 spawnPosition = startPosition;

            int rowCount = gridValues.GetLength(0);
            int colCount = gridValues.GetLength(1);
            
            for (int z = 0; z < rowCount; z++)
            {
                for (int x = 0; x < colCount; x++)
                {
                    spawnPosition.x = startPosition.x + x;
                    spawnPosition.z = startPosition.z + z;

                    int value = gridValues[z, x];

                    var poolType = (value == 0) ? PoolType.Wall : PoolType.Grid;

                    var createdObject = Pooling.instance.poolObjects[(int)poolType].GetItem();
                    createdObject.transform.position = spawnPosition;

                    if (value is 1 or 2)
                    {
                        var gridController = createdObject.GetComponent<GridController>();
                        if (value == 2)
                        {
                            BusSystem.CallSetPlayerStartPos(spawnPosition);
                            gridController.hasStartGrid = true;
                        }
                        gridController.InitGrid();
                    }

                    createdObject.SetActive(true);
                }
            }

            totalGridOnLevel = levelOptions.GetGridAmount();
        }

        private void OnActivateGrid(GameObject grid)
        {
            PlayerController.instance.remainingMoveGridAmount--;

            ActivateGridParticle(grid);
            
            activatedGridAmount++;
            if (activatedGridAmount == totalGridOnLevel - 1)
            {
                BusSystem.CallLevelEnd(true);
            }
        }

        private void ActivateGridParticle(GameObject grid)
        {
            var gridParticle = ParticleManager.instance.GetParticle(ParticleType.Grid);
            var gridParticleController = gridParticle.GetComponent<ParticleController>();
            
            gridParticleController.ChangeColor(gridParticleColor);
            
            Vector3 particleSpawnPos = grid.transform.position;
            particleSpawnPos.y = levelOptions.GetLevelDataOption().gridParticlePosY;
            
            gridParticle.transform.position = particleSpawnPos;
            gridParticle.gameObject.SetActive(true);
            
            float particleScale = PlayerController.instance.remainingMoveGridAmount == 0 ?
                levelOptions.GetLevelDataOption().gridParticleEndMoveScale :
                levelOptions.GetLevelDataOption().gridParticleNormalMoveScale;
            gridParticle.transform.localScale = Vector3.one * particleScale;
            
            gridParticle.Play();
        }

        public void ResetGrids()
        {
            var gridPool = Pooling.instance.poolObjects[(int)PoolType.Grid];
            var passiveItems = gridPool.passiveItems.ToArray();
            
            foreach (var item in passiveItems)
            {
                var gridController = item.GetComponent<GridController>();
                if (gridController != null)
                {
                    gridController.ResetGrid();
                }
            }
        }
    }
}