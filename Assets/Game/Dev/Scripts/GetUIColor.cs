using Template.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Dev.Scripts
{
    public class GetUIColor : MonoBehaviour
    {
        private void Awake()
        {
            InitUIColor();
        }

        private void InitUIColor()
        {
            Color color;
            
            switch (SaveManager.instance.saveData.environmentIndex)
            {
                case 0:
                case 1:
                    color = Color.black;
                    break;
                case 2:
                    color = Color.white;
                    break;
                default:
                    color = Color.clear;
                    break;
            }
            
            GetComponent<Graphic>().color = color;
        }
    }
}