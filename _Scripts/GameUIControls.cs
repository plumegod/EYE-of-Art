using AudioSystem._Scripts.Data;
using Button = UnityEngine.UI.Button;

public class GameUIControls : MonoBehaviour
{
    private Action _continueButtonPointerEnterHandler;
    private Action _continueButtonPointerExitHandler;
    private Action _exitButtonPointerEnterHandler;
    private Action _exitButtonPointerExitHandler;
    private Action _resumeButtonPointerEnterHandler;
    private Action _resumeButtonPointerExitHandler;

    [SerializeField] private AudioType clickType;

    [Space] [SerializeField] private PauseStateButton continueButton;
    [SerializeField] private PauseStateButton exitButton;
    [SerializeField] private GameControls gameControls;

    [Space] [SerializeField] private LoadSceneManager loadSceneManager;

    [Space] [SerializeField] private Button pauseButton;
    [SerializeField] private GameObject pauseMenu;

    [Space] [SerializeField] private TMP_Text pauseText;
    [SerializeField] private Button questionButton;
    [SerializeField] private GameObject questionMenu;
    [SerializeField] private PauseStateButton resumeButton;

    [Space] [SerializeField] private Button successBackButton;
    [SerializeField] private GameObject successMenu;
    [SerializeField] private TMP_Text successText;
    [SerializeField] private AudioType successType;

    private void Awake()
    {
        pauseMenu.SetActive(false);
        questionMenu.SetActive(false);
        successMenu.SetActive(false);
    }

    private void OnEnable()
    {
        pauseButton.onClick.AddListener(Pause);
        questionButton.onClick.AddListener(Question);
        continueButton.onClick.AddListener(Continue);
        exitButton.onClick.AddListener(Exit);
        resumeButton.onClick.AddListener(Resume);
        successBackButton.onClick.AddListener(Exit);

        questionMenu.GetComponent<QuestionUISetting>().OnReadQuestionEvent += Continue;

        // 订阅鼠标进入事件
        _continueButtonPointerEnterHandler = () => UpdatePauseText("继续游戏");
        _exitButtonPointerEnterHandler = () => UpdatePauseText("返回主菜单");
        _resumeButtonPointerEnterHandler = () => UpdatePauseText("重置关卡");
        continueButton.OnPointerEnterEvent += _continueButtonPointerEnterHandler;
        exitButton.OnPointerEnterEvent += _exitButtonPointerEnterHandler;
        resumeButton.OnPointerEnterEvent += _resumeButtonPointerEnterHandler;


        // 订阅鼠标离开事件
        _continueButtonPointerExitHandler = () => UpdatePauseText("暂停");
        _exitButtonPointerExitHandler = () => UpdatePauseText("暂停");
        _resumeButtonPointerExitHandler = () => UpdatePauseText("暂停");
        continueButton.OnPointerExitEvent += _continueButtonPointerExitHandler;
        exitButton.OnPointerExitEvent += _exitButtonPointerExitHandler;
        resumeButton.OnPointerExitEvent += _resumeButtonPointerExitHandler;

        gameControls.OnSuccessLevelEvent += UpdateSuccessMenu;
    }

    private void OnDisable()
    {
        pauseButton.onClick.RemoveListener(Pause);
        questionButton.onClick.RemoveListener(Question);
        continueButton.onClick.RemoveListener(Continue);
        exitButton.onClick.RemoveListener(Exit);
        resumeButton.onClick.RemoveListener(Resume);
        successBackButton.onClick.RemoveListener(Exit);
        //questionMenu.GetComponent<QuestionUISetting>().OnReadQuestionEvent -= Continue;

        // 取消订阅鼠标进入事件
        continueButton.OnPointerEnterEvent -= _continueButtonPointerEnterHandler;
        exitButton.OnPointerEnterEvent -= _exitButtonPointerEnterHandler;
        resumeButton.OnPointerEnterEvent -= _resumeButtonPointerEnterHandler;

        // 取消订阅鼠标离开事件
        continueButton.OnPointerExitEvent -= _continueButtonPointerExitHandler;
        exitButton.OnPointerExitEvent -= _exitButtonPointerExitHandler;
        resumeButton.OnPointerExitEvent -= _resumeButtonPointerExitHandler;

        gameControls.OnSuccessLevelEvent -= UpdateSuccessMenu;
    }

    private async void Pause()
    {
        pauseMenu.SetActive(true);
        GameManager.Instance.Pause();
        await AudioSystem._Scripts.AudioSystem.Instance.FastAudioSource(clickType);
    }

    private async void Question()
    {
        questionMenu.SetActive(true);
        GameManager.Instance.Pause();
        await AudioSystem._Scripts.AudioSystem.Instance.FastAudioSource(clickType);
    }

    public void StartQuestion()
    {
        questionMenu.SetActive(true);
    }

    private async void UpdateSuccessMenu()
    {
        successText.text = "总计用时: " + gameControls.GameTime + "秒";

        successMenu.SetActive(true);
        await AudioSystem._Scripts.AudioSystem.Instance.FastAudioSource(successType);
    }

    private async void Continue()
    {
        questionMenu.SetActive(false);
        pauseMenu.SetActive(false);
        successMenu.SetActive(false);
        GameManager.Instance.Continue();
        await AudioSystem._Scripts.AudioSystem.Instance.FastAudioSource(clickType);
    }

    private async void Exit()
    {
        //todo: 切换至主菜单
        GameManager.Instance.Continue();
        loadSceneManager.LoadScene("Select");
        await AudioSystem._Scripts.AudioSystem.Instance.FastAudioSource(clickType);
    }

    private async void Resume()
    {
        //todo: 重置场景
        GameManager.Instance.Continue();
        loadSceneManager.LoadScene(SceneManager.GetActiveScene().name);

        await AudioSystem._Scripts.AudioSystem.Instance.FastAudioSource(clickType);
    }

    private void UpdatePauseText(string text)
    {
        pauseText.text = text;
    }
}