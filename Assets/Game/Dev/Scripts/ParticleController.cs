﻿using UnityEngine;

namespace Game.Dev.Scripts
{
    public class ParticleController : MonoBehaviour
    {
        public void ResetParticle()
        {
            
        }

        public void ChangeMaterial(Material color)
        {
            Renderer[] allRenderer = transform.GetComponentsInChildren<Renderer>();

            for (int i = 0; i< allRenderer.Length; i++)
            {
                allRenderer[i].material = color;
            }
        }
    }
}