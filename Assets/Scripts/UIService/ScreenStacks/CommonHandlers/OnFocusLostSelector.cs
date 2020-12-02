using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Containers.UI.UIService.ScreenStacks.CommonHandlers
{
    public class OnFocusLostSelector : MonoBehaviour, IDeselectHandler
    {
        public Selectable element;

        public void Start()
        {
            var inputField = GetComponent<InputField>();
            if (inputField != null)
                inputField.onEndEdit.AddListener(arg0 => OnDeselect(null));
        }
        
        public void OnDeselect(BaseEventData eventData)
        {
            element.Select();
        }
    }
}