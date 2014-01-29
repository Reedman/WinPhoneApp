using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace NyAppWP.Controls
{
    public partial class CartographicModeSelector : UserControl
    {
        public CartographicModeSelector()
        {
            InitializeComponent();
        }

        private void CartoRoad_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void CartoHybrid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void CartoTerrain_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
