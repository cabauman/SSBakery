namespace SSBakeryAdmin.Services.Interfaces
{
    public interface IMediaService
    {
        byte[] ResizeImage(byte[] imageData, float width, float height);
    }
}