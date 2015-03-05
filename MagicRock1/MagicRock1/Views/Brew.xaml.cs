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
        string[] grainNames = new string[25];
        double[] grainLabExtract = new double[25];

        string sugarName;
        double sugarLabExtract;
        double sugarLitreDegrees;

        double targetOG;
        double startBoil;
        double totalLiquorBack;
        double endOfBoilGravity;

        double effOfMash;

        double tempMaltGrist;
        double tempMaltBill;
        double maltBillTotal;
        double litreDegreesTotal;

        double potentialGravity;
        double gravityWithEfficiency;
        double[] gravities = new double[2];

        // Custom calculation methods
        public Brew()
        {
            InitializeComponent();

            InstatiateMaltData();
            PopulateListPickers();
        }

        // Populate arrays with Grain 'Name' and 'Lab Extract' values (including Sugar)
        public void InstatiateMaltData()
        {
            sugarName = "Sugar";
            sugarLabExtract = 340.0;

            grainNames[0] = "Low Colour Maris Otter";
            grainLabExtract[0] = 291.0;
            grainNames[1] = "Golden Promise";
            grainLabExtract[1] = 295.0;
            grainNames[2] = "Wheat malt";
            grainLabExtract[2] = 291.0;
            grainNames[3] = "Vienna";
            grainLabExtract[3] = 289.0;
            grainNames[4] = "Munich";
            grainLabExtract[4] = 285.0;
            grainNames[5] = "Pilsner";
            grainLabExtract[5] = 298.0;
            grainNames[6] = "Acidulated malt";
            grainLabExtract[6] = 0.0;
            grainNames[7] = "Rye malt";
            grainLabExtract[7] = 260.0;
            grainNames[8] = "Smoked malt";
            grainLabExtract[8] = 295.0;
            grainNames[9] = "Oat Malt";
            grainLabExtract[9] = 219.0;
            grainNames[10] = "Carapils";
            grainLabExtract[10] = 267.4;
            grainNames[11] = "Carared";
            grainLabExtract[11] = 268.0;
            grainNames[12] = "Caramalt";
            grainLabExtract[12] = 268.4;
            grainNames[13] = "Melanoiden malt";
            grainLabExtract[13] = 287.0;
            grainNames[14] = "Crystal";
            grainLabExtract[14] = 260.8;
            grainNames[15] = "Caramunich II";
            grainLabExtract[15] = 265.0;
            grainNames[16] = "Dark Crystal";
            grainLabExtract[16] = 270.8;
            grainNames[17] = "Cara Aroma";
            grainLabExtract[17] = 266.0;
            grainNames[18] = "Amber";
            grainLabExtract[18] = 266.1;
            grainNames[19] = "Special B";
            grainLabExtract[19] = 266.0;
            grainNames[20] = "Brown";
            grainLabExtract[20] = 270.0;
            grainNames[21] = "Chocolate";
            grainLabExtract[21] = 267.2;
            grainNames[22] = "Roast Barley";
            grainLabExtract[22] = 264.0;
            grainNames[23] = "Black";
            grainLabExtract[23] = 265.7;
            grainNames[24] = "Carafa Special III";
            grainLabExtract[24] = 250.0;
        }

        public void PopulateListPickers()
        {
            SugarLP.ItemsSource = sugarName;
            SugarLabExTb.Text = sugarLabExtract.ToString();

            MaltOneLP.ItemsSource = grainNames;
            MaltTwoLP.ItemsSource = grainNames;
            MaltThreeLP.ItemsSource = grainNames;
            MaltFourLP.ItemsSource = grainNames;
            MaltFiveLP.ItemsSource = grainNames;
            MaltSixLP.ItemsSource = grainNames;
        }

        public void UserUpdateLabExtract()
        {

        }

        public double CalculateGrist(double tempMaltGrist, double tempMaltBill)
        {
            tempMaltGrist = (tempMaltBill / maltBillTotal) * 100;
            return tempMaltGrist;
        }

        public void CalculateLitreDegrees()
        {
        }

        public double[] CalculateGravities()
        {
            litreDegreesTotal = 0;

            potentialGravity = (litreDegreesTotal / startBoil) + 1000;

            gravityWithEfficiency = (potentialGravity - 1000) * (effOfMash / 100) + 1000 + (sugarLitreDegrees / startBoil);

            gravities[0] = potentialGravity;
            gravities[1] = gravityWithEfficiency;

            return gravities;
        }

        // Events for gathering required input data
        private void EffOfMashTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                effOfMash = Convert.ToDouble(EffOfMashTb.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Efficiency of Mash must be a numeric value");
            }
        }

        private void StartBoilTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                startBoil = Convert.ToDouble(StartBoilTb.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Start boil must be a numeric value");
            }
        }

        private void TargetOgTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                targetOG = Convert.ToDouble(TargetOgTb.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Target OG must be a numeric value");
            }
        }

        private void SugarLP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SugarLP.ItemsSource = sugarName;
            SugarLabExTb.Text = sugarLabExtract.ToString();
        }

        private void MaltOneLP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MaltTwoLP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MaltThreeLP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MaltFourLP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MaltFiveLP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MaltSixLP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        // Help messages
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