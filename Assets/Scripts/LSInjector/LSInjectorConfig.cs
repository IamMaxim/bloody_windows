using System.Collections.Generic;
using UnityEngine;

namespace LS.LSInjector
{
    public abstract class LSInjectorConfig : MonoBehaviour
    {
        public abstract void Setup(LSInjector injector);
    }
}