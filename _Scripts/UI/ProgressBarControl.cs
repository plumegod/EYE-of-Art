public class ProgressBarControl : MonoBehaviour
{
    private Image _image;
    [SerializeField] private ImagesData images;

    private void Start()
    {
        _image = GetComponent<Image>();
    }

    public void UpdateProgressBar(int percentage)
    {
        // 将百分比限制在0到4之间
        //percentage = Mathf.Clamp(percentage, 0, images.Image.Length - 1);
        //mappedValue = (inputValue - inputMin) * (outputMax - outputMin) / (inputMax - inputMin) + outputMin
        percentage = (percentage - 0) * (images.Image.Length - 1 - 0) / (100 - 0) + 0;

        // 提高玩家对进度条完成度的认知
        _image.sprite = images.Image[percentage + 1];
    }

    public void SuccessProgressBar()
    {
        _image.sprite = images.Image[^1];
    }
}