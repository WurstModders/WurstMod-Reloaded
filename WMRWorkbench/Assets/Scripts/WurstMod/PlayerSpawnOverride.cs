using FistVR;
using UnityEngine;

namespace WurstMod
{
    public class PlayerSpawnOverride : MonoBehaviour
    {
        private void Start()
        {
            GM.CurrentMovementManager.TeleportToPoint(transform.position, true, transform.eulerAngles);
            GM.CurrentSceneSettings.DeathResetPoint = transform;
        }
    }
}