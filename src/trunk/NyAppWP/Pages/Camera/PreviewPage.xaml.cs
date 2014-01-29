/*
 * Copyright © 2012-2013 Nokia Corporation. All rights reserved.
 * Nokia and Nokia Connecting People are registered trademarks of Nokia Corporation. 
 * Other product and company names mentioned herein may be trademarks
 * or trade names of their respective owners. 
 * See LICENSE.TXT for license information.
 */

using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework.Media;
using System;
using System.IO.IsolatedStorage;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Runtime.InteropServices.WindowsRuntime;

using NyAppHelper.Model;
using System.Threading.Tasks;
using Windows.Foundation;
using NyAppWP.Pages;

namespace NyAppWP.Pages.Camera
{
    /// <summary>
    /// Preview page displays the captured photo from DataContext.ImageStream and
    /// has a button to save the image to phone's camera roll.
    /// </summary>
    public partial class PreviewPage : PhoneApplicationPage
    {
        private CameraDataContext _dataContext = CameraDataContext.Singleton;
        private BitmapImage _bitmap = new BitmapImage();
        public Photo photo = null;

        public PreviewPage()
        {
            InitializeComponent();

            DataContext = _dataContext;
        }

        /// <summary>
        /// When navigating to this page, DataContext.ImageStream will be set as the source
        /// for the Image control in XAML. If ImageStream is null, application will navigate
        /// directly back to the main page.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (_dataContext.ImageStream != null)
            {
                _bitmap.SetSource(_dataContext.ImageStream);
                image.Source = _bitmap;
            }
            else
            {
                NavigationService.GoBack();
            }

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // NavigationEventArgs returns destination page
            if (e.Content is PivotPageMockup)
            {
                PivotPageMockup destinationPage = e.Content as PivotPageMockup;

                if (destinationPage != null && photo != null)
                {
                    destinationPage.imageListDc.AddPhoto(photo);
                }
            }
            else if(e.Content is PoiInfoPage)
            {
                var page = e.Content as PoiInfoPage;
                if(page!=null&&photo!=null)
                {
                    page.AddPhotoToList(photo);
                }
            }

        }

        

        /// <summary>
        /// Clicking on the save button saves the photo in DataContext.ImageStream to media library
        /// camera roll. Once image has been saved, the application will navigate back to the main page.
        /// </summary>
        private async void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Reposition ImageStream to beginning, because it has been read already in the OnNavigatedTo method.
                _dataContext.ImageStream.Position = 0;
                var imageBuffer = WindowsRuntimeBufferExtensions.AsBuffer(_dataContext.ImageStream.GetBuffer());

                photo = await ImageHelper.SaveImage(imageBuffer);

                GC.Collect();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Saving picture to camera roll failed: " + ex.HResult.ToString("x8") + " - " + ex.Message);
            }

            NavigationService.RemoveBackEntry();
            NavigationService.GoBack();
            
        }

        private void DiscardButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}