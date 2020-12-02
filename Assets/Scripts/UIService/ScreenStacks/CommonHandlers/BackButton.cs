using LS.LSInjector;
using UnityEngine;
using UnityEngine.UI;

namespace Containers.UI.UIService.ScreenStacks.CommonHandlers
{
    public class BackButton : MonoBehaviour
    {
        private global::UIService.UIService UIService;

        private void Awake()
        {
            UIService = LSInjector.Instance.GetService<global::UIService.UIService>();
        }

        public void Start()
        {
            var b = GetComponent<Button>();
            if (b == null)
                Debug.LogError("Attached BackButton to non-button object");
            else
                b.onClick.AddListener(async () => (await UIService.GetScreenStack()).Pop());
        }
    }
}