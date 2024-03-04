using System;
using Game.Dev.Scripts.Scriptables;
using Template.Scripts;
using UnityEngine;

namespace Game.Dev.Scripts.Grid
{
    public class GridManager : MonoBehaviour
    {
        public Transform startPos;
        
        private LevelOptions levelOptions;

        private int activatedGridAmount;
        private int totalGridOnLevel;

        private void OnEnable()
        {
            BusSystem.OnActivateGrid += OnActivateGrid;
            BusSystem.OnResetGrids += ResetGrids;
        }

        private void OnDisable()
        {
            BusSystem.OnActivateGrid -= OnActivateGrid;
            BusSystem.OnResetGrids -= ResetGrids;
        }

        private void Awake()
        {
            levelOptions = InfrastructureManager.instance.gameSettings.levelOptions;
        }

        private void Start()
        {
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

        private void OnActivateGrid()
        {
            activatedGridAmount++;
            if (activatedGridAmount == totalGridOnLevel - 1)
            {
                BusSystem.CallLevelEnd(true);
            }
        }

        private void ResetGrids()
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