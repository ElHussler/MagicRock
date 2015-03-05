using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MagicRock1.Views
{
    public partial class Brew : PhoneApplicationPage
    {
        public Brew()
        {
            InitializeComponent();
            PopulateGrainArrays();

            double targetOG = 0;
            double startBoil = 0;
            double totalLiquorBack = 0;
            double endOfBoilGravity = 0;
        }

        public void PopulateGrainArrays()
        {
            string[] grainNames = new string[26];
            double[] grainLabExtract = new double[26];
            grainNames[0] = "Low Colour Maris Otter";
            grainLabExtract[0] = 291.0;

            SugarLP.ItemsSource = grainNames;
        }

        public void AddLabExtract()
        {

        }

        public void CalculateGrist()
        {

        }

        private void AppBarHelpBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("> TBC");
        }

        private void PotentialGravityLbl_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MessageBox.Show("Potential Gravity = \n(Litre Degrees TOTAL / Start Boil vol (L)) + 1000");
        }

        private void GravityEfficiencyLbl_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MessageBox.Show("Gravity with Efficiency = " + 
                            "\n(Potential Gravity - 1000) * ((Efficiency of Mash = 80?) / 100)" + 
                            "\n + 1000 + (Litre Degrees of SUGAR / Start Boil vol (L))");
        }

        private void GristLbl_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MessageBox.Show("Grist % makeup = " +
                            "\n(Malt Bill of GRAIN / Malt Bill TOTAL) * 100");
        }
    }
}