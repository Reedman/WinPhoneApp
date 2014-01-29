using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using System.Windows.Controls.Primitives;
using System.Windows.Media;
using NyAppHelper.Data;
using NyAppHelper.Model;

namespace NyAppWP.Controls
{


    public partial class CollectionRequirementPopup : UserControl
    {
        //private Popup popup_container = null;
        public PopupResult popup_result = PopupResult.PRCANCEL;

        public delegate void dismissEventHandler(Object sender,PopUpResultEventArgs e);
        public event dismissEventHandler OnDisMiss;
 
        protected virtual void OnCtlDismiss(PopupResult rst)
        {
            if (OnDisMiss != null)
                OnDisMiss(this,new PopUpResultEventArgs(rst));
        }

        public CollectionRequirementPopup()
        {
            InitializeComponent();
            
            border.Height = Application.Current.Host.Content.ActualHeight;
            border.Width = Application.Current.Host.Content.ActualWidth;
        }

        public CollectionRequirementPopup(int taskId, Boolean mask = true)
        {
            InitializeComponent();
            var pestRequiremenList = new CollectionTaskPestViewDataAccessLayer().GetAll(taskId);
            lbRequirement.ItemsSource = pestRequiremenList.ToList<CollectionTaskPestView>();
            border.Height = Application.Current.Host.Content.ActualHeight;
            border.Width = Application.Current.Host.Content.ActualWidth;

            if (!mask)
            {
                border.Background = new SolidColorBrush(Colors.Transparent);
            }
        }

        private void Confirm_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            CloseMe();
        }

        private void CloseMe()
        {
            if (this.Parent is Panel)
            {
                Panel p = (Panel)this.Parent;
                p.Children.Remove(this);
                OnCtlDismiss(popup_result);
            }
        }


    }
}
