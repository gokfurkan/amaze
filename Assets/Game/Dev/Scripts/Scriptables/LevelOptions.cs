using System;
using System.Collections.Generic;
using Template.Scripts;
using UnityEngine;

namespace Game.Dev.Scripts.Scriptables
{
    [CreateAssetMenu(fileName = "LevelOptions", menuName = "ScriptableObjects/LevelOptions")]
    public class LevelOptions : ScriptableObject
    {
        public int tutorialLevelAmount;
        public List<LevelDataOption> levelDataOptions;

        public int GetDataLevel()
        {
            return SaveManager.instance.saveData.GetLevel() % levelDataOptions.Count;
        }
        
        #region Grid

        public int[,] GetGridValues()
        {
            string[] lines = levelDataOptions[GetDataLevel()].gridData.Trim().Split('\n');
    
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

            return gridValues;
        }
        
        public int GetGridAmount()
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

        public int GetWallAmount()
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

        #endregion
    }

    [Serializable]
    public class LevelDataOption
    {
        public string levelName;
        public bool tutorialLevel;
        
        [Header("Grid")]
        [TextArea(10, 10)]
        public string gridData;

        [Header("Camera")] 
        public Vector3 cameraPos;
        public Vector3 cameraRot;
    }
}