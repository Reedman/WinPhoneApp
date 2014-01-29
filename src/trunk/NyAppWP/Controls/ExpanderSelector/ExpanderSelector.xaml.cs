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
using System.Windows.Media;
using NyAppWP.Controls.ExpanderSelector;

namespace NyAppWP.Controls.ExpanderSelector
{

    public partial class ExpanderSelector : UserControl
    {

        private ExpanderSelectorBase _expanderSelectorbase = null;
        private ExpanderSelectorDataContext _expanderSelectorDataContext = null;

        public static DependencyProperty DefaultValueProperty = DependencyProperty.Register("DefaultValue", typeof(String), typeof(ExpanderSelector), new PropertyMetadata("请选择...", DefaultValueChanged));
        public static DependencyProperty DataConrtextProperty = DependencyProperty.Register("DataContext", typeof(ExpanderSelectorDataContext), typeof(ExpanderSelector), new PropertyMetadata(null, DataSourceChanged));

        public Object Seleted = null;

        public String DefaultValue
        {
            get { return (String)GetValue(DefaultValueProperty); }
            set
            {
                SetValue(DefaultValueProperty, value);
            }
        }

        public new ExpanderSelectorDataContext DataContext
        {
            get { return (ExpanderSelectorDataContext)GetValue(DataConrtextProperty); }
            set
            {
                SetValue(DataConrtextProperty, value);
            }
        }

        private static void DefaultValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var es = d as ExpanderSelector;
            if (null == es)
                return;

            if (null != e.NewValue)
            {
                es.TxtSelectedValue.Text = (String)e.NewValue;
            }
        }

        private static void DataSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var es = d as ExpanderSelector;
            if (es == null)
            {
                return;
            }

            if (e.NewValue != null)
            {
                es.DataContext = (ExpanderSelectorDataContext)e.NewValue;
                if(es!=null)
                {
                    es.InitializeTree();
                }
            }

        }

        public ExpanderSelector()
        {
            InitializeComponent();
        }

        public void InitializeTree()
        {

            _expanderSelectorDataContext = DataContext;
            _expanderSelectorbase = new ExpanderSelectorBase(_expanderSelectorDataContext);
            _expanderSelectorbase.ValueSelected += (s, e) =>
            {
                var ev = e as ExpanderSelectedEventArgs;

                if (null != ev && null != ev.SelectedObject)
                {
                    Seleted = ev.SelectedObject;

                    this.TxtSelectedValue.Text = ((ExpanderSelectorDataSource)Seleted).Name;

                }
            };
        }


        private void LayoutRoot_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //MethodToPageNavigateToPage(new Uri("/Controls/ExpanderSelector/ExpanderSelectorPage.xaml",UriKind.Relative));
            _expanderSelectorbase.OpenPickerPage();
        }


    }
}
