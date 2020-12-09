using System.Threading.Tasks;
using JetBrains.Annotations;
using LSInjector;
using UIService.ScreenStacks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UIService
{
    /// <summary>
    /// This service manages the overall app UI (so you should add your UI components to the scene from here).
    /// </summary>
    public class UIService : LSService
    {
        private Task createCanvasTask;

        private Canvas root;
        private RectTransform backgroundPanel;
        private RectTransform contentPanel;
        private RectTransform topMostPanel;
        private UIScreenStack ScreenStack;

        public async Task AddBackgroundUI([NotNull] GameObject ui)
        {
            await createCanvasTask;
            ui.transform.SetParent(backgroundPanel.transform, false);
        }

        public async Task AddUI([NotNull] GameObject ui)
        {
            await createCanvasTask;
            ui.transform.SetParent(contentPanel.transform, false);
        }

        public async Task AddRootUI([NotNull] GameObject ui)
        {
            await createCanvasTask;
            ui.transform.SetParent(root.transform, false);
        }

        public async Task AddTopMostUI([NotNull] GameObject ui)
        {
            await createCanvasTask;
            ui.transform.SetParent(root.transform, false);
        }

        private async Task LoadAddressables()
        {
            var go = await Addressables.InstantiateAsync("UIServiceRoot").Task;
            root = go.GetComponent<Canvas>();
            backgroundPanel = root.transform.Find("Background").GetComponent<RectTransform>();
            contentPanel = root.transform.Find("Content").GetComponent<RectTransform>();
            ScreenStack = root.transform.Find("ScreenStack").GetComponent<UIScreenStack>();
            topMostPanel = root.transform.Find("TopMost").GetComponent<RectTransform>();
        }

        public override async void PreInit()
        {
            createCanvasTask = LoadAddressables();
            await createCanvasTask;
        }

        public async Task<UIScreenStack> GetScreenStack()
        {
            await createCanvasTask;
            return ScreenStack;
        }

        public async Task<float> GetScalingFactor()
        {
            await createCanvasTask;
            return root.scaleFactor;
        }
    }
}