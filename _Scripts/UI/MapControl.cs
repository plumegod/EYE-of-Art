public class MapControl : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private ImagesData images;

    public void UpdateImagesData(ImagesData imagesData)
    {
        images = imagesData;
    }

    public void UpdateImage(int index)
    {
        image.sprite = images.Image[index];
    }
}