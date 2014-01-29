using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NyAppHelper.Model;
using NyAppWP.Resources;
using Windows.Foundation;
using Nokia.Graphics.Imaging;
using Windows.Storage.Streams;
using System.IO.IsolatedStorage;
using System.Runtime.InteropServices.WindowsRuntime;
using System.IO;
using System.Windows.Media.Imaging;

namespace NyAppWP.Pages.Camera
{

    public static class ImageHelper
    {

        public static readonly Size thumbnailMaxSize = new Size(142, 142);
        public static readonly Double thumbnailMaxArea = 5 * 200 * 200;
        public static readonly uint thumbnailMaxBytes = 2 * 200 * 200;

        public static readonly Size pvMaxSize = new Size(1000, 1000);
        public static readonly Double pvMaxArea = 5 * 1000 * 1000;
        public static readonly uint pvMaxBytes = 2 * 1000 * 1000;

        /// <summary>
        /// Calculate the thumbnail image size
        /// </summary>
        /// <param name="originalSize"></param>
        /// <returns></returns>
        public static Size CalculateSize(Size originalSize)
        {
            // Make sure that the image does not exceed the maximum size

            var width = originalSize.Width;
            var height = originalSize.Height;

            if (width > pvMaxSize.Width)
            {
                var scale = pvMaxSize.Width / width;

                width = width * scale;
                height = height * scale;
            }

            if (height > pvMaxSize.Height)
            {
                var scale = pvMaxSize.Height / height;

                width = width * scale;
                height = height * scale;
            }

            // Make sure that the image does not exceed the maximum area

            var originalPixels = width * height;

            if (originalPixels > pvMaxArea)
            {
                var scale = Math.Sqrt(pvMaxArea / originalPixels);

                width = originalSize.Width * scale;
                height = originalSize.Height * scale;
            }

            return new Size(width, height);
        }

        public static Rect CalculateRect(Size originalSize)
        {
            var width = originalSize.Width;
            var height = originalSize.Height;
            var length = width > height ? height : width;
            var x = width > height ? (width - length) / 2 : 0;
            var y = width > height ? 0 : (height - length) / 2;

            return new Rect(new Point(x,y) ,new Size(length, length));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="libraryPath"></param>
        /// <returns></returns>
        public static Stream LocalPhotoFromPath(String filePath)
        {

            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (store.FileExists(filePath))
                {
                    return store.OpenFile(filePath, FileMode.Open);
                }
            }

            return null;
        }

        /// <summary>
        /// delete localstorage image , dont forget delete after delete
        /// </summary>
        /// <param name="filePath"></param>
        public static void DeleteLocalPhoto(String filePath)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if(store.FileExists(filePath))
                {
                    store.DeleteFile(filePath);
                }
            }
        }

        public static async Task<IBuffer> Reframe(IBuffer image, Rect area)
        {
            using (var source = new BufferImageSource(image))
            using (var effect = new FilterEffect(source))
            {
                effect.Filters = new List<IFilter>()
                {
                    new ReframingFilter()
                    {
                        ReframingArea = area,
                    }
                };

                using (var renderer = new JpegRenderer(effect))
                {
                    return await renderer.RenderAsync();
                }
            }
        }


        /// <summary>
        /// Save the primitive image and thumbnail image and return the Photo model with local url
        /// </summary>
        /// <param name="imageBuffer"></param>
        /// <returns></returns>
        public static async Task<Photo> SaveImage(IBuffer imageBuffer)
        {
            try
            {
                Photo photo = new Photo
                {
                    CreatedTime = DateTime.UtcNow
                };

                AutoResizeConfiguration tbRC = null;
                AutoResizeConfiguration pvRC = null;
                Rect thumbnailRect;
                Size pvSize;
                
                using (var source = new BufferImageSource(imageBuffer))
                {
                    var info = await source.GetInfoAsync();
                    //var thumbnailSize = ImageHelper.CalculateSize(info.ImageSize);
                    
                    thumbnailRect = CalculateRect(info.ImageSize);
                    pvSize = CalculateSize(info.ImageSize);
                    tbRC = new AutoResizeConfiguration(ImageHelper.thumbnailMaxBytes, thumbnailMaxSize, new Size(0, 0), AutoResizeMode.Automatic, 0, ColorSpace.Yuv420);
                    pvRC = new AutoResizeConfiguration(ImageHelper.pvMaxBytes, pvSize, new Size(0, 0), AutoResizeMode.Automatic, 0, ColorSpace.Yuv420);
                }

                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    //Get LocalPath
                    var localPath = AppResources.LocalImagePath;

                    if (!store.DirectoryExists(localPath))
                    {
                        store.CreateDirectory(localPath);
                    }

                    //Save the primitive bitmap file
                    using (var file = store.CreateFile(photo.LocalUri = localPath + @"\" + photo.Tag + @"_Raw"))
                    {
                        using (var localImage = imageBuffer.AsStream())
                        {
                            localImage.CopyTo(file);
                            file.Flush();
                            localImage.Close();
                        }
                    }

                    //Save Preview Image
                    if (pvRC != null)
                    {
                        //Resize the Image to priview image

                        var pvBuffer = await Nokia.Graphics.Imaging.JpegTools.AutoResizeAsync(imageBuffer, pvRC);

                        //Save Preview Image
                        using (var file = store.CreateFile(localPath + @"\" + photo.Tag + @"_Preview.jpg"))
                        {
                            photo.PreviewDetailUri = file.Name;
                            ;
                            using (var localImage = pvBuffer.AsStream())
                            {
                                localImage.CopyTo(file);
                                file.Flush();
                                localImage.Close();
                            }
                        }
                    }

                    //Save thumbnail Image
                    if(tbRC != null)
                    {
                        //Crop to the square
                        var rb = await Reframe(imageBuffer, thumbnailRect);

                        //Resize the Image to thumbnail
                        var tb = await Nokia.Graphics.Imaging.JpegTools.AutoResizeAsync(rb, tbRC);

                        //Save thumbnail
                        using (var file = store.CreateFile(photo.LocalThumbnailUri = localPath + @"\" + photo.Tag + @"_Thumbnail.jpg"))
                        {
                            photo.ThumbnailDetailUri = file.Name;

                            using (var localImage = tb.AsStream())
                            {
                                localImage.CopyTo(file);
                                file.Flush();
                                localImage.Close();
                            }

                        }
                    }
                }

            return photo;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }

}
