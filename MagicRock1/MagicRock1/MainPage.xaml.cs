using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO;
using System.IO.IsolatedStorage;

namespace MagicRock1
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            SetUpMaltData();
        }

        public void SetUpMaltData()
        {            
            try
            {
                IsolatedStorageFile appIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();

                // No Malt data directory/file (app is fresh install), so create them
                if (!appIsolatedStorage.DirectoryExists("MaltData"))
                {
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

                    MessageBox.Show("Malts created and saved to storage");
                }
                // Malt data directory exists (app has been opened before), so read data from file
                else
                {
                    MessageBox.Show("Malts already in storage, go to Brew page to load them for lists");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Couldn't set up Malt data", "Error", MessageBoxButton.OK);
            }
        }

        private void CreateBrewBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Brew.xaml", UriKind.Relative));
        }

        private void PreviousBrewBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("> TBC");
        }

        private void ToolsBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("> TBC");
        }

        private void AppBarHelpBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("> Create a new brew, or select a previous brew to alter or use as a starting point\n> Tools contains extra assistive features");
        }
    }
}