public class SelectMenu : MonoBehaviour
{
    private int _levelIndex;

    private GameObject currentGameObject;
    [SerializeField] private Button leftButton;
    private GameObject[] levelObject;

    [Header("关卡")] [SerializeField] private GameObject[] levelObjectPrefab;
    [SerializeField] private Button rightButton;

    [SerializeField] private Transform rotationCenter;

    [Header("设置")] [SerializeField] private readonly float rotationRadius = 5f;
    private Vector3 targetPosition;

    private void Start()
    {
        SpawnObjects();
        SwitchObject(0);
        levelObject = new GameObject[levelObjectPrefab.Length];

        leftButton.onClick.AddListener(SwitchToLeft);
        rightButton.onClick.AddListener(SwitchToRight);
    }

    private void SpawnObjects()
    {
        foreach (var objPrefab in levelObjectPrefab)
        {
            Vector3 spawnPosition = rotationCenter.position +
                                    Quaternion.Euler(0, _levelIndex * 360.0f / levelObjectPrefab.Length, 0) *
                                    Vector3.forward * rotationRadius;
            GameObject spawnedObject = Instantiate(objPrefab, spawnPosition, Quaternion.identity);
            spawnedObject.transform.parent = transform;
            _levelIndex++;
            levelObject[_levelIndex] = spawnedObject;
        }
    }

    public void SwitchToLeft()
    {
        _levelIndex = (_levelIndex - 1 + levelObjectPrefab.Length) % levelObjectPrefab.Length;
        SwitchObject(_levelIndex);
    }

    public void SwitchToRight()
    {
        _levelIndex = (_levelIndex + 1) % levelObjectPrefab.Length;
        SwitchObject(_levelIndex);
    }

    private void SwitchObject(int index)
    {
        targetPosition = rotationCenter.position + Quaternion.Euler(0, index * 360.0f / levelObjectPrefab.Length, 0) *
            Vector3.forward * rotationRadius;
        currentGameObject = transform.GetChild(index).gameObject;
    }
}