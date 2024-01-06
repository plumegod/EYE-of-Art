public class QuestionUISetting : MonoBehaviour
{
    private int _questionIndex;

    [SerializeField] private Button leftButton;
    [SerializeField] private QuestionData[] questionData;
    [SerializeField] private Image questionImage;
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Button rightButton;

    public event Action OnReadQuestionEvent;

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        Init();
        leftButton.onClick.AddListener(UpQuestionTip);
        rightButton.onClick.AddListener(NextQuestionTip);
    }

    private void OnDisable()
    {
        leftButton.onClick.RemoveListener(UpQuestionTip);
        rightButton.onClick.RemoveListener(NextQuestionTip);
    }

    private void Init()
    {
        leftButton.gameObject.SetActive(false);
        rightButton.gameObject.SetActive(true);

        _questionIndex = 0;

        if (questionData[_questionIndex].QuestionImage != null)
            questionImage.sprite = questionData[_questionIndex].QuestionImage;

        if (questionData[_questionIndex].QuestionText != null)
            questionText.text = questionData[_questionIndex].QuestionText;
    }

    private void NextQuestionTipPanel()
    {
        leftButton.gameObject.SetActive(true);
        rightButton.gameObject.SetActive(true);

        if (questionData[_questionIndex].QuestionImage != null)
            questionImage.sprite = questionData[_questionIndex].QuestionImage;

        if (questionData[_questionIndex].QuestionText != null)
            questionText.text = questionData[_questionIndex].QuestionText;
    }

    private void UpdateQuestionTip()
    {
        if (_questionIndex == 0)
            Init();

        if (_questionIndex <= 0) return;
        NextQuestionTipPanel();
    }

    private void NextQuestionTip()
    {
        if (_questionIndex < questionData.Length - 1)
        {
            _questionIndex++;
            UpdateQuestionTip();
        }

        else if (_questionIndex == questionData.Length - 1)
        {
            OnReadQuestionEvent?.Invoke();
        }
    }

    private void UpQuestionTip()
    {
        if (_questionIndex != 0)
        {
            _questionIndex--;
            UpdateQuestionTip();
        }
    }
}