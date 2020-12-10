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
            UserInfoText.text = $"Hours played: {profile.hours_played}" +
                                $"Location: {profile.location}" +
                                $"Map plays: {profile.map_plays}" +
                                $"Succ. map plays: {profile.successful_map_plays}" +
                                $"Total score: {profile.total_score}";

            var myProfile = await OSU.Instance.MyProfile(OSU.Token.access);

            if (profile.user == myProfile.user)
            {
                // Handle self
                AddToFriendsButton.enabled = false;
            }
            else
            {
                if (profile.friends.Contains(myProfile.user))
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
                await popupService.ShowPopup(string.Join("\n", profile.friends)));

            await Push();
        }
    }
}