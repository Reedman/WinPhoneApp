using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using NyAppHelper.Model;
using System.IO.IsolatedStorage;
using NyAppWP.Resources;

namespace NyAppWP.Pages.Camera
{
    public partial class ImageGalleryPage : PhoneApplicationPage
    {
        public DataContext.ImageListViewDataContext imageListDc = null;

        public ImageGalleryPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //PanoramaImage.DataContext = imageListDc;
            PanoramaImage.DataContext = imageListDc;

            string StartIndex = string.Empty;
            if (NavigationContext.QueryString.TryGetValue("StartIndex", out StartIndex))
            {
                PanoramaImage.SelectedItem = PanoramaImage.Items[int.Parse(StartIndex)];
            }

            base.OnNavigatedTo(e);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("是否确定删除图片","",MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                var p = PanoramaImage.SelectedItem as Photo;
                if( null != p)
                {
                    imageListDc.RemovePhoto(p);

                    using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        var localPath = AppResources.LocalImagePath;
                        var files = isolatedStorage.GetFileNames(localPath + "\\" + p.Tag + "_*");
                        foreach (string file in files)
                        {
                            isolatedStorage.DeleteFile(file);
                        }
                    }
                }
            }

        }
    }
}