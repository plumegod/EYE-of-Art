[CreateAssetMenu(menuName = "ImageData(图片组)", fileName = "ImageData_")]
public class ImagesData : ScriptableObject
{
    [field: SerializeField] public Sprite[] Image { get; }
}