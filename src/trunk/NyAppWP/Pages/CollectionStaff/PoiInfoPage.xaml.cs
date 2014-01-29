using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Globalization;
using NyAppHelper.Data;
using NyAppHelper.Model;
using System.Windows.Media;
using System.IO;
using System.Xml;
using System.Windows.Markup;
using NyAppWP.Controls;
using NyAppWP.DataContext;
using System.Collections.ObjectModel;
using NyAppWP.Controls.ExpanderSelector;
using NyAppHelper.Http;
using System.IO.IsolatedStorage;
using NyAppWP.Resources;
using NyAppWP.Pages.Camera;
using NyAppHelper.Location;


namespace NyAppWP.Pages
{
    public partial class PoiInfoPage : PhoneApplicationPage
    {

        #region 病虫草害计数索引

        /// <summary>
        /// 病害计数索引
        /// </summary>
        private int _pestIndex1 = 0;
        /// <summary>
        /// 虫害计数索引
        /// </summary>
        private int _pestIndex2 = 0;
        /// <summary>
        /// 草害计数索引
        /// </summary>
        /// </summary>
        private int _pestIndex3 = 0;
        /// <summary>
        /// 异常计数索引
        /// </summary>
        /// </summary>
        private int _pestIndex4 = 0;

        #endregion

        #region 私有变量

        private int _taskId;
        private CollectionTask _taskInfo;
        private CollectionTaskDataAccessLayer _taskDataAccessLayer;
        private CollectionTaskResultDataAccessLayer _resultDataAccessLayer;
        private CollectionTaskNatureDataAccessLayer _natureDataAccessLayer;
        private CollectionTaskPestViewDataAccessLayer _pestDataAccessLayer;
        private PhotoDataAccessLayer _photoDataAccessLayer;
        private PestDataContext _pestDataContext;
        private ObservableCollection<Pest> _pestInfo;
        private List<CollectionTaskResultDisease> _diseaseList;
        private List<CollectionTaskResultPest> _pestList;
        private List<CollectionTaskResultWeed> _weedList;
        private CollectionTaskResult _taskResult;
        private CollectionTaskService _service;
        private TaskException _taskException;
        private Dictionary<int, ImageListViewDataContext> _imageDataDic;
        private int _photoListIndex;
        private int _photoType;

        #endregion

        /// <summary>
        /// 将拍摄的照片添加到照片列表中
        /// </summary>
        /// <param name="photo"></param>
        public void AddPhotoToList(Photo photo)
        {
            photo.ExtensionField1 = _photoType;
            _imageDataDic[_photoListIndex].AddPhoto(photo);
        }

        public PoiInfoPage()
        {
            InitializeComponent();
            _taskDataAccessLayer = new CollectionTaskDataAccessLayer();
            _resultDataAccessLayer = new CollectionTaskResultDataAccessLayer();
            _pestDataAccessLayer = new CollectionTaskPestViewDataAccessLayer();
            _natureDataAccessLayer = new CollectionTaskNatureDataAccessLayer();
            _photoDataAccessLayer = new PhotoDataAccessLayer();
            _service = new CollectionTaskService();
            _pestDataContext = new PestDataContext();
            ((PivotItem)(tabList.Items[0])).Header = "基本信息";
            var esBorder = ((Border)(esPest.LayoutRoot.Children[0]));
            esBorder.Background = new SolidColorBrush(Color.FromArgb(255, 187, 122, 54));
            ((TextBlock)esBorder.Child).Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            _diseaseList = new List<CollectionTaskResultDisease>();
            _pestList = new List<CollectionTaskResultPest>();
            _weedList = new List<CollectionTaskResultWeed>();
            _taskResult = new CollectionTaskResult();
            _taskException = new TaskException();
            _imageDataDic = new Dictionary<int, ImageListViewDataContext>();
        }

