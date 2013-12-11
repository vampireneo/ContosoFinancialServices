using ContosoFinancialServices.DataModel;
using ContosoFinancialServices.DataSource;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace ContosoFinancialServices
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            RequestedTheme = ApplicationTheme.Dark;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null || !string.IsNullOrEmpty(e.Arguments))
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (string.IsNullOrEmpty(e.Arguments))
                {
                    if (!rootFrame.Navigate(typeof(LandingPage), e.Arguments))
                    {
                        throw new Exception("Failed to create initial page");
                    }
                }
                else
                {
                    if (!rootFrame.Navigate(typeof(ProductDetailPage), e.Arguments))
                    {
                        throw new Exception("Failed to create initial page");
                    }
                }
            }
            // Ensure the current window is active
            Window.Current.Activate();

            CreateLiveTiles();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        #region Feature - Search
        /// <summary>
        /// Invoked when the application is activated to display search results.
        /// </summary>
        /// <param name="args">Details about the activation request.</param>
        protected override void OnSearchActivated(SearchActivatedEventArgs args)
        {
            //Get the frame i.e. the content of the current window
            Frame frame = Window.Current.Content as Frame;

            //Navigate to the Search Results Page
            frame.Navigate(typeof(SearchResultsPage), args.QueryText);
        }
        #endregion

        #region Feature - Live Tile

        /// <summary>
        /// This method creates a new notification for each product by iterating through a list of random products and
        /// passes the notification to the TileNotificationManager.
        /// It then enables notification for the application.
        /// </summary>
        private async void CreateLiveTiles()
        {
            var randomProducts = await ProductDataSource.GetRandomProductsAsync(3);
            foreach (var product in randomProducts)
            {
                // Create a tile notification.  
                TileNotification tileNotification = new TileNotification(CreateWideTile(product));
                TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
            }
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
        }

        /// <summary>
        /// This method takes a Product as its input and generates an XML Document which contains the actual values to
        /// be displayed in the live tile.
        /// </summary>
        /// <param name="product">An object of Product class for getting the actual content to be shown</param>
        /// <returns>An XML document which is used for generating the live tile content</returns>

        private XmlDocument CreateWideTile(Product product)
        {
            // Create a live update for a wide tile
            XmlDocument wideTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150SmallImageAndText04);

            // Assign text
            XmlNodeList wideTileTextAttributes = wideTileXml.GetElementsByTagName("text");
            wideTileTextAttributes[0].AppendChild(wideTileXml.CreateTextNode(product.ProductCategory));
            wideTileTextAttributes[1].AppendChild(wideTileXml.CreateTextNode(product.ProductName));

            // Assign Image
            XmlNodeList wideTileImageAttributes = wideTileXml.GetElementsByTagName("image");
            ((XmlElement)wideTileImageAttributes[0]).SetAttribute("src", product.ProductImageLarge);
            ((XmlElement)wideTileImageAttributes[0]).SetAttribute("alt", "Contoso FinServ Product Image");

            return wideTileXml;
        }

        #endregion
    }
}
