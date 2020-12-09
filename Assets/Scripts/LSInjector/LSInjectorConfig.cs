using UnityEngine;

namespace LSInjector
{
    public abstract class LSInjectorConfig : MonoBehaviour
    {
        public abstract void Setup(LSInjector injector);
    }
}