using API;
using Services.InGameService;
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
        public GameObject MapSelectionEntryPrefab;

        public InGameUI InGameUI;


        private void Awake()
        {
            OnWindowActivated.Subscribe(async _ =>
            {
                infoPanel.SetActive(false);

                // Clear the map list
                for (var i = 0; i < MapListContent.childCount; i++)
                    DestroyImmediate(MapListContent.GetChild(i));

                // Fetch the map list
                var metadata = await Map.Instance.ReadAllMetaData();

                // Add all maps to map list
                metadata.ForEach(md =>
                {
                    infoPanel.SetActive(true);

                    var go = Instantiate(MapSelectionEntryPrefab, MapListContent, false);
                    go.GetComponentInChildren<Text>().text = md.title;

                    // on element click, init info and enable info panel
                    go.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        InfoText.text = $"Difficulty: {md.difficulty}\n" +
                                        $"Downloads: {md.downloads}\n" +
                                        $"Duration: {md.duration}\n" +
                                        $"Succ. plays: {md.successPlays}";

                        PlayButton.onClick.RemoveAllListeners();
                        PlayButton.onClick.AddListener(() => InGameUI.StartMap(md));
                    });
                });
            });
        }
    }
}