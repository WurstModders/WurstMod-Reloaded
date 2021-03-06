using UnityEngine;

namespace WurstMod
{
    public class ItemSpawner : MonoBehaviour
    {
        private void Start()
        {
            var itemSpawner = WurstModPlugin.Instance.Prefabs["ItemSpawner"];
            Instantiate(itemSpawner, transform.position, transform.rotation, transform.parent);
        }
    }
}