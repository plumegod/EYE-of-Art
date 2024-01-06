public class ExitGame : MonoBehaviour
{
    [SerializeField] private Button exitButton;

    private void Start()
    {
        exitButton.onClick.AddListener(Exit);
    }

    private void Exit()
    {
        Application.Quit();
    }
}