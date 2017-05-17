namespace SMAS.Web.Core.Configuration
{
    public class ImageWidthConstants
    {
        public static int ImageSm = 320;
        public static int ImageMd = 640;
        public static int ImageLg = 1080;

        public static int[] GetAll()
        {
            return new[] {ImageSm, ImageMd, ImageLg};
        }
    }
}
