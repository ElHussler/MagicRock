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
            //DataContext = App.ViewModel;
        }

        private void CreateBrewBtn_Click(object sender, RoutedEventArgs e)
        {
            //startProgressIndicator("Reticulating Splines…");

            /*var indicator = new ProgressIndicator {
                indicator.Text = "Some message",
                indicator.IsIndeterminate = true,
                indicator.IsVisible = true
            };*/

            //ProgressIndicator indicator = new ProgressIndicator()
            //{
            //    IsVisible = true,
            //    IsIndeterminate = true,
            //    Text = "Reticulating Splines…"
            //};

            //App.ViewModel.ProgressIndicator = indicator;

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

        /*private void startProgressIndicator(string progressText)
        {
            ProgressIndicator currentProgressIndicator = new ProgressIndicator()
            {
                IsVisible = true,
                IsIndeterminate = true,
                Text = progressText
            };
            SystemTray.SetProgressIndicator(this, currentProgressIndicator);
        }

        private void stopProgressIndicator()
        {
            ProgressIndicator currentProgressIndicator = SystemTray.GetProgressIndicator(this);
            if (currentProgressIndicator != null)
            {
                currentProgressIndicator.IsVisible = false;
            }
        }*/
    }
}