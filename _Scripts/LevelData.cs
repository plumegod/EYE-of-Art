[CreateAssetMenu(menuName = "LevelData(关卡数据)", fileName = "levelData_")]
public class LevelData : ScriptableObject
{
    [field: SerializeField] public ImagesData LevelImage { get; }

    [field: SerializeField] public Vector3[] LevelVector { get; }
}