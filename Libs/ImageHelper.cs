namespace Libs
{
    public class ImageHelper
    {
        public static async Task<string?> ConvertImageToBase64(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath) || !File.Exists(imagePath))
                return null;

            try
            {
                byte[] imageBytes = await File.ReadAllBytesAsync(imagePath);
                return Convert.ToBase64String(imageBytes);
            }
            catch
            {
                throw;
            }
        }
    }

}
