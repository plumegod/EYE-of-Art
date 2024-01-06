using System.Collections;
using aPlume.Input;
using AudioSystem._Scripts.Data;

public class GameControls : MonoBehaviour
{
    private int _levelIndex;
    private LoadSceneManager _loadSceneManager;

    private MapControl _mapControl;
    private ProgressBarControl _progressBarControl;
    private RotateGameObject _rotateGameObject;
    private float _startTime;

    [Header("关卡数据")] [SerializeField] private LevelData levelData;

    //[SerializeField] private float zRotationThreshold = 12f;
    [Header("旋转速度")] [SerializeField] private readonly float rotationSpeed = 1f;

    [Header("角度差值")] [SerializeField] private readonly float rotationThreshold = 8f;

    [FormerlySerializedAs("saveName")] [SerializeField]
    private SaveData saveData;

    [SerializeField] private float saveLevel;
    [SerializeField] private AudioType successAudioType;


    public float GameTime { get; private set; }

    public event Action OnAngleReachedEvent;
    public event Action OnSuccessLevelEvent;

    private void Awake()
    {
        _progressBarControl = FindObjectOfType<ProgressBarControl>();
        _rotateGameObject = FindObjectOfType<RotateGameObject>();
        _mapControl = FindObjectOfType<MapControl>();
        _loadSceneManager = FindObjectOfType<LoadSceneManager>();

        _rotateGameObject.OnRotateObjectEvent += UpdateProgressBar;
        OnAngleReachedEvent += SuccessRotate;
        OnAngleReachedEvent += _progressBarControl.SuccessProgressBar;

        InitLevel();
    }

    private void OnDisable()
    {
        _rotateGameObject.OnRotateObjectEvent -= UpdateProgressBar;
        OnAngleReachedEvent -= SuccessRotate;
        OnAngleReachedEvent -= _progressBarControl.SuccessProgressBar;
    }

    private void InitLevel()
    {
        // 初始化参数
        _levelIndex = 0;
        GameTime = 0f;

        // 记录开始时间
        _startTime = Time.time;

        // 更新地图
        _mapControl.UpdateImagesData(levelData.LevelImage);
        _mapControl.UpdateImage(_levelIndex);
    }

    private async void UpdateLevel()
    {
        if (_levelIndex >= levelData.LevelVector.Length - 1)
        {
            EndLevel();
            return;
        }

        _levelIndex++;
        _progressBarControl.UpdateProgressBar(0);
        _mapControl.UpdateImage(_levelIndex);

        // 切换回游戏操作模式
        SceneInputManager.Instance.InputReader.SetGameplay();

        await AudioSystem._Scripts.AudioSystem.Instance.FastAudioSource(successAudioType);
    }

    private void EndLevel()
    {
        //todo: 保存游戏
        GameTime = Time.time - _startTime;

        var save = SaveSystem.Load(saveData.SaveName);
        var loadSaveLevel = save[0];
        if (loadSaveLevel < saveLevel)
            SaveSystem.Save(saveData.SaveName, saveLevel);

        OnSuccessLevelEvent?.Invoke();
    }

    // 更新进度条
    private void UpdateProgressBar()
    {
        _progressBarControl.UpdateProgressBar(CheckRotateProgress());
    }

