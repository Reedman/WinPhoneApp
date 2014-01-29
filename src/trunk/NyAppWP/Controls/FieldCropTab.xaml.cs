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
using NyAppWP.Controls.ExpanderSelector;

namespace NyAppWP.Controls
{
    public partial class FieldCropTab : UserControl, ITabInfoCanBeSaved
    {

        private List<string> _cropNameList;
        private ObservableCollection<Crop> _cropData;
        private bool _isCreateView;
        private FieldCrop _cropInfo;


        public FieldCropTab()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化农田作物控件
        /// </summary>
        /// <param name="cropData">绑定于种植作物的控件的数据源</param>
        /// <param name="cropInfo">为空时，表示是添加农田信息</param>
        public FieldCropTab(ObservableCollection<Crop> cropData, FieldCrop cropInfo, bool isCreateView = true)
        {
            InitializeComponent();
            _isCreateView = isCreateView;
            _cropInfo = cropInfo;
            esCrop.DataContext = new ExpanderSelectorDataContext(cropData.ToList<object>());
            _cropData = cropData;
            _cropNameList = cropData.Select(c => c.CropName).ToList<string>();
            if (!isCreateView)
            {
                esCrop.DefaultValue = _cropInfo.PlantingCropName;
                dpCreatedDate.Value = _cropInfo.PlantingDate;
                dpHarvestDate.Value = _cropInfo.HarvestDate;
                lpPlantCategory.SelectedIndex = _cropInfo.PlantingMethod.HasValue ? _cropInfo.PlantingMethod.Value : 0;
                txtFieldArea.Text = _cropInfo.PlantingArea.HasValue ? _cropInfo.PlantingArea.Value.ToString() : "0";
            }
        }

        private void dpCreatedDate_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            if (!_isCreateView)
            {
                _cropInfo.PlantingDate = dpCreatedDate.Value.Value;
                dpCreatedDate.Value = _cropInfo.PlantingDate;
            }
        }

        private void dpHarvestDate_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            if (!_isCreateView)
            {
                _cropInfo.HarvestDate = dpHarvestDate.Value;
                dpHarvestDate.Value = _cropInfo.HarvestDate;
            }
        }

        private void cbIsCrops_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            if (checkBox.IsChecked.Value)
            {
                var cropList = _cropData.Where(c => c.IsCashCrop == 1).ToList<object>();
                esCrop.DataContext = new ExpanderSelectorDataContext(cropList);
            }
            else
            {
                esCrop.DataContext = new ExpanderSelectorDataContext(_cropData.ToList<object>());
            }
        }


        public bool IsInputValueVailed()
        {
            var selectedCropItem = (ExpanderSelectorDataSource)(esCrop.Seleted);
            var selectedCrop = (selectedCropItem != null) ? _cropData.ToList<Crop>().FirstOrDefault(c => c.CropName == selectedCropItem.Name) : _cropData.ToList<Crop>().FirstOrDefault(c => c.CropName == esCrop.DefaultValue);
            int isCrops = (cbIsCrops.IsChecked.Value) ? 1 : 0;
            dpHarvestDate.Value = dpHarvestDate.Value.HasValue ? dpHarvestDate.Value.Value : DateTime.Now;
            int plantMethodId = lpPlantCategory.SelectedIndex > 0 ? ((ListPickerItem)lpPlantCategory.SelectedItem).TabIndex : 0;
            DateTime createdDate = dpCreatedDate.Value.Value;
            DateTime harvestDate = dpHarvestDate.Value.Value;
            if (dpCreatedDate.Value == null || selectedCrop == null || plantMethodId <= 0)
            {
                MessageBox.Show("农田作物信息必须完整");
                return false;
            }
            else
            {
                _cropInfo.HarvestDate = harvestDate;
                _cropInfo.IsCashCrop = isCrops;
                _cropInfo.PlantingCropId = selectedCrop.CropID;
                _cropInfo.PlantingDate = createdDate;
                _cropInfo.PlantingMethod = plantMethodId;
                _cropInfo.PlantingCropName = selectedCrop.CropName;
                _cropInfo.PlantingArea = int.Parse(txtFieldArea.Text.ToString());
                return true;
            }
        }

    }
}
