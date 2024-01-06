[CreateAssetMenu(menuName = "SaveData(存档数据)", fileName = "SaveData_")]
public class SaveData : ScriptableObject
{
    [field: SerializeField] public string SaveName { get; }
}