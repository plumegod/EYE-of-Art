public class SelectLoadLevel : MonoBehaviour
{
    private string _saveName;
    [SerializeField] private SaveData saveData;

    public int SaveLevel { get; private set; }

    private void Start()
    {
        _saveName = saveData.SaveName;

        LoadSave();
    }

    private void LoadSave()
    {
        try
        {
            if (!SaveSystem.CheckSaveIsExist(_saveName))
            {
                SaveSystem.NewSave(_saveName);
                SaveSystem.Save(_saveName, 0);
            }

            var saveData = SaveSystem.Load(_saveName);
            SaveLevel = (int)saveData[0];
            Debug.Log(_saveName + SaveLevel);
        }
        catch
        {
            SaveSystem.NewSave(_saveName);
            SaveSystem.Save(_saveName, 0);
        }
    }
}