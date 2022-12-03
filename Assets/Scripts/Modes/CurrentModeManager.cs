using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Modes
{
    public class CurrentModeManager : MonoBehaviour, ICurrentModeManager
    {
        public static CurrentModeManager instance;

        private IMode currentMode;

        public IMode GetCurrentMod()
        {
            return currentMode;
        }

        public void ChangeCurrentMode(IMode newMode)
        {
            currentMode = newMode;
        }

        private void Awake()
        {
            instance = this;
        }

        public void UpdateCurrentMode()
        {
            if (Time.timeScale == 0)
            {
                return;
            }

            currentMode.UpdateMode();
        }
    }
}