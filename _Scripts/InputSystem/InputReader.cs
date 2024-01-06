using System;

namespace aPlume.Input
{
    [CreateAssetMenu(menuName = "Create InputReader(创建UI控制器)")]
    public class InputReader : ScriptableObject, GameInput.IPlayActions, GameInput.IUIActions
    {
        [SerializeField] private readonly InputMode inputMode = InputMode.Play;
        public GameInput GameInput { get; private set; }

        public bool Click { get; private set; }

        public void OnMove(InputAction.CallbackContext context)
        {
            OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
        }

        void GameInput.IPlayActions.OnClick(InputAction.CallbackContext context)
        {
            OnClickEvent?.Invoke();
            if (context.started) Click = true;

            if (context.canceled)
            {
                Click = false;
                OnClickCancelEvent?.Invoke();
            }
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
            OnNavigateEvent?.Invoke();
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            OnSubmitEvent?.Invoke();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            OnCancelEvent?.Invoke();
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            OnPointEvent?.Invoke();
        }

        void GameInput.IUIActions.OnClick(InputAction.CallbackContext context)
        {
            OnUIClickEvent?.Invoke();
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            OnScrollWheelEvent?.Invoke();
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
            OnMiddleClickEvent?.Invoke();
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            OnRightClickEvent?.Invoke();
        }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context)
        {
            OnTrackedDevicePositionEvent?.Invoke();
        }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
        {
            OnTrackedDeviceOrientationEvent?.Invoke();
        }

        private void OnEnable()
        {
            InitializeMode();
        }

        // 初始化模式
        private void InitializeMode()
        {
            if (GameInput != null) return;
            GameInput = new GameInput();
            GameInput.Play.SetCallbacks(this);
            GameInput.UI.SetCallbacks(this);

            if (inputMode == InputMode.Play)
                SetGameplay();
            if (inputMode == InputMode.UI)
                SetUI();
        }

        // 切换输入模式
        public void SetGameplay()
        {
            GameInput.Play.Enable();
            GameInput.UI.Disable();
        }

        public void SetUI()
        {
            GameInput.Play.Disable();
            GameInput.UI.Enable();
        }

        // Play输入
        public event Action<Vector2> OnMoveEvent;
        public event Action OnClickEvent;
        public event Action OnClickCancelEvent;

        // UI输入
        public event Action OnNavigateEvent;
        public event Action OnSubmitEvent;
        public event Action OnCancelEvent;
        public event Action OnPointEvent;
        public event Action OnUIClickEvent;
        public event Action OnScrollWheelEvent;
        public event Action OnMiddleClickEvent;
        public event Action OnRightClickEvent;
        public event Action OnTrackedDevicePositionEvent;
        public event Action OnTrackedDeviceOrientationEvent;

        private enum InputMode
        {
            Play,
            UI
        }
    }
}