        private void SubTab_NavigateToPageEvent(object sender, EventArgs e)
        {
            NavigationService.Navigate(((NavigationEventArgs)e).Uri);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (e.Content is ImageGalleryPage)
            {
                var gallery = e.Content as ImageGalleryPage;
                if (gallery != null)
                    gallery.imageListDc = _imageDataDic[_photoListIndex];
            }
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                _taskId = int.Parse(NavigationContext.QueryString["tid"].ToString().Trim());
                _taskInfo = _taskDataAccessLayer.Get(_taskId);
                _taskInfo.NatureInfoList = _natureDataAccessLayer.GetAll(_taskId).ToList<CollectionTaskNature>();
                _taskInfo.PestList = _pestDataAccessLayer.GetAll(_taskId).ToList<CollectionTaskPestView>();
                initTaskDetail();
            }
        }

        private void initTaskDetail()
        {
            txtPestType.Text = initPestTypeBox(_taskInfo.PestList);
            txtPlantCate.Text = _taskInfo.CropName;
            txtPlantMethod.Text = _taskInfo.PlantingMethod.HasValue ? FieldPlantingMethod.PlantingMethodDic[(PlantingMethodEnum)_taskInfo.PlantingMethod] : FieldPlantingMethod.PlantingMethodDic[PlantingMethodEnum.None];
            txtWorkDuration.Text = String.Concat(_taskInfo.CollectionStartTime.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture), " - ", _taskInfo.CollectionEndTime.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture));
            initTaskPestTypeDetail(_taskInfo.PestList);
            initTaskNatureDetail(_taskInfo.NatureInfoList);
        }

        /// <summary>
        /// 初始化任务
        /// </summary>
        /// <param name="pestTypeStr"></param>
        /// <returns></returns>
        private string initPestTypeBox(List<CollectionTaskPestView> list)
        {
            var pestType = String.Empty;
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (i == (list.Count - 1))
                    {
                        pestType += list[i].PestName;
                    }
                    else
                    {
                        pestType += list[i].PestName + "；";
                    }
                }
            }
            return pestType;
        }

        /// <summary>
        /// 初始化采集任务自然信息
        /// </summary>
        /// <param name="list"></param>
        private void initTaskNatureDetail(List<CollectionTaskNature> list)
        {
            foreach (var item in list)
            {
                switch (item.Name.ToLowerInvariant())
                {
                    case ("airtemp"):
                        {
                            txtAirTemperature.Text = "必须填写";
                            break;
                        }
                    case ("airhumidity"):
                        {
                            txtAirHumidity.Text = "必须填写";
                            break;
                        }
                    case ("soiltemp"):
                        {
                            txtSoilTemperature.Text = "必须填写";
                            break;
                        }
                    case ("soilhumidity"):
                        {
                            txtSoilMoisture.Text = "必须填写";
                            break;
                        }
                    case ("soilph"):
                        {
                            txtSoilPH.Text = "必须填写";
                            break;
                        }
                    case ("soilion"):
                        {
                            txtSoilIon.Text = "必须填写";
                            break;
                        }
                    case ("soilfertility"):
                        {
                            txtSoilFertility.Text = "必须填写";
                            break;
                        }
                    case ("leaftemp"):
                        {
                            txtLeafTemp.Text = "必须填写";
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// 初始化病虫草害采集Tab
        /// </summary>
        /// <param name="list"></param>
        private void initTaskPestTypeDetail(List<CollectionTaskPestView> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                string itemTitle = String.Empty;
                //病害
                if (item.PestType == 1)
                {
                    _pestIndex1++;
                    var result = new CollectionTaskResultDisease();
                    result.TabID = _pestIndex2;
                    result.PestID = item.PestID;
                    result.TaskId = _taskId;
                    itemTitle = "病害" + _pestIndex1;
                    var content = new DiseasePestTab(item.PestName, result);
                    tabList.Items.Add(new PivotItem()
                    {
                        Header = itemTitle,
                        Content = content,
                    });
                    _diseaseList.Add(result);
                    _imageDataDic.Add(item.PestID, content.ImgDataContext);
                    content.EventForMoveToGalleryPage += new EventHandler(SubTab_NavigateToPageEvent);
                }
                //虫害
                else if (item.PestType == 0)
                {
                    _pestIndex2++;
                    itemTitle = "虫害" + _pestIndex2;
                    var result = new CollectionTaskResultPest();
                    result.TabID = _pestIndex2;
                    result.PestID = item.PestID;
                    result.TaskId = _taskId;
                    var content = new InsectPestTab(item.PestName, result);
                    tabList.Items.Add(new PivotItem()
                    {
                        Header = itemTitle,
                        Content = content,
                    });
                    _pestList.Add(result);
                    _imageDataDic.Add(item.PestID, content.ImgDataContext);
                    content.EventForMoveToGalleryPage += new EventHandler(SubTab_NavigateToPageEvent);
                }
                //草害
                else if (item.PestType == 2)
                {
                    _pestIndex3++;
                    var result = new CollectionTaskResultWeed();
                    result.TabID = _pestIndex3;
                    result.PestID = item.PestID;
                    result.TaskId = _taskId;
                    itemTitle = "草害" + _pestIndex3;
                    var content = new WeedPestTab(item.PestName, result);
                    tabList.Items.Add(new PivotItem()
                    {
                        Header = itemTitle,
                        Content = content,
                    });
                    _weedList.Add(result);
                    _imageDataDic.Add(item.PestID, content.ImgDataContext);
                    content.EventForMoveToGalleryPage += new EventHandler(SubTab_NavigateToPageEvent);
                }
                //异常
                else
                {
                    _pestIndex4++;
                    itemTitle = "异常" + _pestIndex4;
                    _taskException.ExceptionID = 0;
                    var content = new ExceptionPestTab(_taskException);
                    tabList.Items.Add(new PivotItem()
                    {
                        Header = itemTitle,
                        Content = content,
                    });
                    _imageDataDic.Add(_taskException.ExceptionID.Value, content.ImgDataContext);
                    content.EventForMoveToGalleryPage += new EventHandler(SubTab_NavigateToPageEvent);
                }
            }
        }

        private void btnShowReq_Click(object sender, RoutedEventArgs e)
        {
            this.ContentPanel.Children.Add(new CollectionRequirementPopup(_taskId));
        }

        private void btnAddPestType_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var button = (Button)sender;
            var selectedType = ((ListPickerItem)lpPestType.SelectedItem).TabIndex;
            if (caPestType.Visibility == Visibility.Collapsed)
            {
                if (_pestInfo == null)
                {
                    _pestDataContext.GetAllPests((data) =>
                    {
                        _pestInfo = data;
                        var pestList = _pestInfo.Where(p => p.PestType == selectedType).ToList<object>();
                        esPest.DataContext = new ExpanderSelectorDataContext(pestList);
                        caPestType.Visibility = Visibility.Visible;
                    });
                }
                else
                {
                    var pestList = _pestInfo.Where(p => p.PestType == selectedType).ToList<object>();
                    esPest.DataContext = new ExpanderSelectorDataContext(pestList);
                    caPestType.Visibility = Visibility.Visible;
                }
                button.Background = new System.Windows.Media.SolidColorBrush(Color.FromArgb(255, 122, 122, 122));
            }
            else
            {
                caPestType.Visibility = Visibility.Collapsed;
                button.Background = new System.Windows.Media.SolidColorBrush(Color.FromArgb(255, 0, 128, 0));
            }
        }

        private void lpPestType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_pestInfo != null)
            {
                var selectedType = ((ListPicker)sender).SelectedIndex;
                if (selectedType < 3)
                {
                    esPest.Visibility = Visibility.Visible;
                    var pestList = _pestInfo.Where(p => p.PestType == selectedType).ToList<object>();
                    esPest.DataContext = new ExpanderSelectorDataContext(pestList);
                }
                else
                {
                    esPest.Visibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// 动态添加病虫害
        /// 异常只能添加一次
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddPestTab_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string tabTitle = txtPestTitle.Text;
            var selectedPestType = (ExpanderSelectorDataSource)esPest.Seleted;
            int pestType = ((ListPickerItem)lpPestType.SelectedItem).TabIndex;
            switch (pestType)
            {
                case (1):
                    {
                        _pestIndex1++;
                        var index = _pestIndex2 + _pestIndex1;
                        var result = new CollectionTaskResultDisease();
                        result.TabID = index;
                        result.PestID = int.Parse(selectedPestType.ID);
                        result.TaskId = _taskId;
                        var content = new DiseasePestTab(selectedPestType.Name, result);
                        tabList.Items.Insert(index, new PivotItem()
                        {
                            Header = tabTitle,
                            Content = content,
                        });
                        _diseaseList.Add(result);
                        _imageDataDic.Add(int.Parse(selectedPestType.ID), content.ImgDataContext);
                        tabList.SelectedIndex = index;
                        content.EventForMoveToGalleryPage += new EventHandler(SubTab_NavigateToPageEvent);
                        break;
                    }
                //虫害
                case (0):
                    {
                        _pestIndex2++;
                        var result = new CollectionTaskResultPest();
                        result.TabID = _pestIndex2;
                        result.PestID = int.Parse(selectedPestType.ID);
                        result.TaskId = _taskId;
                        var content = new InsectPestTab(selectedPestType.Name, result);
                        tabList.Items.Insert(_pestIndex2, new PivotItem()
                        {
                            Header = tabTitle,
                            Content = content,
                        });
                        _pestList.Add(result);
                        _imageDataDic.Add(int.Parse(selectedPestType.ID), content.ImgDataContext);
                        tabList.SelectedIndex = _pestIndex2;
                        content.EventForMoveToGalleryPage += new EventHandler(SubTab_NavigateToPageEvent);
                        break;
                    }
                //草害
                case (2):
                    {
                        _pestIndex3++;
                        var index = _pestIndex2 + _pestIndex1 + _pestIndex3;
                        var result = new CollectionTaskResultWeed();
                        result.TabID = index;
                        result.PestID = int.Parse(selectedPestType.ID);
                        result.TaskId = _taskId;
                        var content = new WeedPestTab(selectedPestType.Name, result);
                        tabList.Items.Insert(index, new PivotItem()
                        {
                            Header = tabTitle,
                            Content = content,
                        });
                        _weedList.Add(result);
                        _imageDataDic.Add(int.Parse(selectedPestType.ID), content.ImgDataContext);
                        tabList.SelectedIndex = index;
                        content.EventForMoveToGalleryPage += new EventHandler(SubTab_NavigateToPageEvent);
                        break;
                    }
                //异常
                case (3):
                    {
                        if (!_taskException.ExceptionID.HasValue)
                        {
                            _pestIndex4++;
                            var index = _pestIndex2 + _pestIndex1 + _pestIndex3 + _pestIndex4;
                            _taskException.ExceptionID = 0;
                            var content = new ExceptionPestTab(_taskException);
                            tabList.Items.Insert(index, new PivotItem()
                            {
                                Header = tabTitle,
                                Content = content,
                            });
                            _imageDataDic.Add(_taskException.ExceptionID.Value, content.ImgDataContext);
                            tabList.SelectedIndex = index;
                            content.EventForMoveToGalleryPage += new EventHandler(SubTab_NavigateToPageEvent);
                        }
                        break;
                    }
            }
            txtPestTitle.Text = String.Empty;
            esPest.DefaultValue = "请选择";
            caPestType.Visibility = Visibility.Collapsed;
            btnAddPestType.Background = new System.Windows.Media.SolidColorBrush(Color.FromArgb(255, 0, 128, 0));
        }

        private void btnCancelPestTab_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            txtPestTitle.Text = String.Empty;
            esPest.DefaultValue = "请选择";
            caPestType.Visibility = Visibility.Collapsed;
            btnAddPestType.Background = new System.Windows.Media.SolidColorBrush(Color.FromArgb(255, 0, 128, 0));
        }

        private void txtPestTitle_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.Background = new SolidColorBrush(Color.FromArgb(255, 187, 122, 54));
            textBox.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        }

        private void txtPestTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.Background = new SolidColorBrush(Color.FromArgb(255, 187, 122, 54));
            textBox.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        }

        /// <summary>
        /// 回到首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHome_Click(object sender, EventArgs e)
        {
            string hubPage = @"/Pages/CollectionStaff/PickerHubPage.xaml";
            NavigationService.Navigate(new Uri(hubPage, UriKind.Relative));
        }

        /// <summary>
        /// 保存采集结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            _taskResult.TaskId = _taskId;
            //_taskResult.CollectionLatitude = "131.22";
            //_taskResult.CollectionLongitude = "22.31";
            _taskResult.FarmlandID = _taskInfo.FarmlandID;
            for (int i = 1; i < tabList.Items.Count; i++)
            {
                var content = ((PivotItem)tabList.Items[i]).Content;
                if (content is ITabInfoCanBeSaved)
                {
                    if (!((ITabInfoCanBeSaved)content).IsInputValueVailed())
                    {
                        MessageBox.Show("采集数据数据未填写完整，请检查");
                        return;
                    }
                }
            }
            if (_taskException.ExceptionID.HasValue)
            {
                _taskResult.ExceptionID = _taskException.ExceptionID;
                _taskResult.Exception = _taskException.Exception;
            }
            _taskResult.CollectionResultPestList = _pestList;
            _taskResult.CollectionResultWeedList = _weedList;
            _taskResult.CollectionResultDiseaseList = _diseaseList;
            int defaultIntValue = 0;
            float defaultFloatValue = 0;
            _taskResult.AirTemperature = float.TryParse(txtAirTemperature.Text, out defaultFloatValue) ? float.Parse(txtAirTemperature.Text) : defaultFloatValue;
            _taskResult.AirHumidity = float.TryParse(txtAirHumidity.Text, out defaultFloatValue) ? float.Parse(txtAirHumidity.Text) : defaultFloatValue;
            _taskResult.SoilFertility = float.TryParse(txtSoilFertility.Text, out defaultFloatValue) ? float.Parse(txtSoilFertility.Text) : defaultFloatValue;
            _taskResult.SoilIon = int.TryParse(txtSoilIon.Text, out defaultIntValue) ? int.Parse(txtSoilIon.Text) : defaultIntValue;
            _taskResult.SoilMoisture = float.TryParse(txtSoilMoisture.Text, out defaultFloatValue) ? float.Parse(txtSoilMoisture.Text) : defaultFloatValue;
            _taskResult.SoilPH = float.TryParse(txtSoilPH.Text, out defaultFloatValue) ? float.Parse(txtSoilPH.Text) : defaultFloatValue;
            _taskResult.SoilTemperature = float.TryParse(txtSoilTemperature.Text, out defaultFloatValue) ? float.Parse(txtSoilTemperature.Text) : defaultFloatValue;
            _taskResult.LeafTemp = float.TryParse(txtLeafTemp.Text, out defaultFloatValue) ? float.Parse(txtLeafTemp.Text) : defaultFloatValue;
            _taskResult.Remark = String.IsNullOrEmpty(txtRemark.Text) ? null : txtRemark.Text;
            if (_taskResult.UniqueId == 0)
            {
                if (_resultDataAccessLayer.Add(_taskResult))
                {
                    var tempId = _taskResult.UniqueId;
                    foreach (int key in _imageDataDic.Keys)
                    {
                        var images = _imageDataDic[key].ImageList;
                        for (int i = 0; i < images.Count; i++)
                        {
                            var photo = images[i];
                            photo.ID = tempId;
                        }
                        _photoDataAccessLayer.AddAll(images);
                    }
                    MessageBox.Show("保存成功");
                    tabList.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("保存成功");
                }
            }
            else
            {
                if (_resultDataAccessLayer.Update(_taskResult))
                {
                    var tempId = _taskResult.UniqueId;
                    if (hasImages())
                    {
                        _photoDataAccessLayer.RemoveAll(tempId);
                    }
                    foreach (int key in _imageDataDic.Keys)
                    {
                        var images = _imageDataDic[key].ImageList;
                        if (images.Count > 0)
                        {
                            for (int i = 0; i < images.Count; i++)
                            {
                                var photo = images[i];
                                photo.ID = tempId;
                            }
                            _photoDataAccessLayer.AddAll(images);
                        }
                    }
                    MessageBox.Show("保存成功");
                    tabList.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("保存失败");
                }
            }
        }

        /// <summary>
        /// 上传采集结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            if (_taskResult.UniqueId <= 0)
            {
                MessageBox.Show("未保存的结果不能提交");
            }
            else
            {
                var position = await LocationHelper.GetLocation();
                _taskResult.CollectionLatitude = position.Coordinate.Latitude.ToString();
                _taskResult.CollectionLongitude = position.Coordinate.Longitude.ToString();
                SystemTray.ProgressIndicator = new ProgressIndicator();
                SetProgressIndicator(true);
                SystemTray.ProgressIndicator.Text = "开始提交";
                bool result = await _service.UploadResult(_taskResult);
                if (result)
                {
                    if (_resultDataAccessLayer.Update(_taskResult))
                    {
                        SystemTray.ProgressIndicator.Text = "数据提交成功，开始检查照片";
                        var resultId = _taskResult.CollectionResultID;
                        var pestResults = _taskResult.CollectionResultPestList;
                        var diseaseResults = _taskResult.CollectionResultDiseaseList;
                        var weedResults = _taskResult.CollectionResultWeedList;
                        int photoIndex = 1;
                        if(pestResults!=null&&pestResults.Count>0)
                        {
                            foreach(var pestResult in pestResults)
                            {
                                var photoResultId=pestResult.PestCollectionResultID;
                                var pestId = pestResult.PestID;
                                var photoList = _imageDataDic[pestId].ImageList;
                                if(photoList.Count>0)
                                {
                                   foreach(var photo in photoList)
                                   {
                                       photo.ID = photoResultId;
                                       if (await _service.UploadResultImage(photo))
                                       {
                                            using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                                            {
                                                var localPath = AppResources.LocalImagePath;
                                                var files = isolatedStorage.GetFileNames(localPath + "\\" + photo.Tag + "_*");
                                                foreach (string file in files)
                                                {
                                                    isolatedStorage.DeleteFile(file);
                                                }
                                            }
                                            //上传成功，删除数据库图片信息
                                            _photoDataAccessLayer.Remove(photo.UniqueId);
                                            SystemTray.ProgressIndicator.Text = "照片" + photoIndex + "提交成功";
                                       }
                                       else
                                       {
                                           SystemTray.ProgressIndicator.Text = "照片" + photoIndex + "提交失败，传输下一张照片";
                                       }
                                       photoIndex++;
                                   }
                                }
                            }
                        }
                        if (diseaseResults!=null&&diseaseResults.Count>0)
                        {
                            foreach (var diseaseResult in diseaseResults)
                            {
                                var photoResultId = diseaseResult.DiseaseCollectionResultID;
                                var pestId = diseaseResult.PestID;
                                var photoList = _imageDataDic[pestId].ImageList;
                                if (photoList.Count > 0)
                                {
                                    foreach (var photo in photoList)
                                    {
                                        photo.ID = photoResultId;
                                        if (await _service.UploadResultImage(photo))
                                        {
                                            using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                                            {
                                                var localPath = AppResources.LocalImagePath;
                                                var files = isolatedStorage.GetFileNames(localPath + "\\" + photo.Tag + "_*");
                                                foreach (string file in files)
                                                {
                                                    isolatedStorage.DeleteFile(file);
                                                }
                                            }
                                            //上传成功，删除数据库图片信息
                                            _photoDataAccessLayer.Remove(photo.UniqueId);
                                            SystemTray.ProgressIndicator.Text = "照片"+photoIndex+"提交成功";
                                        }
                                        else
                                        {
                                            SystemTray.ProgressIndicator.Text = "照片" + photoIndex + "提交失败，传输下一张照片";
                                        }
                                        photoIndex++;
                                    }
                                }
                            }
                        }
                        if (weedResults!=null&&weedResults.Count > 0)
                        {
                            foreach (var weedResult in weedResults)
                            {
                                var photoResultId = weedResult.WeedCollectionResultID;
                                var pestId = weedResult.PestID;
                                var photoList = _imageDataDic[pestId].ImageList;
                                if (photoList.Count > 0)
                                {
                                    foreach (var photo in photoList)
                                    {
                                        photo.ID = photoResultId;
                                        if (await _service.UploadResultImage(photo))
                                        {
                                            using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                                            {
                                                var localPath = AppResources.LocalImagePath;
                                                var files = isolatedStorage.GetFileNames(localPath + "\\" + photo.Tag + "_*");
                                                foreach (string file in files)
                                                {
                                                    isolatedStorage.DeleteFile(file);
                                                }
                                            }
                                            //上传成功，删除数据库图片信息
                                            _photoDataAccessLayer.Remove(photo.UniqueId);
                                            SystemTray.ProgressIndicator.Text = "照片"+photoIndex+"提交成功";
                                        }
                                        else
                                        {
                                            SystemTray.ProgressIndicator.Text = "照片" + photoIndex + "提交失败，传输下一张照片";
                                        }
                                        photoIndex++;
                                    }
                                }
                            }
                        }
                        SystemTray.ProgressIndicator.Text = "数据提交完成，返回列表";
                        SetProgressIndicator(false);
                        NavigationService.GoBack();
                    }
                    else
                    {
                        MessageBox.Show("更新本地失败");
                        SetProgressIndicator(false);
                    }
                }
                else
                {
                    MessageBox.Show("提交失败");
                    SetProgressIndicator(false);
                }
            }
        }

        private void btnShoot_Click(object sender, EventArgs e)
        {
            if (tabList.SelectedIndex < 1)
            {
                MessageBox.Show("请到病虫草害详情页面拍照");
            }
            else
            {
                var currentItem = (PivotItem)tabList.SelectedItem;
                var content = currentItem.Content;
                if (content is ITabInfoCanBeCaptured)
                {
                    var item = (ITabInfoCanBeCaptured)content;
                    _photoListIndex = item.PhtotoListIndex;
                    _photoType = item.TypeValue;
                }
                NavigationService.Navigate(new Uri("/Pages/Camera/CameraPage.xaml", UriKind.Relative));
            }
        }


        private static void SetProgressIndicator(bool isVisible)
        {
            SystemTray.ProgressIndicator.IsIndeterminate = isVisible;
            SystemTray.ProgressIndicator.IsVisible = isVisible;
        }

        /// <summary>
        /// 判断当前采集结果是否包含图片
        /// </summary>
        /// <returns></returns>
        private bool hasImages()
        {
            bool hasImage = false;
            foreach (int key in _imageDataDic.Keys)
            {
                var images = _imageDataDic[key].ImageList;
                if (images.Count() > 0)
                {
                    hasImage = true;
                }
            }
            return hasImage;
        }

    }
}