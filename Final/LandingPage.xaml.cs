using ContosoFinancialServices.Common;
using ContosoFinancialServices.DataSource;
using ContosoFinancialServices.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;

// The Hub Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=286574

namespace ContosoFinancialServices
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class LandingPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public LandingPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;

            // Register a handler for the Window Size Changed event
            Window.Current.SizeChanged += Window_SizeChanged;            
        }

        #region Feature - Snapped View

        /// <summary>
        /// Represents the event handler for the window size changed event.
        /// This event is raised whenever the window is resized and helps 
        /// handle any changes to visual state of the page. 
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically the current window.
        /// </param>
        /// <param name="e">
        /// Event data that provides the changed size for the Window.
        /// </param>
        void Window_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            // Check the window size and update the Visual State
            UpdateVisualState(e.Size.Width);
        }

        /// <summary>
        /// Check the window width and update the Visual State
        /// </summary>
        /// <param name="width">Width of current window</param>
        void UpdateVisualState (double width)
        {
            if (width < 620)
            {
                VisualStateManager.GoToState(this, "NarrowLayout", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "PrimaryLayout", true);
            }
        }
        #endregion


        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {

            this.defaultViewModel["RecentCustomers"] = await CustomerDataSource.GetRecentCustomerListAsync(4);
            this.defaultViewModel["FeaturedProducts"] = await ProductDataSource.GetFeaturedProductListAsync(4);

            // Check the window size and update the Visual State
            UpdateVisualState(Window.Current.Bounds.Width);
        }

        /// <summary>
        /// This method handles the OnItemClick event of the ItemGridView control.
        /// On clicking on a Job card, the user is navigated to the details page for that job. 
        /// The Job ID is passed as a parameter.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ItemClickEventArgs"/> instance containing the event data.</param>
        private void FeaturedProductItemGridView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(ProductDetailPage), (e.ClickedItem as Product).ProductId);
        }

        // <summary>
        /// This method handles the OnItemClick event of the ItemGridView control.
        /// On clicking on a Job card, the user is navigated to the details page for that job. 
        /// The Job ID is passed as a parameter.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ItemClickEventArgs"/> instance containing the event data.</param>
        private void RecentCustomerItemGridView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(CustomerDetailPage), (e.ClickedItem as Customer).CustomerId);
        }

        private void ViewAllCustomer_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CustomerListingPage));
        }

        private void ViewAllProduct_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ProductListingPage));
        }

        

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void ViewCustomers_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CustomerListingPage));
        }

        private void ViewProducts_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ProductListingPage));
        }
    }
}
