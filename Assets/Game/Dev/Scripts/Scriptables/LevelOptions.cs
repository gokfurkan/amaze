using System;
using System.Collections.Generic;
using Template.Scripts;
using UnityEngine;

namespace Game.Dev.Scripts.Scriptables
{
    [CreateAssetMenu(fileName = "LevelOptions", menuName = "ScriptableObjects/LevelOptions")]
    public class LevelOptions : ScriptableObject
    {
        public List<LevelDataOption> levelDataOptions;
        public List<EnvironmentOption> environmentOptions;

        private int GetDataLevel()
        {
            return SaveManager.instance.saveData.GetLevel() % levelDataOptions.Count;
        }

        public LevelDataOption GetLevelDataOption()
        {
            return levelDataOptions[GetDataLevel()];
        }

        public EnvironmentOption GetEnvironmentOption()
        {
            var currentEnvironment = SaveManager.instance.saveData.environmentIndex;
            return environmentOptions[currentEnvironment];
        }
    }

    [Serializable]
    public class LevelDataOption
    {
        public string levelName;

        [Space(10)]
        public GridOption gridOption;
        public CameraOption cameraOption;
    }

    [Serializable]
    public class GridOption
    {
        [TextArea(10, 10)]
        public string gridData;

        [Space(10)]
        public int rowExpandAmount;
        public int colExpandAmount;
    }

    [Serializable]
    public class CameraOption
    {
        public Vector3 cameraPos;
        public Vector3 cameraRot;
    }

    [Serializable]
    public class ParticleOption
    {
        [Space(10)]
        public float gridPosY;
        
        [Space(10)]
        public float gridNormalScale;
        public float gridEndScale;
    }

    [Serializable]
    public class EnvironmentOption
    {
        public string environmentName;
        
        [Space(10)]
        public Material wallMaterial;
        public Material activeGridMaterial;
        public Material passiveGridMaterial;
        public Material gridParticleMaterial;
        
        [Space(10)]
        public ParticleOption particleOption;
    }
}