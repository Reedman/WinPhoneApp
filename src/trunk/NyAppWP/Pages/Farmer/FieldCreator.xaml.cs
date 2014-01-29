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
using NyAppWP.Controls;
using System.Xml;
using System.Windows.Markup;
using NyAppWP.Controls.ExpanderSelector;


using System.Device.Location;

namespace NyAppWP.Pages
{
    public partial class FieldCreator : PhoneApplicationPage
    {
        private int _cropTabIndex = 0;
        private Guid _guid;

        private FieldDataAccessLayer _fieldDataAccessLayer;
        private CropDataContext _cropDataContext;
        private RegionDataContext _regionDataContext;
        private ObservableCollection<Crop> _cropData;
        private ObservableCollection<Region> _regionData;
        private FieldService _service;
        private List<FieldCrop> _fieldCropList;
        private FieldSurveyResult _fieldSuveryResult;

        /// <summary>
        /// 需要创建的农田
        /// </summary>
        private Field _field;

        public FieldCreator()
        {
            InitializeComponent();
            ((PivotItem)bodyPiv.Items[0]).Header = "基本信息";
            _guid = new Guid();
            _cropDataContext = new CropDataContext();
            _regionDataContext = new RegionDataContext();
            _fieldSuveryResult = new FieldSurveyResult();
            _regionDataContext.GetAllRegions((data) =>
            {
                _regionData = data;
                esRegion.DataContext = new ExpanderSelectorDataContext(_regionData.ToList<object>());
                _service = new FieldService();
                _fieldDataAccessLayer = new FieldDataAccessLayer();
                _field = new Field();
                _fieldCropList = new List<FieldCrop>();
            });
            App.RegionalCultureOverride = Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            App.UICultureOverride = Thread.CurrentThread.CurrentUICulture = new CultureInfo("zh-CN");
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (e.Content is MapPage)
            {
                var map = e.Content as MapPage;
                if (map != null)
                    map.SurveryResult = this._fieldSuveryResult;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
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

            string fieldName = txtFieldName.Text.Trim();
            var regionInfo = (ExpanderSelectorDataSource)esRegion.Seleted;

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

            if (String.IsNullOrEmpty(fieldName) || String.IsNullOrEmpty(txtFieldArea.Text) || _fieldCropList.Count <= 0 || regionInfo == null)
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


                float area = float.Parse(txtFieldArea.Text);
                _field.Area = area;
                _field.FarmlandName = fieldName;
                //_field.Geo = _fieldGPSPoints;
                _field.CreatedDate = DateTime.Now;
                _field.RegionName = regionInfo.Name;
                _field.RegionID = int.Parse(regionInfo.ID);
                _field.Plantings = _fieldCropList;
                _field = await _service.Add(_field);
                if (_field.FarmlandID > 0)
                {
                    if (_fieldDataAccessLayer.Add(_field))
                    {
                        MessageBox.Show("添加农田成功");
                        NavigationService.GoBack();
                    }
                    else
                    {
                        MessageBox.Show("添加农田失败");
                    }
                }
                else
                {
                    MessageBox.Show("向服务端添加农田失败");
                }
            }

        }

        private void btnSurveyField_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MapPage.xaml?viewMode=Survey&fieldID=" + _guid, UriKind.Relative));
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _cropDataContext.GetAllCrops((data) =>
            {
                _cropData = data;
                _cropTabIndex++;
                string headerTxt = "作物" + _cropTabIndex;
                var fieldCrop = new FieldCrop();
                var content = new FieldCropTab(_cropData, fieldCrop);
                bodyPiv.Items.Add(new PivotItem
                {
                    Header = headerTxt,
                    Content = content,
                });
                _fieldCropList.Add(fieldCrop);
                bodyPiv.SelectedIndex = _cropTabIndex;
            });
        }


    }
}