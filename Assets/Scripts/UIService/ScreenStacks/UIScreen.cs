using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LS.LSInjector;
using UniRx;
using UnityEngine;

namespace Containers.UI.UIService.ScreenStacks
{
    /// <summary>
    /// Should be attached to all screens in the canvas.
    /// Allows the canvas to be used with the screen stack. 
    /// </summary>
    public class UIScreen : MonoBehaviour
    {
        public global::UIService.UIService UIService;

        /// <summary>
        /// Fired when this screen becomes active on the screen stack (either it is pushed to it or screen above is
        /// removed and current one is now visible).
        /// </summary>
        public readonly Subject<Unit> OnWindowActivated = new Subject<Unit>();

        /// <summary>
        /// Fired when this screen becomes inactive (either it is popped from stack or one more screen is added above).
        /// </summary>
        public readonly Subject<Unit> OnWindowDeactivated = new Subject<Unit>();


        private void Start()
        {
            if (UIService == null)
                UIService = LSInjector.Instance.GetService<global::UIService.UIService>();
        }

        /// <summary>
        /// Pushes the current screen to the stack.
        /// </summary>
        public async Task Push()
        {
            if (UIService == null)
                UIService = LSInjector.Instance.GetService<global::UIService.UIService>();

            (await UIService.GetScreenStack()).Push(this);
        }

        public async Task Pop()
        {
            if (UIService == null)
                UIService = LSInjector.Instance.GetService<global::UIService.UIService>();

            var screenStack = await UIService.GetScreenStack();
    
            if (screenStack.Peek() != this)
                throw new InvalidOperationException($"Trying to pop non-active screen {gameObject.name}");

            screenStack.Pop();
        }


        /// <summary>
        /// Returns whether it is allowed to close the current screen. True - allow to close, False - deny to close (you
        /// probably want to open a confirmation dialog or something. Don't leave the user without a feedback). 
        /// </summary>
        public delegate void CloseDelegate(CloseEventArgs args);

        // This event is fired when the screen is prompted to be closed if any of the event handlers return false, screen
        // will stay opened.
        public event CloseDelegate OnClose;

        /// <summary>
        /// Fires the screen close event.
        /// </summary>
        /// <returns>If closing is allowed.</returns>
        public bool FireClose()
        {
            var args = new CloseEventArgs();
            OnClose?.Invoke(args);
            return args.allowClose;
        }

        public async Task ForcePop()
        {
            if (UIService == null)
                UIService = LSInjector.Instance.GetService<global::UIService.UIService>();

            var screenStack = await UIService.GetScreenStack();
    
            if (screenStack.Peek() != this)
                throw new InvalidOperationException($"Trying to force-pop non-active screen {gameObject.name}");

            screenStack.ForcePop();
        }
        
        
        // Just a convenient wrapper for getting services from LSInjector
        [CanBeNull]
        protected T SafeGetService<T>() where T : LSService =>
            LSInjector.Instance.GetService<T>();

        // Just a convenient wrapper for getting services from LSInjector
        protected T GetService<T>() where T : LSService
        {
            var service = SafeGetService<T>();
            if (service != null) return service;

            var msg = $"Service {typeof(T).Name} was requested by service {GetType().Name}, " +
                      "but it is not currently loaded.";
            Debug.LogError(msg);
            throw new InvalidOperationException(msg);
        }
    }

    public class CloseEventArgs
    {
        public bool allowClose = true;
    }
}