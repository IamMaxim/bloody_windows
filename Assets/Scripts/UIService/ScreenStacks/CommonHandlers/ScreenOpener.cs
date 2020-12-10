using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UIService.ScreenStacks.CommonHandlers
{
    public class ScreenOpener : MonoBehaviour
    {
        public global::UIService.UIService UIService;
        public UIScreen nextScreen;

        private void Awake()
        {
            UIService = LSInjector.LSInjector.Instance.GetService<global::UIService.UIService>();
        }

        private void Start()
        {
            // If this component is attached to a button, add a listener
            var button = GetComponent<Button>();
            if (button != null)
                button.onClick.AddListener(async () => await Go());
            else
                Debug.LogWarning($"You attached a ScreenOpener to {gameObject.name}, " +
                                 $"but it does not have a button component");
        }

        private async Task Go() => (await UIService.GetScreenStack()).Push(nextScreen);
    }
}