using Game.Dev.Scripts.Scriptables;
using Template.Scripts;
using UnityEngine;

namespace Game.Dev.Scripts
{
    public class GridManager : MonoBehaviour
    {
        public Transform startPos;
        
        private LevelOptions levelOptions;
        
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

                    switch (value)
                    {
                        case 0:
                        {
                            var createdWall = Pooling.instance.poolObjects[(int)PoolType.Wall].GetItem();
                            createdWall.transform.position = spawnPosition;
                            createdWall.SetActive(true);
                            break;
                        }
                        case 1:
                        {
                            var createdGrid = Pooling.instance.poolObjects[(int)PoolType.Grid].GetItem();
                            createdGrid.transform.position = spawnPosition;
                            createdGrid.SetActive(true);
                            break;
                        }
                    }
                }
            }
        }
    }
}