using System.Threading.Tasks;
using LSInjector;
using UIService.ScreenStacks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Services.UI.PopupUIService
{
    public class PopupUIService : LSService
    {
        private Task<GameObject> initTask;
        private GameObject popupPrefab;

        public override async void PreInit()
        {
            initTask = Addressables.InstantiateAsync("PopupUI").Task;
            popupPrefab = await initTask;
        }

        public async Task ShowPopup(string message)
        {
            await initTask;
            var popup = Object.Instantiate(popupPrefab);
            popup.GetComponentInChildren<Text>().text = message;
            await popup.GetComponent<UIScreen>().Push();
        }
    }
}