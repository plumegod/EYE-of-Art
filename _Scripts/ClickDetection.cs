using aPlume.Input;
using AudioSystem._Scripts.Data;

public class ClickDetection : MonoBehaviour, IPointerClickHandler
{
    private Camera _camera;
    [SerializeField] private AudioType clickType;
    [SerializeField] private string levelName;
    [SerializeField] private LoadSceneManager loadScene;

    private void Start()
    {
        _camera = Camera.main;

        SceneInputManager.Instance.InputReader.OnClickCancelEvent += LoadLevel;
    }

    private void OnDisable()
    {
        SceneInputManager.Instance.InputReader.OnClickCancelEvent -= LoadLevel;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }

    private async void LoadLevel()
    {
        var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out var hit)) return;
        if (hit.collider.gameObject == gameObject)
        {
            //todo:要做的事
            loadScene.LoadScene(levelName);
            await AudioSystem._Scripts.AudioSystem.Instance.FastAudioSource(clickType);
        }
    }
}