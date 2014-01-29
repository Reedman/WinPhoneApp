using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Threading.Tasks;
using Microsoft.Phone.Controls;
using System.Windows;

namespace NyAppWP.Controls.ExpanderSelector
{
    public class ExpanderSelectedEventArgs : EventArgs
    {
        public object SelectedObject { get; set; }

        public ExpanderSelectedEventArgs(Object o) { SelectedObject = o; }
    }

    class ExpanderSelectorBase : Control
    {
        private PhoneApplicationFrame _frame;
        private object _frameContentWhenOpened;
        private NavigationInTransition _savedNavigationInTransition;
        private NavigationOutTransition _savedNavigationOutTransition;

        private ExpanderSelectorDataContext _expanderSellectorDataContext = null;
        private Uri _pickerPageUri = new Uri("/Controls/ExpanderSelector/ExpanderSelectorPage.xaml", UriKind.Relative);
        private ExpanderSelectorPage _expanderSelectorPage = null;

        public event EventHandler ValueSelected;

        //public Object SelectItem;

        public Uri PickerPageUri
        {
            get { return _pickerPageUri; }
            set
            {
                if (_pickerPageUri != value)
                {
                    _pickerPageUri = value;
                }
            }
        }

        public ExpanderSelectorBase(ExpanderSelectorDataContext dataContext)
        {

            if (null != dataContext)
                _expanderSellectorDataContext = dataContext;

            //InitRootView();
        }

        public void OpenPickerPage()
        {
            if (null == PickerPageUri)
            {
                throw new ArgumentException("PickerPageUri property must not be null.");
            }

            if (null == _frame)
            {
                _frame = Application.Current.RootVisual as PhoneApplicationFrame;
                if (null != _frame)
                {
                    _frameContentWhenOpened = _frame.Content;

                    // Save and clear host page transitions for the upcoming "popup" navigation
                    UIElement frameContentWhenOpenedAsUIElement = _frameContentWhenOpened as UIElement;
                    if (null != frameContentWhenOpenedAsUIElement)
                    {
                        _savedNavigationInTransition = TransitionService.GetNavigationInTransition(frameContentWhenOpenedAsUIElement);
                        TransitionService.SetNavigationInTransition(frameContentWhenOpenedAsUIElement, null);
                        _savedNavigationOutTransition = TransitionService.GetNavigationOutTransition(frameContentWhenOpenedAsUIElement);
                        TransitionService.SetNavigationOutTransition(frameContentWhenOpenedAsUIElement, null);
                    }

                    _frame.Navigated += OnFrameNavigated;
                    _frame.NavigationStopped += OnFrameNavigationStoppedOrFailed;
                    _frame.NavigationFailed += OnFrameNavigationStoppedOrFailed;

                    _frame.Navigate(PickerPageUri);

                }
            }
        }

        private void ClosePickerPage()
        {
            // Unhook from events
            if (null != _frame)
            {
                _frame.Navigated -= OnFrameNavigated;
                _frame.NavigationStopped -= OnFrameNavigationStoppedOrFailed;
                _frame.NavigationFailed -= OnFrameNavigationStoppedOrFailed;

                // Restore host page transitions for the completed "popup" navigation
                UIElement frameContentWhenOpenedAsUIElement = _frameContentWhenOpened as UIElement;
                if (null != frameContentWhenOpenedAsUIElement)
                {
                    TransitionService.SetNavigationInTransition(frameContentWhenOpenedAsUIElement, _savedNavigationInTransition);
                    _savedNavigationInTransition = null;
                    TransitionService.SetNavigationOutTransition(frameContentWhenOpenedAsUIElement, _savedNavigationOutTransition);
                    _savedNavigationOutTransition = null;
                }

                _frame = null;
                _frameContentWhenOpened = null;
            }

            if (null != _expanderSelectorPage)
            {
                //this.SelectItem = _expanderSelectorPage.SelectItem == null ? _expanderSelectorPage.SelectItem : null;
                ValueSelected(this, new ExpanderSelectedEventArgs(_expanderSellectorDataContext.SelectedItem));
                _expanderSelectorPage = null;
            }

        }

        private void OnFrameNavigated(object sender, NavigationEventArgs e)
        {
            if (e.Content == _frameContentWhenOpened)
            {
                // Navigation to original page; close the picker page
                ClosePickerPage();
            }
            else if (null == _expanderSelectorPage)
            {
                // Navigation to a new page; capture it and push the value in
                var selectorPage  = e.Content as ExpanderSelectorPage;
                
                if (null != selectorPage)
                {
                    _expanderSelectorPage = selectorPage;

                    _expanderSelectorPage.DataContext = _expanderSellectorDataContext;
                    _expanderSelectorPage.InitTreeView();
                    //_expanderSelectorPage.Value = Value.GetValueOrDefault(DateTime.Now);
                    
                    //pickerPage.SetFlowDirection(this.FlowDirection);
                }
            }

        }

        private void OnFrameNavigationStoppedOrFailed(object sender, EventArgs e)
        {
            // Abort
            ClosePickerPage();

        }


    }
}
