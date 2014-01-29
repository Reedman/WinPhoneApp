using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Net.NetworkInformation;

namespace NyAppWP.Controls
{
    public partial class NotificationBar : UserControl
    {

        public NotificationBar()
        {
            InitializeComponent();
        }

        public NotificationBar(bool isIndeterminate,string msgTxt)
        {
            InitializeComponent();
            progressBar.IsIndeterminate = isIndeterminate;
            txtMessage.Text = msgTxt;
        }

        

    }
}
