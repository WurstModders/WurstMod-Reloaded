using System.Collections.Generic;
using UnityEngine;

namespace WurstMod
{
    public class WurstModPlugin : MonoBehaviour
    {
        public static WurstModPlugin Instance { get; private set; }

        public Dictionary<string, GameObject> Prefabs;
        
        private void Awake()
        {
            Instance = this;
        }

        private void PrefabsHandoff(Dictionary<string, GameObject> prefabs)
        {
            Prefabs = prefabs;
        }
    }
}