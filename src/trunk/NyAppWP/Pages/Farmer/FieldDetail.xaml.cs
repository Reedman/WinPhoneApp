using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NyAppHelper.Data;
using NyAppHelper.Model;
using System.Globalization;
using System.Threading;
using NyAppWP.DataContext;
using NyAppWP.Resources;
using System.Collections.ObjectModel;
using NyAppHelper.Http;
using System.Device.Location;
using System.Xml;
using System.Windows.Markup;
using NyAppWP.Controls.ExpanderSelector;
using NyAppWP.Controls;
using NyAppHelper.Location;

namespace NyAppWP.Pages
{
    public partial class FieldDetail : PhoneApplicationPage
    {
        private int _cropTabIndex;


        private RegionDataContext _regionDataContext;
        private ObservableCollection<Region> _regionData;
        private FieldDataAccessLayer _fieldDataAccessLayer;
        private FieldCropDataAccessLayer _fieldCropDataAccessLayer;
        private CropDataContext _cropDataContext;
        private ObservableCollection<Crop> _cropData;
        private FieldService _service;
        private Field _field;
        private int _fieldId;
        private List<FieldCrop> _fieldCropList;
        private FieldSurveyResult _fieldSuveryResult;

        public FieldDetail()
        {
            InitializeComponent();

            _cropDataContext = new CropDataContext();
            _regionDataContext = new RegionDataContext();
            _service = new FieldService();
            _fieldDataAccessLayer = new FieldDataAccessLayer();
            _fieldCropDataAccessLayer = new FieldCropDataAccessLayer();
            _fieldSuveryResult = new FieldSurveyResult();
            App.RegionalCultureOverride = Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            App.UICultureOverride = Thread.CurrentThread.CurrentUICulture = new CultureInfo("zh-CN");
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if(e.Content is MapPage)
            {
                var map = e.Content as MapPage;
                if(map != null)
                    map.SurveryResult = this._fieldSuveryResult;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ((PivotItem)bodyPiv.Items[0]).Header = "基本信息";

            if (e.NavigationMode == NavigationMode.New)
            {
                _fieldId = int.Parse(NavigationContext.QueryString["fid"].ToString());
                _field = _fieldDataAccessLayer.Get(_fieldId);
                _fieldCropList = _field.Plantings;
                _fieldSuveryResult.FieldPoints = GeoHelper.ParsePointsFromString(_field.Geo, ';', ',');

                int plantingCount = _fieldCropList.Count;
                esRegion.DefaultValue = _field.RegionName;
                
                _regionDataContext.GetAllRegions((regionData) =>
                {
                    _regionData = regionData;
                    esRegion.DataContext = new ExpanderSelectorDataContext(_regionData.ToList<object>());
                    _cropDataContext.GetAllCrops((cropData) =>
                   {
                       _cropData = cropData;
                       for (_cropTabIndex = 0; _cropTabIndex < plantingCount; _cropTabIndex++)
                       {
                           int index = _cropTabIndex + 1;
                           string headerTxt = "作物" + index;
                           var content = new NyAppWP.Controls.FieldCropTab(_cropData, _fieldCropList[_cropTabIndex], false);
                           bodyPiv.Items.Add(new PivotItem()
                           {
                               Header = headerTxt,
                               Content = content,
                           });
                       }
                       txtFieldArea.Text = _field.Area.ToString("f2", CultureInfo.InvariantCulture);
                       txtFieldName.Text = _field.FarmlandName;
                   });
                });
            }
            //PhoneApplicationService.Current.State.Add("fieldSurveyResult", fsRst);
            //if (PhoneApplicationService.Current.State.ContainsKey("fieldSurveyResult"))
            //{
            //    Object obj;
            //    PhoneApplicationService.Current.State.TryGetValue("fieldSurveyResult", out obj);
            //    FieldSurveyResult fr = (FieldSurveyResult)obj;
            //    if (fr.FieldID == _guid.ToString())
            //    {
            //        var ptList = fr.FieldPoints.ToList();
            //        System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //        foreach (GeoCoordinate coor in ptList)
            //        {
            //            sb.Append(coor.Longitude.ToString("#0.000000")).Append(",").Append(coor.Latitude.ToString("#0.000000")).Append(";");
            //        }

            //        this._fieldGPSPoints = sb.ToString();
            //        this._area = fr.FieldArea;
            //    }
            //}
        }



        private void txtFieldArea_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnHome_Click(object sender, EventArgs e)
        {

        }

        private void btnQuery_Click(object sender, EventArgs e)
        {

        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(_cropTabIndex.ToString());
            string fieldName = txtFieldName.Text.Trim();

            for (int i = 1; i < bodyPiv.Items.Count; i++)
            {
                var content = ((PivotItem)bodyPiv.Items[i]).Content;
                if (content is FieldCropTab)
                {
                    var item = (FieldCropTab)content;
                    if (!item.IsInputValueVailed())
                    {
                        return;
                    }
                }
            }

            if (String.IsNullOrEmpty(fieldName) || String.IsNullOrEmpty(txtFieldArea.Text))
            {
                MessageBox.Show("农田信息必须完整");
            }
            else
            {
                if (_fieldSuveryResult.FieldPoints != null)
                {
                var ptList = _fieldSuveryResult.FieldPoints.ToList();
                    if (ptList.Count() > 0)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (GeoCoordinate coor in ptList)
                    {
                        sb.Append(coor.Longitude.ToString("#0.000000")).Append(",").Append(coor.Latitude.ToString("#0.000000")).Append(";");
                    }

                    sb.Remove(sb.Length - 1, 1);

                    this._field.Geo = sb.ToString();
                }
                }

                var selectedRegion = _regionData.FirstOrDefault(r => r.RegionName == esRegion.DefaultValue);
                float area = float.Parse(txtFieldArea.Text);
                _field.FarmlandName = fieldName;
                _field.RegionID = selectedRegion.RegionID;
                _field.RegionName = selectedRegion.RegionName;
                _field.Plantings = _fieldCropList;
                if (await _service.Update(_field))
                {
                    if (_fieldDataAccessLayer.Update(_field) && _fieldCropDataAccessLayer.UpdateAll(new ObservableCollection<FieldCrop>(_field.Plantings)))
                    {
                        MessageBox.Show("修改农田成功");
                        NavigationService.GoBack();
                    }
                    else
                    {
                        MessageBox.Show("修改农田失败");
                    }
                }
                else
                {
                    MessageBox.Show("向服务端修改农田失败");
                }
            }
        }

        private void btnSurveyField_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MapPage.xaml?viewMode=Survey", UriKind.Relative));
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _cropTabIndex++;
            string headerTxt = "作物" + _cropTabIndex;
            var fieldCrop = new FieldCrop();
            var content = new FieldCropTab(_cropData,fieldCrop);
            bodyPiv.Items.Add(new PivotItem()
            {
                Header = headerTxt,
                Content = content,
            });
            _fieldCropList.Add(fieldCrop);
            bodyPiv.SelectedIndex = _cropTabIndex;
        }

    }
}