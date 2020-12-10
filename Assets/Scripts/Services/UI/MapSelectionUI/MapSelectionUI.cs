using UIService.ScreenStacks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Services.UI.MapSelectionUI
{
    public class MapSelectionUI : UIScreen
    {
        public Text InfoText;
        public Button PlayButton;
        public RawImage MapImage;

        public GameObject infoPanel;
        
        public RectTransform MapListContent;

        private void Awake()
        {
            OnWindowActivated.Subscribe(_ =>
            {
                infoPanel.SetActive(false);
                
                // Clear the map list
                for (var i = 0; i < MapListContent.childCount; i++)
                    DestroyImmediate(MapListContent.GetChild(i));

                // Fetch the map list
                
                // on element click, init info and enable info panel
            });
        }
    }
}