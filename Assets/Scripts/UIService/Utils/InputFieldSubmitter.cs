// Source: https://answers.unity.com/questions/849739/submit-inputfield-when-enter-is-clicked.html

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Containers.UI.UIService.Utils
{
    /// <summary>Submits an InputField with the specified button.</summary>
    // Prevents MonoBehaviour of same type (or subtype) to be added more than once to a GameObject.
    [DisallowMultipleComponent]
    // This automatically adds required components as dependencies.
    [RequireComponent(typeof(InputField))]
    public class InputFieldSubmitter : MonoBehaviour
    {
        // Cache input field to the variable to avoid expensive GetComponent calls
        private InputField _inputField;
        private EventSystem _eventSystem;
        private bool allowEnter;

        // This button will be clicked on Enter press
        public Button buttonToClick;
        public bool refocusAfterClick;

        void Start()
        {
            _inputField = GetComponent<InputField>();
            _eventSystem = EventSystem.current;
        }

        private IEnumerator refocus()
        {
            yield return null;
            _eventSystem.SetSelectedGameObject(null);
            _eventSystem.SetSelectedGameObject(_inputField.gameObject);
        }

        private void Update()
        {
            if (allowEnter && _inputField.text.Length > 0 &&
                (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter)))
            {
                buttonToClick.onClick.Invoke();
                if (refocusAfterClick)
                    StartCoroutine(refocus());
                
                allowEnter = false;
            }
            else
                allowEnter = _inputField.isFocused;
        }
    }
}