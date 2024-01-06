public class FristGameQuestion : MonoBehaviour
{
    private GameUIControls _controls;

    private void Start()
    {
        _controls = FindObjectOfType<GameUIControls>();

        _controls.StartQuestion();
    }
}