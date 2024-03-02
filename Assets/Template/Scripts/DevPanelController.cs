using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Template.Scripts
{
    public class DevPanelController : MonoBehaviour
    {
        public InputField ipf;

        public void ChangeLevel()
        {
            SaveManager.instance.saveData.level = int.Parse(ipf.text);
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