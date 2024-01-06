public class HideLevel : MonoBehaviour
{
    private static readonly int Pass = Animator.StringToHash("Pass");

    private SelectLoadLevel _selectLoadLevel;
    [SerializeField] private GameObject[] levelCards;

    private void Start()
    {
        _selectLoadLevel = FindObjectOfType<SelectLoadLevel>();

        switch (_selectLoadLevel.SaveLevel)
        {
            case 2:
                levelCards[1].GetComponent<Animator>().SetTrigger(Pass);
                goto case 1;
            case 1:
                levelCards[0].GetComponent<Animator>().SetTrigger(Pass);
                break;
        }
    }
}