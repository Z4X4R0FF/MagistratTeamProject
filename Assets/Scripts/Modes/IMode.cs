using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Modes
{
    public interface IMode
    {
        void Init();
        void Stop();
        void UpdateMode();
    }
}