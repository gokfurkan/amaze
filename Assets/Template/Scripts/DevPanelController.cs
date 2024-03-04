using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Template.Scripts
{
    public class DevPanelController : MonoBehaviour
    {
        public InputField level;
        public InputField environment;

        public void ChangeLevel()
        {
            SaveManager.instance.saveData.level = int.Parse(level.text);
            SaveManager.instance.Save();
            
            BusSystem.CallLevelLoad();
        }
        
        public void ChangeEnvironment()
        {
            SaveManager.instance.saveData.environmentIndex = int.Parse(environment.text);
            SaveManager.instance.Save();
            
            BusSystem.CallLevelLoad();
        }

        public void ResetSaveData()
        {
            SaveManager.instance.Delete();
            BusSystem.CallLevelLoad();
        }
    }
}