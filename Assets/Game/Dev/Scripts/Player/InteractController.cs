using System;
using Game.Dev.Scripts.Interfaces;
using Game.Dev.Scripts.Scriptables;
using Template.Scripts;
using UnityEngine;

namespace Game.Dev.Scripts.Player
{
    public class InteractController : MonoBehaviour
    {
        private PlayerOptions playerOptions;
        
        private void Awake()
        {
            playerOptions = InfrastructureManager.instance.gameSettings.playerOptions;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (ExtensionsMethods.IsInLayerMask(collision.gameObject.layer , playerOptions.canInteractLayers))
            {
                var interacteable = collision.GetComponent<Interacteable>();
                if (interacteable != null)
                {
                    interacteable.Interact();
                }
            }
        }
    }
}