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
using System.Windows.Media;

namespace NyAppWP.Controls.ExpanderSelector
{
    public partial class ExpanderSelectorPage : PhoneApplicationPage
    {

        private ExpanderSelectorDataContext _ds = null;

        private readonly static int indentChildExpander = 20;
        private readonly static int indentSeeMoreIcon = 400;

        public Object SelectItem = null;

        public new ExpanderSelectorDataContext DataContext
        {
            set { if (value != _ds) _ds = value; }
            get { return _ds; }
        }


        public ExpanderSelectorPage()
        {
            InitializeComponent();

            //InitRootView();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }


        public void InitTreeView()
        {
            if (null == _ds)
                throw new ArgumentNullException("DataSource", "数据源不能为空");

            List<ExpanderSelectorDataSource> list = _ds.GetRoots();

            foreach (var item in list)
            {
                if(_ds.IsLeaf(item.ID))
                {
                    var leaf = new Border
                    {
                        Margin = new Thickness(0, 10, 0, 0),
                        Style = Application.Current.Resources["ExpanderLeafComp"] as Style,
                        Child = new TextBlock()
                        {
                            Text = item.Name,
                            Style = Application.Current.Resources["ExpanderViewTextBlock"] as Style
                        },
                    };

                    leaf.Tag = item;
                    leaf.Tap += leaf_Tap;
                    EVStack.Children.Add(leaf);
                }
                else
                {
                    ExpanderView view = new ExpanderView
                    {
                        Style = Application.Current.Resources["ExpanderViewStyleForSelector"] as Style,
                        Margin = new Thickness(0, 10, 0, 0),

                        Expander = new Grid
                        {
                            Width = Application.Current.Host.Content.ActualWidth,
                            Background = new SolidColorBrush(Color.FromArgb(255, 186, 122, 53)),
                            Children = { 
                            new TextBlock
                            {
                                Text = item.Name,
                                Style = Application.Current.Resources["ExpanderViewTextBlock"] as Style
                            },
                            new Image
                            {
                                Style = Application.Current.Resources["ExpanerViewSeeMoreIcon"] as Style,
                                Margin = new Thickness(indentSeeMoreIcon, 0, 0, 0),
                            }
                        }
                        }
                    };

                    InitChildViewRecursion(item.ID, ref view, 1);
                    EVStack.Children.Add(view);
                }
            }
        }

        private void InitChildViewRecursion(String Id, ref ExpanderView ev, int level)
        {
            List<ExpanderSelectorDataSource> list = _ds.GetChildNodeById(Id);

            if (null != list && list.Count() != 0)
            {
                foreach (var item in list)
                {
                    //根节点
                    if (_ds.IsLeaf(item.ID))
                    {
                        var leaf = new Border
                        {
                            Margin = new Thickness(indentChildExpander, 10, 0, 0),
                            Style = Application.Current.Resources["ExpanderLeafComp"] as Style,
                            Child = new TextBlock()
                            {
                                Text = item.Name,
                                Style = Application.Current.Resources["ExpanderViewTextBlock"] as Style
                            },
                        };

                        leaf.Tag = item;
                        leaf.Tap += leaf_Tap;
                        ev.Items.Add(leaf);
                    }
                    else //非根节点，继续搜索
                    {
                        ExpanderView view = new ExpanderView
                        {
                            Style = Application.Current.Resources["ExpanderViewStyleForSelector"] as Style,
                            Margin = new Thickness(indentChildExpander, 10, 0, 0),

                            Expander = new Border
                            {
                                Background = new SolidColorBrush(Color.FromArgb(255, 186, 122, 53)),

                                Child = new Grid
                                {
                                    Width = Application.Current.Host.Content.ActualWidth,
                                    Background = new SolidColorBrush(Color.FromArgb(255, 186, 122, 53)),
                                    Children = {
                                        new TextBlock
                                        {
                                            Text = item.Name,
                                            Style = Application.Current.Resources["ExpanderViewTextBlock"] as Style
                                        }, 
                                        new Image
                                        {
                                            Style = Application.Current.Resources["ExpanerViewSeeMoreIcon"] as Style,
                                            Margin = new Thickness(indentSeeMoreIcon - indentChildExpander * level, 0, 0, 0)
                                        } 
                                    }
                                }
                            },

                        };

                        view.Tap += view_Tap;

                        InitChildViewRecursion(item.ID, ref view, level + 1);
                        ev.Items.Add(view);
                    }
                }
            }
        }

        void view_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("11111");
        }

        void leaf_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var s = sender as Border;
            _ds.SelectedItem = s.Tag;
            NavigationService.GoBack();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }



    }
}