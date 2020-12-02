using UnityEngine;
using UnityEngine.UI;

namespace Containers.UI.UIService.ScreenStacks.CommonHandlers
{
    public class UIScreenDestroyer : MonoBehaviour
    {
        public GameObject uiScreen;

        public void Awake() => GetComponent<Button>().onClick.AddListener(async () =>
        {
            (await uiScreen.GetComponent<UIScreen>().UIService.GetScreenStack()).ForcePop();
            Destroy(uiScreen);
        });
    }
}