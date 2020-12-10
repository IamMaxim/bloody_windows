using System;
using Containers.UI.UIService.ScreenStacks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Services.UI.SignUpSignInUIService
{
    public class SignUpSignInUI : UIScreen
    {
        public InputField LoginField;
        public InputField PasswordField;
        public Button SignButton;
        public Button SwitchButton;

        private bool isSignUp = false;


        private void Awake()
        {
            OnWindowActivated.Subscribe(_ =>
            {
                LoginField.text = "";
                PasswordField.text = "";
            });

            SwitchButton.onClick.AddListener(SwitchMode);

            SignButton.onClick.AddListener(Sign);
        }

        public void Sign()
        {
            throw new NotImplementedException();
        }

        public void SwitchMode()
        {
            if (isSignUp)
            {
                // Switch to sign in
                SignButton.GetComponentInChildren<Text>().text = "Sign In";
                SwitchButton.GetComponentInChildren<Text>().text = "Sign Up instead";
                isSignUp = false;
            }
            else
            {
                // Switch to sign up
                SignButton.GetComponentInChildren<Text>().text = "Sign Up";
                SwitchButton.GetComponentInChildren<Text>().text = "Sign In instead";
                isSignUp = true;
            }
        }
    }
}