    // 检查旋转进度
    private int CheckRotateProgress()
    {
        Vector3 currentRotationAngles = _rotateGameObject.Rotation; // 获取当前旋转角度

        currentRotationAngles.x = Mathf.Repeat(currentRotationAngles.x + 180f, 360f) - 180f;
        currentRotationAngles.y = Mathf.Repeat(currentRotationAngles.y + 180f, 360f) - 180f;
        currentRotationAngles.z = Mathf.Repeat(currentRotationAngles.z + 180f, 360f) - 180f;

        // 检查每个轴上的角度差值
        float xAngleDifference = Quaternion.Angle(Quaternion.Euler(currentRotationAngles.x, 0f, 0f),
            Quaternion.Euler(levelData.LevelVector[_levelIndex].x, 0f, 0f));
        float yAngleDifference = Quaternion.Angle(Quaternion.Euler(0f, currentRotationAngles.y, 0f),
            Quaternion.Euler(0f, levelData.LevelVector[_levelIndex].y, 0f));
        float zAngleDifference = Quaternion.Angle(Quaternion.Euler(0f, 0f, currentRotationAngles.z),
            Quaternion.Euler(0f, 0f, levelData.LevelVector[_levelIndex].z));

        Vector3 angleDifference = levelData.LevelVector[_levelIndex];

        angleDifference.x = Mathf.Repeat(angleDifference.x + 180f, 360f) - 180f;
        angleDifference.y = Mathf.Repeat(angleDifference.y + 180f, 360f) - 180f;

        var xAngleReachedResume = Mathf.Abs(angleDifference.x) < rotationThreshold;
        var yAngleReachedResume = Mathf.Abs(angleDifference.y) < rotationThreshold;


        // 检查每个轴上的角度差值是否小于阈值
        var xAngleReached = xAngleDifference < rotationThreshold;
        var yAngleReached = yAngleDifference < rotationThreshold;
        var zAngleReached = zAngleDifference < rotationThreshold;

        Debug.Log(angleDifference + " " + currentRotationAngles);

        // 如果所有轴上的角度差值都小于阈值，则触发事件（不判断z轴）
        if ((xAngleReached && yAngleReached) || (xAngleReachedResume && yAngleReachedResume))
        {
            // 切换操作模式，禁止玩家旋转
            SceneInputManager.Instance.InputReader.SetUI();
            _progressBarControl.SuccessProgressBar();

            OnAngleReachedEvent?.Invoke();
        }

        // 检查每个轴上的旋转百分比
        var xRotationPercentage = (1.0f - xAngleDifference / rotationThreshold) * 100f;
        var yRotationPercentage = (1.0f - yAngleDifference / rotationThreshold) * 100f;
        var zRotationPercentage = (1.0f - zAngleDifference / rotationThreshold) * 100f;

        // 将百分比限制在0到100之间
        xRotationPercentage = Mathf.Clamp(xRotationPercentage, 0f, 100f);
        yRotationPercentage = Mathf.Clamp(yRotationPercentage, 0f, 100f);
        zRotationPercentage = Mathf.Clamp(zRotationPercentage, 0f, 100f);

        // 计算平均旋转百分比
        var rotationPercentage = (xRotationPercentage + yRotationPercentage + zRotationPercentage) / 3;

        // 返回百分比
        return (int)rotationPercentage;
    }

    // 旋转到阈值后，进入功能保证旋转成功
    private IEnumerator RotateCoroutine(Vector3 currentRotation, Vector3 finishRotation, Action rotationCompleteAction)
    {
        var timeElapsed = 0.0f;

        while (timeElapsed < 1.0f)
        {
            timeElapsed += Time.deltaTime * rotationSpeed;
            //Quaternion targetRotation = Quaternion.Euler(Vector3.Lerp(currentRotation, finishRotation, timeElapsed));
            Quaternion targetRotation = Quaternion.Slerp(Quaternion.Euler(currentRotation),
                Quaternion.Euler(finishRotation), timeElapsed);


            _rotateGameObject.UpdateRotate(targetRotation);

            yield return null;
        }

        rotationCompleteAction?.Invoke();
    }

    // 旋转成功
    private void SuccessRotate()
    {
        Vector3 currentRotation = _rotateGameObject.Rotation;
        Vector3 finishRotation = levelData.LevelVector[_levelIndex];

        StartCoroutine(RotateCoroutine(currentRotation, finishRotation, () =>
        {
            Debug.Log("旋转完成！");
            //todo: 在旋转完成后执行的操作
            UpdateLevel();
        }));
    }
}