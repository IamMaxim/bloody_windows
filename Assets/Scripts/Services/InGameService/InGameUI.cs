using System;
using API;
using Services.UI.MapCompletionUIService;
using UIService.ScreenStacks;
using UnityEngine;
using UnityEngine.UI;

namespace Services.InGameService
{
    public class InGameUI : UIScreen
    {
        public Button FinishButton;

        public MapCompletionUI MapCompletionUI;

        private MetaData currentMap;

        private void Awake()
        {
            FinishButton.onClick.AddListener(async () =>
            {
                MapCompletionUI.Show(currentMap.duration, "100");
                await Map.Instance.UpdatePlaysByPK(currentMap.id, "true");
                await OSU.Instance.SubmitPlay(
                    OSU.Token.access, "100", "true", "100", currentMap.id
                );
            });
        }

        public void StartMap(MetaData map)
        {
            currentMap = map;
            
            Push();
        }
    }
}