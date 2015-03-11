using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MagicRock1.ViewModels;
using MagicRock1.Models;
using MagicRock1.Views;
using System.IO.IsolatedStorage;
using System.IO;

namespace MagicRock1.Views
{
    public partial class Brew : PhoneApplicationPage
    {
        ///// Page-scope variable declarations
        //

        //string[] grainNames = new string[25];         NOT
        //double[] grainLabExtract = new double[25];    IN
        //private GrainsViewModel gvm;                  USE

        // Grain & sugar data
        string[] grainNames = new string[25];
        double[] grainLabExtracts = new double[25];
        double sugarLabExtract = 340.0;

        // User-input (Malts top)
        double targetOG = 0;
        double startBoil = 0;
        double mashEfficiency = 0;

        // User-input (Malts rows)
        double maltOneBill = 0;
        double maltTwoBill = 0;
        double maltThreeBill = 0;
        double maltFourBill = 0;
        double maltFiveBill = 0;
        double maltSixBill = 0;

        // Calculated based on user-input values
        double maltOneLitreDegrees = 0;
        double maltTwoLitreDegrees = 0;
        double maltThreeLitreDegrees = 0;
        double maltFourLitreDegrees = 0;
        double maltFiveLitreDegrees = 0;
        double maltSixLitreDegrees = 0;

        // Vary depending on number and quantity of malts used
        double totalMaltBill = 0;
        double totalLitreDegrees = 0;

        // Dynamic variables (Malts bottom)
        double potentialGravity;
        double gravityWithEfficiency;
        double[] gravities = new double[2];

        // Calculated on 'Malts', passed for use on 'Mash'
        double totalLiquorBackVol = 0;
        double endOfBoilGravity = 0;

        private bool IgnoreSelectionChanged = true;

        ///// Setup
        //
        public Brew()
        {
            InitializeComponent();
            SetUpMaltData();

            //gvm = new GrainsViewModel();
            //gvm.GetGrains();

            //GrainViewOne.DataContext = gvm.Grains;
            //GrainViewSugar.DataContext = gvm.Grains;
            //GrainViewTwo.DataContext = gvm.Grains;
            //GrainViewThree.DataContext = gvm.Grains;
            //GrainViewFour.DataContext = gvm.Grains;
            //GrainViewFive.DataContext = gvm.Grains;
            //GrainViewSix.DataContext = gvm.Grains;

            //InstatiateMaltData();
            //SetUpListPickers();
        }

        public void SetUpMaltData()
        {
            IsolatedStorageFile appIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream maltReadStream = appIsolatedStorage.OpenFile("MaltData\\malts.txt", FileMode.Open, FileAccess.ReadWrite);

            string[] fileMaltData = new string[25];

            // create new StreamReader to read from malts.txt
            using (StreamReader maltReader = new StreamReader(maltReadStream))
            {
                int maltIndex = 0;
                while (!maltReader.EndOfStream)
                {
                    fileMaltData[maltIndex] = maltReader.ReadLine();
                    maltIndex++;
                }
            }

            string nameLabExtractSplitter = "~";
            int nameLabExIndex = 0;

            foreach (string malt in fileMaltData)
            {
                // Formats txt file into array elements
                string[] tempMalt = malt.Split(new string[] { nameLabExtractSplitter }, StringSplitOptions.None);

                grainNames[nameLabExIndex] = tempMalt[0];
                grainLabExtracts[nameLabExIndex] = Convert.ToDouble(tempMalt[1]);

                nameLabExIndex++;
            }

            MaltOneLP.ItemsSource = grainNames;

            MessageBox.Show("Malts loaded from storage");
        }

        // Populate arrays with Grain 'Name' and 'Lab Extract' values (including Sugar)
        public void InstatiateMaltData()
        {
            //sugarName[0] = "Sugar";
            //sugarLabExtract = 340.0;

            //grainNames[0] = "Low Colour Maris Otter";
            //grainLabExtract[0] = 291.0;
            //grainNames[1] = "Golden Promise";
            //grainLabExtract[1] = 295.0;
            //grainNames[2] = "Wheat malt";
            //grainLabExtract[2] = 291.0;
            //grainNames[3] = "Vienna";
            //grainLabExtract[3] = 289.0;
            //grainNames[4] = "Munich";
            //grainLabExtract[4] = 285.0;
            //grainNames[5] = "Pilsner";
            //grainLabExtract[5] = 298.0;
            //grainNames[6] = "Acidulated malt";
            //grainLabExtract[6] = 0.0;
            //grainNames[7] = "Rye malt";
            //grainLabExtract[7] = 260.0;
            //grainNames[8] = "Smoked malt";
            //grainLabExtract[8] = 295.0;
            //grainNames[9] = "Oat Malt";
            //grainLabExtract[9] = 219.0;
            //grainNames[10] = "Carapils";
            //grainLabExtract[10] = 267.4;
            //grainNames[11] = "Carared";
            //grainLabExtract[11] = 268.0;
            //grainNames[12] = "Caramalt";
            //grainLabExtract[12] = 268.4;
            //grainNames[13] = "Melanoiden malt";
            //grainLabExtract[13] = 287.0;
            //grainNames[14] = "Crystal";
            //grainLabExtract[14] = 260.8;
            //grainNames[15] = "Caramunich II";
            //grainLabExtract[15] = 265.0;
            //grainNames[16] = "Dark Crystal";
            //grainLabExtract[16] = 270.8;
            //grainNames[17] = "Cara Aroma";
            //grainLabExtract[17] = 266.0;
            //grainNames[18] = "Amber";
            //grainLabExtract[18] = 266.1;
            //grainNames[19] = "Special B";
            //grainLabExtract[19] = 266.0;
            //grainNames[20] = "Brown";
            //grainLabExtract[20] = 270.0;
            //grainNames[21] = "Chocolate";
            //grainLabExtract[21] = 267.2;
            //grainNames[22] = "Roast Barley";
            //grainLabExtract[22] = 264.0;
            //grainNames[23] = "Black";
            //grainLabExtract[23] = 265.7;
            //grainNames[24] = "Carafa Special III";
            //grainLabExtract[24] = 250.0;
        }

