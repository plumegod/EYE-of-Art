[CreateAssetMenu(menuName = "QuestionData(提示信息)", fileName = "QuestionData_")]
public class QuestionData : ScriptableObject
{
    [field: SerializeField] public string QuestionText { get; }

    [field: SerializeField] public Sprite QuestionImage { get; }
}