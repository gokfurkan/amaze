using System;
using Game.Dev.Scripts.Scriptables;
using Template.Scripts;
using UnityEngine;

namespace Game.Dev.Scripts
{
    public class CameraController : MonoBehaviour
    {
        private LevelOptions levelOptions;

        private void Awake()
        {
            levelOptions = InfrastructureManager.instance.gameSettings.levelOptions;
        }

        private void Start()
        {
            InitCamera();
        }

        private void InitCamera()
        {
            int currentLevel = LevelSpawnManager.instance.levelIndex;
            
            transform.position = levelOptions.levelDataOptions[currentLevel].cameraPos;
            transform.eulerAngles = levelOptions.levelDataOptions[currentLevel].cameraRot;
        }
    }
}