using System;
using UIService.ScreenStacks;
using UnityEngine.UI;

namespace Services.UI.AddFriendUIService
{
    public class AddFriendUI : UIScreen
    {
        public Button addFriendButton;

        private void Awake()
        {
            var popupService = LSInjector.LSInjector.Instance.GetService<PopupUIService.PopupUIService>();
            
            addFriendButton.onClick.AddListener(() =>
            {
                Pop();
                popupService.ShowPopup("Friend request sent.");
            });
        }
    }
}