        public void SetUpListPickers()
        {
            //SugarLP.ItemsSource = sugarName;
            //SugarLabExtTb.Text = sugarLabExtract.ToString();

            //MaltOneLP.ItemsSource = grainNames;
            //MaltTwoLP.ItemsSource = grainNames;
            //MaltThreeLP.ItemsSource = grainNames;
            //MaltFourLP.ItemsSource = grainNames;
            //MaltFiveLP.ItemsSource = grainNames;
            //MaltSixLP.ItemsSource = grainNames;
        }

        ///// Events for gathering required input data
        //
        private void MaltOneLP_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IgnoreSelectionChanged = false;
        }

        private void MaltOneLP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IgnoreSelectionChanged)
            {
                MaltOneLabExtTb.Text = GetMaltLabExtract(MaltOneLP.SelectedIndex);

                //RecalculateGrainSpecificValues(1);
                //RecalculateTotals();
            }

            IgnoreSelectionChanged = true;
        }

        private void MaltOneBillTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (MaltOneBillTb.Text == "")
            {
                MaltOneBillTb.Text = "0";
            }

            double tempLabExt = Convert.ToDouble(MaltOneLabExtTb.Text);
            double tempBill = Convert.ToDouble(MaltOneBillTb.Text);

            maltOneBill = tempBill;

            maltOneLitreDegrees = CalculateLitreDegrees(tempLabExt, tempBill);

            totalLitreDegrees = CalculateTotalLitreDegrees();

            totalMaltBill = CalculateTotalMaltBill();

            MaltOneGristLbl.Text = CalculateGrist(tempBill).ToString();

            //RecalculateGrainSpecificValues(1);
            //RecalculateTotals();

        }

        private void MaltOneLabExtTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (MaltOneLabExtTb.Text == "")
            {
                MaltOneLabExtTb.Text = GetMaltLabExtract(MaltOneLP.SelectedIndex);
            }

            double tempLabExt = Convert.ToDouble(MaltOneLabExtTb.Text);
            double tempBill = Convert.ToDouble(MaltOneBillTb.Text);

            maltOneLitreDegrees = CalculateLitreDegrees(tempLabExt, tempBill);

            totalLitreDegrees = CalculateTotalLitreDegrees();

            totalMaltBill = CalculateTotalMaltBill();

            MaltOneGristLbl.Text = CalculateGrist(tempBill).ToString();

            //RecalculateTotals();

            //RecalculateGrainSpecificValues(1);

        }
        
        private void TargetOgTb_LostFocus(object sender, RoutedEventArgs e)
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

        private void StartBoilTb_LostFocus(object sender, RoutedEventArgs e)
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

        private void MashEfficiencyTb_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                mashEfficiency = Convert.ToDouble(MashEfficiencyTb.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Efficiency of Mash must be a numeric value");
            }
        }

        ///// Custom calculation methods
        //
        //public void RecalculateTotals()
        //{
        //    totalMaltBill = CalculateTotalMaltBill();
        //
        //    totalLitreDegrees = CalculateTotalLitreDegrees();
        //
        //    UpdateGravities();
        //}

        public void RecalculateGrainSpecificValues(int grain)
        {
            switch (grain)
            {
                case 0:
                    // Sugar
                    break;
                case 1:
                    // Malt One
                    break;
                case 2:
                    // Malt Two etc.
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                default:
                    break;
            }
        }

        public string GetMaltLabExtract(int maltIndex)
        {
            return grainLabExtracts[maltIndex].ToString();
        }

        public double CalculateGrist(double tempMaltBill)
        {
            return ((tempMaltBill / totalMaltBill) * 100);
        }

        public double CalculateLitreDegrees(double tempMaltLabExtract, double tempMaltBill)
        {
            return (tempMaltLabExtract * tempMaltBill);
        }

        public double CalculateTotalMaltBill()
        {
            return maltOneBill + maltTwoBill + maltThreeBill + maltFourBill + maltFiveBill + maltSixBill;
        }

        public double CalculateTotalLitreDegrees()
        {
            return maltOneLitreDegrees + maltTwoLitreDegrees + maltThreeLitreDegrees +
                   maltFourLitreDegrees + maltFiveLitreDegrees + maltSixLitreDegrees;
        }

        public void UpdateGravities()
        {
            totalLitreDegrees = CalculateTotalLitreDegrees();

            potentialGravity = (totalLitreDegrees / startBoil) + 1000;

            double sugarLitreDegrees = CalculateLitreDegrees(sugarLabExtract, 0/*Convert.ToDouble(SugarBillTb.Text)*/);

            gravityWithEfficiency = (potentialGravity - 1000) * (mashEfficiency / 100) + 1000 + (sugarLitreDegrees / startBoil);

            PotentialGravityTb.Text = potentialGravity.ToString();
            GravityWithEfficiencyTb.Text = gravityWithEfficiency.ToString();
        }

        ///// Help messages
        //
        private void AppBarHelpBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("> TBC");
        }

        private void PotentialGravityLbl_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MessageBox.Show("Potential Gravity = \n(Litre Degrees TOTAL / Start Boil vol (L)) + 1000");
        }

        private void GravityWithEfficiencyLbl_Tap(object sender, System.Windows.Input.GestureEventArgs e)
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