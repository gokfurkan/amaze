using System.Collections.Generic;
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
        
        private Material gridParticleColor;
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
            gridParticleColor = levelOptions.GetEnvironmentOption().gridParticleMaterial;
            
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            int[,] gridValues = GetGridValues();

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

                    if (value is 0)
                    {
                        var wallController = createdObject.GetComponent<WallController>();
                        wallController.InitWall();
                    }
                    else if (value is 1 or 2)
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

            totalGridOnLevel = GetGridAmount();
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
            
            gridParticleController.ChangeMaterial(gridParticleColor);
            
            Vector3 particleSpawnPos = grid.transform.position;
            particleSpawnPos.y = levelOptions.GetLevelDataOption().particleOption.gridPosY;
            
            gridParticle.transform.position = particleSpawnPos;
            gridParticle.gameObject.SetActive(true);
            
            float particleScale = PlayerController.instance.remainingMoveGridAmount == 0 ?
                levelOptions.GetLevelDataOption().particleOption.gridEndScale :
                levelOptions.GetLevelDataOption().particleOption.gridNormalScale;
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

        public void ResetWalls()
        {
            var wallPool = Pooling.instance.poolObjects[(int)PoolType.Wall];
            var passiveItems = wallPool.passiveItems.ToArray();
            
            foreach (var item in passiveItems)
            {
                var wallController = item.GetComponent<WallController>();
                if (wallController != null)
                {
                    wallController.ResetWall();
                }
            }
        }
        
        private int[,] GetGridValues()
        {
            string[] lines = levelOptions.GetLevelDataOption().gridOption.gridData.Trim().Split('\n');
    
            List<string> cleanedLines = new List<string>();
    
            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
        
                if (!trimmedLine.StartsWith("//"))
                {
                    cleanedLines.Add(trimmedLine.TrimEnd(',', ' '));
                }
            }
    
            lines = cleanedLines.ToArray();
    
            int rowCount = lines.Length;
            int colCount = lines[0].Trim().Split(',').Length;
    
            int[,] gridValues = new int[rowCount, colCount];
    
            for (int i = 0; i < rowCount; i++)
            {
                string[] values = lines[rowCount - 1 - i].Trim().Split(',');

                for (int j = 0; j < colCount; j++)
                {
                    int value = int.Parse(values[j]);
                    gridValues[i, j] = value;
                }
            }

            return ExpandMatrix(gridValues);
        }
        
        private int[,] ExpandMatrix(int[,] originalMatrix)
        {
            int rowExpand = levelOptions.GetLevelDataOption().gridOption.rowExpandAmount;
            int colExpand = levelOptions.GetLevelDataOption().gridOption.colExpandAmount;
            
            int originalRows = originalMatrix.GetLength(0);
            int originalCols = originalMatrix.GetLength(1);
            
            int expandedRows = originalRows + 2 * rowExpand;
            int expandedCols = originalCols + 2 * colExpand;
            
            int[,] expandedMatrix = new int[expandedRows, expandedCols];
            
            for (int i = 0; i < originalRows; i++)
            {
                for (int j = 0; j < originalCols; j++)
                {
                    expandedMatrix[i + rowExpand, j + colExpand] = originalMatrix[i, j];
                }
            }

            return expandedMatrix;
        }
        
        private int GetGridAmount()
        {
            int[,] gridValues = GetGridValues();
            int gridCount = 0;

            for (int i = 0; i < gridValues.GetLength(0); i++)
            {
                for (int j = 0; j < gridValues.GetLength(1); j++)
                {
                    if (gridValues[i, j] == 1)
                    {
                        gridCount++;
                    }
                }
            }

            return gridCount;
        }

        private int GetWallAmount()
        {
            int[,] gridValues = GetGridValues();
            int wallCount = 0;

            for (int i = 0; i < gridValues.GetLength(0); i++)
            {
                for (int j = 0; j < gridValues.GetLength(1); j++)
                {
                    if (gridValues[i, j] == 0)
                    {
                        wallCount++;
                    }
                }
            }

            return wallCount;
        }
    }
}