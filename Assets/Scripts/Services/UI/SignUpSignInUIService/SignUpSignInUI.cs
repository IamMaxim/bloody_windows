using System.Threading.Tasks;
using API;
using Services.UI.MainMenuUIService;
using UIService.ScreenStacks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Services.UI.SignUpSignInUIService
{
    public class SignUpSignInUI : UIScreen
    {
        public Text Title;
        public InputField LoginField;
        public InputField PasswordField;
        public Button SignButton;
        public Button SwitchButton;

        public MainMenuUI MainMenu;

        private bool isSignUp = true;


        private void Awake()
        {
            OnWindowActivated.Subscribe(_ =>
            {
                LoginField.text = "";
                PasswordField.text = "";
            });

            SwitchButton.onClick.AddListener(SwitchMode);

            SignButton.onClick.AddListener(async () =>
            {
                await Sign();
                await MainMenu.Push();
            });
        }

        public async Task Sign()
        {
            if (LoginField.text == "" || PasswordField.text == "")
                return;

            if (isSignUp)
            {
                var user = await OSU.Instance.CreateUser(LoginField.text, "example@example.com", PasswordField.text);
                var token = await OSU.Instance.GetToken(LoginField.text, "example@example.com", PasswordField.text);
                OSU.Token = token;
                OSU.Username = user.username;
            }
            else
            {
                var token = await OSU.Instance.GetToken(LoginField.text, "example@example.com", PasswordField.text);
                OSU.Token = token;
                OSU.Username = LoginField.text;
            }
            
            Debug.Log($"Token: {OSU.Token.access}");
            Debug.Log($"Username: {OSU.Username}");
        }

        public void SwitchMode()
        {
            if (isSignUp)
            {
                // Switch to sign in
                Title.text = "Sign In";
                SignButton.GetComponentInChildren<Text>().text = "Sign In";
                SwitchButton.GetComponentInChildren<Text>().text = "Sign Up instead";
                isSignUp = false;
            }
            else
            {
                // Switch to sign up
                Title.text = "Sign Up";
                SignButton.GetComponentInChildren<Text>().text = "Sign Up";
                SwitchButton.GetComponentInChildren<Text>().text = "Sign In instead";
                isSignUp = true;
            }
        }
    }
}