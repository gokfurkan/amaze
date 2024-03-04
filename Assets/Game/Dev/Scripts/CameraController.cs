using System;
using Game.Dev.Scripts.Scriptables;
using Template.Scripts;
using UnityEngine;

namespace Game.Dev.Scripts
{
    public class CameraController : MonoBehaviour
    {
        private LevelDataOption levelData;

        private void Awake()
        {
            levelData = InfrastructureManager.instance.gameSettings.levelOptions.GetLevelDataOption();
        }

        private void Start()
        {
            InitCamera();
        }

        private void InitCamera()
        {
            transform.position = levelData.cameraPos;
            transform.eulerAngles = levelData.cameraRot;
        }
    }
}