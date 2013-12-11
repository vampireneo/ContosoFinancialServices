using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ContosoFinancialServices.Common;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ContosoFinancialServices.DataModel;
using ContosoFinancialServices.DataSource;
using Windows.ApplicationModel.DataTransfer;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.StartScreen;

// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232

namespace ContosoFinancialServices
{
    /// <summary>
    /// A page that displays details for a single item within a group while allowing gestures to
    /// flip through other items belonging to the same group.
    /// </summary>
    public sealed partial class ProductDetailPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private Product product;
        private Slider sliderAge;
        private Slider sliderTerm;
        private Slider sliderIncome;
        private Slider sliderSum;

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

        public ProductDetailPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
        }

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
            product = await ProductDataSource.GetProductDetailsAsync(e.NavigationParameter.ToString());
            this.DefaultViewModel["Product"] = product;
            this.DefaultViewModel["ProductName"] = product.ProductName;
            this.DefaultViewModel["ProductCharacteristics"] = product.ProductCharacteristics;
            this.DefaultViewModel["ProductStatistics"] = product.ProductStatistics;
            this.DefaultViewModel["CustomizationParameters"] = product.CustomizationParameters;
            this.DefaultViewModel["EMI"] = "0.00";

            LoadSliders();                       

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

        #region Feature - App Bar

        /// <summary>
        /// Handles the App Bar event - Home. This event is used to take the user back to landing page. 
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to product listing page
            this.Frame.Navigate(typeof(LandingPage));
        }
        
        #endregion

        private void hsCustomizeProduct_Loaded(object sender, RoutedEventArgs e)
        {

        }

        #region Feature - Share
        
        #endregion

        #region Feature - Pin Secondary Tile
        
        #endregion

        #region EMI calculation

        private void LoadSliders()
        {
            this.DefaultViewModel["MinimumValueAge"] = product.CustomizationParameters.Where(item => item.ParameterName.Equals("Age")).FirstOrDefault().MinimumValue;
            this.DefaultViewModel["MaximumValueAge"] = product.CustomizationParameters.Where(item => item.ParameterName.Equals("Age")).FirstOrDefault().MaximumValue;
            this.DefaultViewModel["MinimumValueTerm"] = product.CustomizationParameters.Where(item => item.ParameterName.Equals("Policy Term")).FirstOrDefault().MinimumValue;
            this.DefaultViewModel["MaximumValueTerm"] = product.CustomizationParameters.Where(item => item.ParameterName.Equals("Policy Term")).FirstOrDefault().MaximumValue;
            this.DefaultViewModel["MinimumValueIncome"] = product.CustomizationParameters.Where(item => item.ParameterName.Equals("Gross Annual Income")).FirstOrDefault().MinimumValue;
            this.DefaultViewModel["MaximumValueIncome"] = product.CustomizationParameters.Where(item => item.ParameterName.Equals("Gross Annual Income")).FirstOrDefault().MaximumValue;
            this.DefaultViewModel["MinimumValueSum"] = product.CustomizationParameters.Where(item => item.ParameterName.Equals("Sum Assured")).FirstOrDefault().MinimumValue;
            this.DefaultViewModel["MaximumValueSum"] = product.CustomizationParameters.Where(item => item.ParameterName.Equals("Sum Assured")).FirstOrDefault().MaximumValue;
        }
        private void slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            double rate = 6.8;
            if (sliderAge != null && sliderTerm != null && sliderIncome != null && sliderSum != null)
            {
                if (sliderAge.Value > 40 && sliderAge.Value < 55)
                    rate = 6.6;
                else if (sliderAge.Value >= 55 && sliderAge.Value <= 65)
                    rate = 6;
                double emi = ((sliderSum.Value * 1000 * rate) - ((sliderIncome.Value * 1000) / 12)) / sliderTerm.Value;

                this.DefaultViewModel["EMI"] = (emi / 12).ToString("C");

            }
        }

        private void sliderAge_Loaded(object sender, RoutedEventArgs e)
        {
            sliderAge = sender as Slider;
        }

        private void sliderTerm_Loaded(object sender, RoutedEventArgs e)
        {
            sliderTerm = sender as Slider;
        }

        private void sliderIncome_Loaded(object sender, RoutedEventArgs e)
        {
            sliderIncome = sender as Slider;
        }

        private void sliderSum_Loaded(object sender, RoutedEventArgs e)
        {
            sliderSum = sender as Slider;
        }
        #endregion
    }
}
