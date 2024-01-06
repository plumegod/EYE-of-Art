using AudioSystem._Scripts.Data;

public class MainStart : MonoBehaviour
{
    [SerializeField] private AudioType bgmType;
    [SerializeField] private AudioType effectType;
    [SerializeField] private LoadSceneManager loadSceneManager;
    [SerializeField] private Button startButton;

    private async void Start()
    {
        startButton.onClick.AddListener(StartGame);

        await AudioSystem._Scripts.AudioSystem.Instance.FastAudioSource(bgmType);
    }

    private async void StartGame()
    {
        startButton.onClick.RemoveListener(StartGame);

        loadSceneManager.LoadScene("Select");

        await AudioSystem._Scripts.AudioSystem.Instance.FastAudioSource(effectType);
    }
}