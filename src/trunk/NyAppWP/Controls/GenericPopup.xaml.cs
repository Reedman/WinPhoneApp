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

namespace NyAppWP.Controls
{
    public enum PopupResult {PROK,PRCANCEL};

    public class PopUpResultEventArgs : EventArgs
    {
        public PopupResult result { get; set; }

        public PopUpResultEventArgs(PopupResult rst)
        {
            result = rst;
        }
    }

    public partial class GenericPopup : UserControl
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

        public GenericPopup()
        {
            InitializeComponent();

            border.Height = Application.Current.Host.Content.ActualHeight;
            border.Width = Application.Current.Host.Content.ActualWidth;
        }

        public GenericPopup(String MsgPopup,Boolean mask = true)
        {
            InitializeComponent();

            this.TxtMsg.Text = MsgPopup;

            border.Height = Application.Current.Host.Content.ActualHeight;
            border.Width = Application.Current.Host.Content.ActualWidth;

            if (!mask)
            {
                border.Background = new SolidColorBrush(Colors.Transparent);
            }

        }

        private void Confirm_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            popup_result = PopupResult.PROK;
            CloseMe();
        }

        private void Cancel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            popup_result = PopupResult.PRCANCEL;
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
