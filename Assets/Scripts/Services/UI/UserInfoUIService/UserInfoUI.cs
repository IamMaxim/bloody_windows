using System.Linq;
using System.Threading.Tasks;
using API;
using UIService.ScreenStacks;
using UnityEngine.UI;

namespace Services.UI.UserInfoUIService
{
    public class UserInfoUI : UIScreen
    {
        public Text Username;
        public RawImage UserImage;
        public Text UserInfoText;
        public Button AddToFriendsButton;
        public Button ShowFriendsList;

        public async Task Show(Profile profile)
        {
            var popupService = LSInjector.LSInjector.Instance.GetService<PopupUIService.PopupUIService>();

            Username.text = profile.user;
            UserInfoText.text = $"Hours played: {profile.hours_played}\n" +
                                $"Location: {profile.location}\n" +
                                $"Map plays: {profile.map_plays}\n" +
                                $"Succ. map plays: {profile.successful_map_plays}\n" +
                                $"Total score: {profile.total_score}";

            if (profile.user == OSU.Username)
            {
                // Handle self
                AddToFriendsButton.enabled = false;
            }
            else
            {
                if (profile.friends.Contains(OSU.Username))
                {
                    // Handle friend
                    AddToFriendsButton.enabled = false;
                }
                else
                {
                    // Handle non-friend
                    AddToFriendsButton.enabled = true;
                    AddToFriendsButton.onClick.RemoveAllListeners();
                    AddToFriendsButton.onClick.AddListener(async () =>
                    {
                        await OSU.Instance.AddFriend(OSU.Token.access, profile.user);
                        await popupService.ShowPopup($"Added {profile.user} to friends.");
                    });
                }
            }

            ShowFriendsList.onClick.RemoveAllListeners();
            ShowFriendsList.onClick.AddListener(async () =>
                {
                    var tasks = await Task.WhenAll(profile.friends.Select(id =>
                        OSU.Instance.GetUser(OSU.Token.access, id)));

                    await popupService.ShowPopup(string.Join("\n",
                            tasks.Select(t => t.user)
                        )
                    );
                }
            );

            await Push();
        }
    }
}