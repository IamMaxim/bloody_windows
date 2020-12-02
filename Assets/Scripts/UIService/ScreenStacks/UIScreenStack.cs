using System.Collections.Generic;
using Containers.UI.UIService.ScreenStacks;
using UnityEngine;
using UniRx;
using UnityEngine.EventSystems;

namespace UIService.ScreenStacks
{
    public class UIScreenStack : MonoBehaviour
    {
        // Keeps the stack of UI screens in order to allow pushing new screens/going back.
        private readonly Stack<UIScreen> _screens = new Stack<UIScreen>();

        /// <summary>
        /// Optional; if set, the provided screen is pushed to the stack on startup.
        /// </summary>
        public UIScreen MainScreen { get; private set; }

        /// <summary>
        /// If set to true, it will be impossible to close the main screen (if it is provided).
        /// </summary>
        public bool KeepMainScreen = true;


        public void SetMainScreen(UIScreen mainScreen, bool keepMainScreen)
        {
            this.MainScreen = mainScreen;
            this.KeepMainScreen = keepMainScreen;
            
            // Forbid screen closing
            if (keepMainScreen)
                mainScreen.OnClose += args => args.allowClose = false;
            // Push the main screen to the stack
            Push(mainScreen);
        }

        public void Start()
        {
            if (MainScreen != null)
                SetMainScreen(MainScreen, KeepMainScreen);
        }

        public void Push(UIScreen screen)
        {
            Debug.Log($"Pushing {screen.gameObject.name}");
            
            // Deselect the currently selected object
            EventSystem.current.SetSelectedGameObject(null);

            // Disable the current screen as the new one will be displayed instead of it.
            if (_screens.Count > 0)
            {
                var s = _screens.Peek();
                s.OnWindowDeactivated.OnNext(Unit.Default);
                s.gameObject.SetActive(false);
            }

            // Set the new screen to be active and push it to the stack.
            var go = screen.gameObject;
            go.SetActive(true);
            EventSystem.current.SetSelectedGameObject(go);
            go.transform.SetParent(transform, false);
            _screens.Push(screen);
            screen.OnWindowActivated.OnNext(Unit.Default);
        }

        public UIScreen Pop()
        {
            // If there are no screens, just ignore.
            if (_screens.Count == 0)
                return null;

            var screen = _screens.Peek();
            var allowToClose = screen.FireClose();
            if (!allowToClose)
                return null;

            // // Deselect the currently selected object
            // EventSystem.current.SetSelectedGameObject(null);

            screen.OnWindowDeactivated.OnNext(Unit.Default);
            _screens.Pop();
            // Deactivate the screen that is popped.
            screen.gameObject.SetActive(false);

            // Activate the next screen (if any)
            if (_screens.Count > 0)
            {
                var s = _screens.Peek();
                s.gameObject.SetActive(true);
                s.OnWindowActivated.OnNext(Unit.Default);
            }

            return screen;
        }

        /// <summary>
        /// If back key (same as escape key) was pressed, pop the screen
        /// </summary>
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Pop();
        }

        public UIScreen Peek() => _screens.Peek();

        public UIScreen ForcePop()
        {
            // If there are no screens, just ignore.
            if (_screens.Count == 0)
                return null;

            var screen = _screens.Peek();

            // // Deselect the currently selected object
            // EventSystem.current.SetSelectedGameObject(null);

            screen.OnWindowDeactivated.OnNext(Unit.Default);
            _screens.Pop();
            // Deactivate the screen that is popped.
            screen.gameObject.SetActive(false);

            // Activate the next screen (if any)
            if (_screens.Count > 0)
            {
                var s = _screens.Peek();
                s.gameObject.SetActive(true);
                s.OnWindowActivated.OnNext(Unit.Default);
            }

            return screen;
        }

        // TODO: support main screen here
        public void Clear()
        {
            if (_screens.Count > 0)
            {
                var screen = Peek();
                screen.OnWindowDeactivated.OnNext(Unit.Default);
                screen.gameObject.SetActive(false);
            }

            _screens.Clear();
        }
    }
}