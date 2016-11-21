using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace MasterDetailApp
{
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<ListItem> ItemList { get; set; }
        public MainPage()
        {
            this.InitializeComponent();

            ItemList = new ObservableCollection<ListItem>();

            for (int i = 0; i < 10; i++)
            {
                ItemList.Add(new ListItem() { Title = $"Title {i}", Detail = $"Detail {i}" });
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < 720)
            {
                if (ItemListView.SelectedItem == null)
                {
                    VisualStateManager.GoToState(this, "VisualState_NarrowListView", true);
                }
                else
                {
                    VisualStateManager.GoToState(this, "VisualState_NarrowDetail", true);
                }
            }
            else
            {
                VisualStateManager.GoToState(this, "VisualState_Default", true);
            }
        }

        private void VisualStateGroup_MD_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = VisualStateGroup_MD.CurrentState ==
                VisualState_NarrowDetail ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;

            if (VisualStateGroup_MD.CurrentState == VisualState_NarrowDetail)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
            }
        }

        private void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            VisualStateManager.GoToState(this, "VisualState_NarrowListView", true);
            ItemListView.SelectedItem = null;
        }

        private void ItemListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (VisualStateGroup_MD.CurrentState == VisualState_NarrowListView)
            {
                VisualStateManager.GoToState(this, "VisualState_NarrowDetail", true);
            }
        }
    }

    public class ListItem
    {
        public string Title { get; set; }
        public string Detail { get; set; }
    }
}
