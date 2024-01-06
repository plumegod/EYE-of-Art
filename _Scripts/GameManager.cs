using aPlume.Input;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Start()
    {
        Instance = this;

        Continue();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        SceneInputManager.Instance.InputReader.SetUI();
    }

    public void Continue()
    {
        Time.timeScale = 1;
        SceneInputManager.Instance.InputReader.SetGameplay();
    }
}