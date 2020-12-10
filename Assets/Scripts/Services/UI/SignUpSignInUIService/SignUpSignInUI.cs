using System.Threading.Tasks;
using API;
using Containers.UI.UIService.ScreenStacks;
using Services.UI.MainMenuUIService;
using UniRx;
using UnityEngine.UI;

namespace Services.UI.SignUpSignInUIService
{
    public class SignUpSignInUI : UIScreen
    {
        public InputField LoginField;
        public InputField PasswordField;
        public Button SignButton;
        public Button SwitchButton;

        public MainMenuUI MainMenu;

        private bool isSignUp = false;


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
                
            });
        }

        public async Task Sign()
        {
            if (LoginField.text == "" || PasswordField.text == "")
                return;

            if (isSignUp)
            {
                var user = await OSU.Instance.CreateUser(LoginField.text, "example@example.com", PasswordField.text);
                var token = await OSU.Instance.GetToken(user.username, user.email, PasswordField.text);
                OSU.Token = token;
            }
            else
            {
                var token = await OSU.Instance.GetToken(LoginField.text, "example@example.com", PasswordField.text);
                OSU.Token = token;
            }
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