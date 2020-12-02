using UnityEngine;
using UnityEngine.UI;

namespace Containers.UI.UIService.ScreenStacks.CommonHandlers
{
    public class ElementFocuser : MonoBehaviour
    {
        public Selectable selectable;

        void Start()
        {
            var b = GetComponent<Button>();
            if (b == null)
                Debug.LogError("ElementFocuser is attached to element that is not a button. It will have no effect.");
            else
                b.onClick.AddListener(() => selectable.Select());
        }
    }
}