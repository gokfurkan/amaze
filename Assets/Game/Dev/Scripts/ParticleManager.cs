using System;
using System.Collections;
using System.Collections.Generic;
using Template.Scripts;
using UnityEngine;

namespace Game.Dev.Scripts
{
    public class ParticleManager : Singleton<ParticleManager>
    {
        public List<ParticleOption> particleOptions;

        protected override void Initialize()
        {
            base.Initialize();
            
            InitializeParticles();
        }

        private void InitializeParticles()
        {
            foreach (var option in particleOptions)
            {
                option.ParticlePool = new Stack<ParticleSystem>();
                for (int i = 0; i < option.poolAmount; i++)
                {
                    var createdParticle = Instantiate(option.particle, option.parent);
                    createdParticle.gameObject.SetActive(false);
                    option.ParticlePool.Push(createdParticle);
                }
            }
        }
        
        public ParticleSystem GetParticle(ParticleType type , bool rePush = true ,float rePushDuration = 2)
        {
            var particle = GetParticleFromPool(type);
            if (rePush)
            {
                StartCoroutine(RePushParticle(particle, type, rePushDuration));
            }
            return particle;
        }

        private ParticleSystem GetParticleFromPool(ParticleType type)
        {
            ParticleOption option = particleOptions[(int)type];
            if (option.ParticlePool.Count == 0)
            {
                Debug.LogWarning("Particle pool is empty! Consider increasing pool amount.");
                return Instantiate(option.particle, option.parent);
            }
            return option.ParticlePool.Pop();
        }

        private IEnumerator RePushParticle(ParticleSystem particle, ParticleType type, float duration)
        {
            yield return new WaitForSeconds(duration);
            particle.gameObject.SetActive(false);
            particle.transform.localScale = Vector3.one;
            particleOptions[(int)type].ParticlePool.Push(particle);
        }

        public void ResetParticles()
        {
            for (int i = 0; i < particleOptions.Count; i++)
            {
                var particleStack = particleOptions[i].ParticlePool.ToArray();
                
                foreach (var item in particleStack)
                {
                    var gridController = item.GetComponent<ParticleController>();
                    if (gridController != null)
                    {
                        gridController.ResetParticle();
                    }
                }
            }
        }
    }

    [Serializable]
    public class ParticleOption
    {
        public string name;
        
        [Space(10)]
        public ParticleSystem particle;
        public Transform parent;
        public int poolAmount;

        [Space(10)]
        public Stack<ParticleSystem> ParticlePool;
    }
}