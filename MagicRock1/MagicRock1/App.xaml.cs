using System;
using System.Diagnostics;
using System.Resources;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MagicRock1.Resources;
using MagicRock1.ViewModels;
using System.IO.IsolatedStorage;
using System.IO;

namespace MagicRock1
{
    public partial class App : Application
    {
        /*private static MainViewModel viewModel = null;

        /// <summary>
        /// A static ViewModel used by the views to bind against.
        /// </summary>
        /// <returns>The MainViewModel object.</returns>
        public static MainViewModel ViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                if (viewModel == null)
                    viewModel = new MainViewModel();

                return viewModel;
            }
        }

        private static ProgIndViewModel viewModel;

        public static ProgIndViewModel ViewModel
        {
            get
            {
                return viewModel ?? (viewModel = new ProgIndViewModel());
            }
        }*/

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public static PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions.
            UnhandledException += Application_UnhandledException;

            // Standard XAML initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Language display initialization
            InitializeLanguage();

            // Show graphics profiling information while debugging.
            if (Debugger.IsAttached)
            {
                // Display the current frame rate counters
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode,
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Prevent the screen from turning off while under the debugger by disabling
                // the application's idle detection.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }
        }

        // Code to execute when a contract activation such as a file open or save picker returns 
        // with the picked file or other return values
        private void Application_ContractActivated(object sender, Windows.ApplicationModel.Activation.IActivatedEventArgs e)
        {
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            // Set up Grain data for 'Malts' pivot view
            try
            {
                IsolatedStorageFile appIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();

                // No Malt data directory/file (app is fresh install), so create them
                if (!appIsolatedStorage.DirectoryExists("MaltData"))
                {
                    //startProgressIndicator("Manipulating Modal Memory…");

                    appIsolatedStorage.CreateDirectory("MaltData");

                    IsolatedStorageFileStream maltWriteStream = appIsolatedStorage.CreateFile("MaltData\\malts.txt");

                    // Create and populate 'malts.txt' file inside 'MaltData', separate Name & LE with tilde, malts with '\r\n'???
                    using (StreamWriter maltDataWriter = new StreamWriter(maltWriteStream))
                    {
                        //maltDataWriter.WriteLine("Sugar~340.0");
                        maltDataWriter.WriteLine("Low Colour Maris Otter~291.0");
                        maltDataWriter.WriteLine("Golden Promise~295.0");
                        maltDataWriter.WriteLine("Wheat malt~291.0");
                        maltDataWriter.WriteLine("Vienna~289.0");
                        maltDataWriter.WriteLine("Munich~285.0");
                        maltDataWriter.WriteLine("Pilsner~298.0");
                        maltDataWriter.WriteLine("Acidulated malt~0.0");
                        maltDataWriter.WriteLine("Rye malt~260.0");
                        maltDataWriter.WriteLine("Smoked malt~295.0");
                        maltDataWriter.WriteLine("Oat malt~219.0");
                        maltDataWriter.WriteLine("Carapils~267.4");
                        maltDataWriter.WriteLine("Carared~268.0");
                        maltDataWriter.WriteLine("Caramalt~268.4");
                        maltDataWriter.WriteLine("Melanoiden malt~287.0");
                        maltDataWriter.WriteLine("Crystal~260.8");
                        maltDataWriter.WriteLine("Caramunich II~265.0");
                        maltDataWriter.WriteLine("Dark Crystal~270.8");
                        maltDataWriter.WriteLine("Cara Aroma~266.0");
                        maltDataWriter.WriteLine("Amber~266.1");
                        maltDataWriter.WriteLine("Special B~266.0");
                        maltDataWriter.WriteLine("Brown~270.0");
                        maltDataWriter.WriteLine("Chocolate~267.2");
                        maltDataWriter.WriteLine("Roast Barley~264.0");
                        maltDataWriter.WriteLine("Black~265.7");
                        maltDataWriter.WriteLine("Carafa Special III~250.0");

                        maltDataWriter.Close();
                    }

                    //MessageBox.Show("Malts created and saved to storage");
                }
                // Malt data directory exists (app has been opened before), so read data from file
                else
                {
                    //MessageBox.Show("Malts already in storage, go to Brew page to load them for lists");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Couldn't set up Malt data", "Error", MessageBoxButton.OK);
            }
            
            //stopProgressIndicator();
        }

        //private void startProgressIndicator(string progressText)
        //{
        //    ProgressIndicator currentProgressIndicator = new ProgressIndicator()
        //    {
        //        IsVisible = true,
        //        IsIndeterminate = true,
        //        Text = progressText
        //    };
        //    SystemTray.SetProgressIndicator(this, currentProgressIndicator);
        //}

        //private void stopProgressIndicator()
        //{
        //    ProgressIndicator currentProgressIndicator = SystemTray.GetProgressIndicator(this);
        //    if (currentProgressIndicator != null)
        //    {
        //        currentProgressIndicator.IsVisible = false;
        //    }
        //}

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            // Ensure that application state is restored appropriately
            /*if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }*/
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            // Ensure that required application state is persisted here.
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Handle reset requests for clearing the backstack
            RootFrame.Navigated += CheckForResetNavigation;

            // Handle contract activation such as a file open or save picker
            PhoneApplicationService.Current.ContractActivated += Application_ContractActivated;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        private void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            // If the app has received a 'reset' navigation, then we need to check
            // on the next navigation to see if the page stack should be reset
            if (e.NavigationMode == NavigationMode.Reset)
                RootFrame.Navigated += ClearBackStackAfterReset;
        }

        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            // Unregister the event so it doesn't get called again
            RootFrame.Navigated -= ClearBackStackAfterReset;

            // Only clear the stack for 'new' (forward) and 'refresh' navigations
            if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
                return;

            // For UI consistency, clear the entire page stack
            while (RootFrame.RemoveBackEntry() != null)
            {
                ; // do nothing
            }
        }

        #endregion

        // Initialize the app's font and flow direction as defined in its localized resource strings.
        //
        // To ensure that the font of your application is aligned with its supported languages and that the
        // FlowDirection for each of those languages follows its traditional direction, ResourceLanguage
        // and ResourceFlowDirection should be initialized in each resx file to match these values with that
        // file's culture. For example:
        //
        // AppResources.es-ES.resx
        //    ResourceLanguage's value should be "es-ES"
        //    ResourceFlowDirection's value should be "LeftToRight"
        //
        // AppResources.ar-SA.resx
        //     ResourceLanguage's value should be "ar-SA"
        //     ResourceFlowDirection's value should be "RightToLeft"
        //
        // For more info on localizing Windows Phone apps see http://go.microsoft.com/fwlink/?LinkId=262072.
        //
        private void InitializeLanguage()
        {
            try
            {
                // Set the font to match the display language defined by the
                // ResourceLanguage resource string for each supported language.
                //
                // Fall back to the font of the neutral language if the Display
                // language of the phone is not supported.
                //
                // If a compiler error is hit then ResourceLanguage is missing from
                // the resource file.
                RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);

                // Set the FlowDirection of all elements under the root frame based
                // on the ResourceFlowDirection resource string for each
                // supported language.
                //
                // If a compiler error is hit then ResourceFlowDirection is missing from
                // the resource file.
                FlowDirection flow = (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
                RootFrame.FlowDirection = flow;
            }
            catch
            {
                // If an exception is caught here it is most likely due to either
                // ResourceLangauge not being correctly set to a supported language
                // code or ResourceFlowDirection is set to a value other than LeftToRight
                // or RightToLeft.

                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                throw;
            }
        }
    }
}