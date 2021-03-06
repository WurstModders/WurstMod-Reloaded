using System.Collections.Generic;
using System.Reflection;
using FistVR;
using UnityEngine;

namespace WurstMod
{
    public class SceneSettingsOverride : MonoBehaviour
    {
        [Header("PlayerAffordance")] public bool IsSpawnLockingEnabled = true;
        public bool AreHitboxesEnabled;
        public bool DoesDamageGetRegistered;
        public float MaxPointingDistance = 1f;
        public float MaxProjectileRange = 500f;
        public bool ForcesCasingDespawn;
        [Header("Locomotion Options")] public bool IsLocoTeleportAllowed = true;
        public bool IsLocoSlideAllowed = true;
        public bool IsLocoDashAllowed = true;
        public bool IsLocoTouchpadAllowed = true;
        public bool IsLocoArmSwingerAllowed = true;
        public bool DoesTeleportUseCooldown;
        public bool DoesAllowAirControl;
        public bool UsesMaxSpeedClamp;
        public float MaxSpeedClamp = 3f;
        [Header("Player Catching Options")] public bool UsesPlayerCatcher = true;
        public float CatchHeight = -50f;
        [Header("Player Respawn Options")] public int DefaultPlayerIFF;
        public bool DoesPlayerRespawnOnDeath = true;
        public float PlayerDeathFade = 3f;
        public float PlayerRespawnLoadDelay = 3.5f;
        public string SceneToLoadOnDeath = string.Empty;
        public bool DoesUseHealthBar;
        public bool IsQuickbeltSwappingAllowed = true;
        public bool AreQuickbeltSlotsEnabled;
        public bool ConfigQuickbeltOnLoad;
        public int QuickbeltToConfig;
        public bool IsSceneLowLight;
        public bool IsAmmoInfinite;
        public bool AllowsInfiniteAmmoMags = true;
        public bool UsesUnlockSystem;
        public Transform DeathResetPoint;
        public GameObject OnDeathSendMessageTarget;
        public string OneDeathSendMessage;
        public List<GameObject> ShotEventReceivers;
        public int HowManyToShotReceivePerFrame = 10;
        [Header("QuitReceivers")] public List<GameObject> QuitReceivers;
        [Header("Audio Stuff")] public FVRSoundEnvironment DefaultSoundEnvironment;
        public float BaseLoudness = 5f;
        public bool UsesWeaponHandlingAISound;
        public float MaxImpactSoundEventDistance = 15f;

        private static readonly Dictionary<FieldInfo, FieldInfo> SerializedFields = new Dictionary<FieldInfo, FieldInfo>();
        static SceneSettingsOverride()
        {
            foreach (var field in typeof(SceneSettingsOverride).GetFields())
                SerializedFields.Add(field, typeof(FVRSceneSettings).GetField(field.Name));
        }
        
        
        private void Start()
        {
            var real = GM.CurrentSceneSettings;
            foreach (var field in SerializedFields)
                field.Value.SetValue(real, field.Key.GetValue(this));
            SM.SetReverbEnvironment(this.DefaultSoundEnvironment);
        }
    }
}