using UnityEngine;
using UnityEngine.UI;

namespace UIService.ScreenStacks.CommonHandlers
{
    public class ExitButton : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var b = GetComponent<Button>();
            if (b == null)
                Debug.LogError("ExitButton attached to non-button element");
            else
                b.onClick.AddListener(Application.Quit);
        }
    }
}
