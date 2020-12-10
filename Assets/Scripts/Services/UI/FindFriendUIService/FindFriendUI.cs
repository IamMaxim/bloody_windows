using System.Linq;
using API;
using Services.UI.UserInfoUIService;
using UIService.ScreenStacks;
using UnityEngine.UI;

namespace Services.UI.FindFriendUIService
{
    public class FindFriendUI : UIScreen
    {
        public InputField username;
        public Button addFriendButton;
        public UserInfoUI UserInfoUI;

        private void Awake()
        {
            var popupService = LSInjector.LSInjector.Instance.GetService<PopupUIService.PopupUIService>();

            addFriendButton.onClick.AddListener(async () =>
            {
                // await OSU.Instance.AddFriend(OSU.Token.access, username.text);
                // await Pop();
                // await popupService.ShowPopup("Friend request sent.");

                var users = await OSU.Instance.SearchUser(OSU.Token.access, username.text);

                if (users.Count == 0)
                {
                    await popupService.ShowPopup($"No user with name {username.text} found");
                }
                else
                {
                    await Pop();
                    await UserInfoUI.Show(users.First());
                }
            });
        }
    }
}