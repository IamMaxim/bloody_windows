using API;
using Services.UI.UserInfoUIService;
using UIService.ScreenStacks;
using UnityEngine.UI;

namespace Services.UI.MainMenuUIService
{
    public class MainMenuUI : UIScreen
    {
        public Button myProfileButton;
        public UserInfoUI userInfoUI;

        private void Awake()
        {
            myProfileButton.onClick.AddListener(async () =>
            {
                await userInfoUI.Show(await OSU.Instance.MyProfile(OSU.Token.access));
            });
        }
    }
}