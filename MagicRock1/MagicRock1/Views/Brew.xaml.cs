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
        string[] grainNames = new string[26];
        double[] grainLabExtracts = new double[26];
        double sugarLabExtract = 340.0;

        // User-input (Malts top)
        double targetOG = 0;
        double startBoil = 0;
        double mashEfficiency = 0;

        // User-input (Malts rows)
        double grainOneLabExt = 0;
        double grainTwoLabExt = 0;
        double grainThreeLabExt = 0;
        double grainFourLabExt = 0;
        double grainFiveLabExt = 0;
        double grainSixLabExt = 0;

        double grainOneBill = 0;
        double grainTwoBill = 0;
        double grainThreeBill = 0;
        double grainFourBill = 0;
        double grainFiveBill = 0;
        double grainSixBill = 0;
        double sugarBill = 0;

        // Calculated based on user-input values
        double grainOneLitreDegrees = 0;
        double grainTwoLitreDegrees = 0;
        double grainThreeLitreDegrees = 0;
        double grainFourLitreDegrees = 0;
        double grainFiveLitreDegrees = 0;
        double grainSixLitreDegrees = 0;
        double sugarLitreDegrees = 0;

        // Vary depending on number and quantity of malts used
        double totalMaltBill = 0;
        double totalLitreDegrees = 0;

        // Needed to calculate below values
        double volumeInFV = 0;
        double endVolumeInCopper = 0;

        // Calculated on 'Malts', passed for use on 'Mash'
        double endOfBoilGravity = 0;
        double totalLiquorBackVol = 0;

        // Dynamic variables (Malts bottom)
        double potentialGravity = 0;
        double gravityWithEfficiency = 0;

        // Used to select appropriate Listpicker actions
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
            int nameLabExIndex = 1;

            grainNames[nameLabExIndex] = "None";
            grainLabExtracts[nameLabExIndex] = 0;

            foreach (string malt in fileMaltData)
            {
                // Formats txt file into array elements
                string[] tempMalt = malt.Split(new string[] { nameLabExtractSplitter }, StringSplitOptions.None);

                grainNames[nameLabExIndex] = tempMalt[0];
                grainLabExtracts[nameLabExIndex] = Convert.ToDouble(tempMalt[1]);

                nameLabExIndex++;
            }

            Grain1LP.ItemsSource = grainNames;
            Grain2LP.ItemsSource = grainNames;
            Grain3LP.ItemsSource = grainNames;
            Grain4LP.ItemsSource = grainNames;
            Grain5LP.ItemsSource = grainNames;
            Grain6LP.ItemsSource = grainNames;

            MessageBox.Show("Malts loaded from storage");
        }

        // NOT IN USE - Populate arrays with Grain 'Name' and 'Lab Extract' values (including Sugar)
        /*public void InstatiateMaltData()
        {
            sugarName[0] = "Sugar";
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

        public void SetUpListPickers()
        {
            SugarLP.ItemsSource = sugarName;
            SugarLabExtTb.Text = sugarLabExtract.ToString();

            MaltOneLP.ItemsSource = grainNames;
            MaltTwoLP.ItemsSource = grainNames;
            MaltThreeLP.ItemsSource = grainNames;
            MaltFourLP.ItemsSource = grainNames;
            MaltFiveLP.ItemsSource = grainNames;
            MaltSixLP.ItemsSource = grainNames;
        }*/

        ///// UI Control Events for gathering required input data
        //
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
        
        private void SugarBillTb_LostFocus(object sender, RoutedEventArgs e)
        {
            sugarBill = Convert.ToDouble(SugarBillTb.Text);
            UpdateGrainAndTotals(0);
        }

        private void Grain1LP_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IgnoreSelectionChanged = false;
        }
        private void Grain2LP_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IgnoreSelectionChanged = false;
        }
        private void Grain3LP_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IgnoreSelectionChanged = false;
        }
        private void Grain4LP_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IgnoreSelectionChanged = false;
        }
        private void Grain5LP_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IgnoreSelectionChanged = false;
        }
        private void Grain6LP_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IgnoreSelectionChanged = false;
        }

        private void Grain1LP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IgnoreSelectionChanged)
            {
                if (Grain1LP.SelectedIndex == 0)
                {
                    Grain1LabExtTb.Text = "";
                    Grain1BillTb.Text = "";
                }
                else
                {
                    Grain1LabExtTb.Text = GetDefaultMaltLabExtract(Grain1LP.SelectedIndex);
                    Grain1BillTb.Text = "0";
                }
            }

            IgnoreSelectionChanged = true;
        }
        private void Grain1LabExtTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Grain1LabExtTb.Text == "")
            {
                Grain1LabExtTb.Text = GetDefaultMaltLabExtract(Grain1LP.SelectedIndex);
            }

            grainOneLabExt = Convert.ToDouble(Grain1LabExtTb.Text);
            grainOneBill = Convert.ToDouble(Grain1BillTb.Text);

            UpdateGrainAndTotals(1);
        }
        private void Grain1BillTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Grain1BillTb.Text == "")
            {
                Grain1BillTb.Text = "0";
            }

            grainOneLabExt = Convert.ToDouble(Grain1LabExtTb.Text);
            grainOneBill = Convert.ToDouble(Grain1BillTb.Text);

            UpdateGrainAndTotals(1);
        }

        private void Grain2LP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Grain2LabExtTb_LostFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Grain2BillTb_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Grain3LP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Grain3LabExtTb_LostFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Grain3BillTb_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Grain4LP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Grain4LabExtTb_LostFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Grain4BillTb_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Grain5LP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Grain5LabExtTb_LostFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Grain5BillTb_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Grain6LP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Grain6LabExtTb_LostFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Grain6BillTb_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        ///// Custom calculation methods
        //
        public void UpdateGrainAndTotals(int grain)
        {
            switch (grain)
            {
                case 0:
                    // Sugar
                    SugarGristLbl.Text = GetGrist(sugarBill).ToString();
                    sugarLitreDegrees = GetLitreDegrees(sugarLabExtract, sugarBill);
                    break;
                case 1:
                    // Grain 1
                    Grain1GristLbl.Text = GetGrist(grainOneBill).ToString();
                    grainOneLitreDegrees = GetLitreDegrees(grainOneLabExt, grainOneBill);
                    break;
                case 2:
                    // Grain 2
                    Grain2GristLbl.Text = GetGrist(grainTwoBill).ToString();
                    grainTwoLitreDegrees = GetLitreDegrees(grainTwoLabExt, grainTwoBill);
                    break;
                case 3:
                    // Grain 3
                    Grain3GristLbl.Text = GetGrist(grainThreeBill).ToString();
                    grainThreeLitreDegrees = GetLitreDegrees(grainThreeLabExt, grainThreeBill);
                    break;
                case 4:
                    // Grain 4
                    Grain4GristLbl.Text = GetGrist(grainFourBill).ToString();
                    grainFourLitreDegrees = GetLitreDegrees(grainFourLabExt, grainFourBill);
                    break;
                case 5:
                    // Grain 5
                    Grain5GristLbl.Text = GetGrist(grainFiveBill).ToString();
                    grainFiveLitreDegrees = GetLitreDegrees(grainFiveLabExt, grainFiveBill);
                    break;
                case 6:
                    // Grain 6
                    Grain6GristLbl.Text = GetGrist(grainSixBill).ToString();
                    grainSixLitreDegrees = GetLitreDegrees(grainSixLabExt, grainSixBill);
                    break;
                default:
                    // Default
                    MessageBox.Show("Error: Could not update Grain & Totals");
                    break;
            }

            CalculateTotalLiquorBackVol();

            UpdateGravityTextblocks();
        }

        public string GetDefaultMaltLabExtract(int maltIndex)
        {
            return grainLabExtracts[maltIndex].ToString();
        }

        public double GetLitreDegrees(double inputLabExtract, double inputBill)
        {
            return (inputLabExtract * inputBill);
        }

        public double GetGrist(double inputBill)
        {
            double grist;

            CalculateTotalMaltBill();

            if (totalMaltBill == 0)
            {
                grist = 0;
                MessageBox.Show("Add grain(s) will Bill values to display Grist percentages");
            }
            else
            {
                grist = (inputBill / totalMaltBill) * 100;
            }

            return grist;
        }

            public void CalculateTotalMaltBill()
            {
                totalMaltBill = grainOneBill + grainTwoBill + grainThreeBill + grainFourBill + grainFiveBill + grainSixBill;
            }

        public void CalculateTotalLiquorBackVol()
        {
            CalculateEndOfBoilGravity();

            totalLiquorBackVol = (volumeInFV * (endOfBoilGravity - 1000)) / ((targetOG - 1000) - volumeInFV);
        }

            public void CalculateEndOfBoilGravity()
            {
                CalculateGravityWithEfficiency();
                CalculateVolumeInFV();

                endOfBoilGravity = (((startBoil * (gravityWithEfficiency - 1000)) / (endVolumeInCopper)) + 1000);
            }

                public void CalculateGravityWithEfficiency()
                {
                    CalculatePotentialGravity();

                    gravityWithEfficiency = (potentialGravity - 1000) * (mashEfficiency / 100) + 1000 + (sugarLitreDegrees / startBoil);
                }

                    public void CalculatePotentialGravity()
                    {
                        CalculateTotalLitreDegrees();
                        potentialGravity = (totalLitreDegrees / startBoil) + 1000;
                    }

                        public void CalculateTotalLitreDegrees()
                        {
                            totalLitreDegrees = grainOneLitreDegrees + grainTwoLitreDegrees + grainThreeLitreDegrees +
                                                grainFourLitreDegrees + grainFiveLitreDegrees + grainSixLitreDegrees;
                        }

                public void CalculateVolumeInFV()
                {
                    CalculateEndVolumeInCopper();
                    volumeInFV = ((endVolumeInCopper - (0.04 * endVolumeInCopper)) - 1.5);
                }

                    public void CalculateEndVolumeInCopper()
                    {
                        endVolumeInCopper = startBoil - (startBoil * 0.045);
                    }

        public void UpdateGravityTextblocks()
        {
            PotentialGravityTb.Text = potentialGravity.ToString();
            GravityWithEfficiencyTb.Text = gravityWithEfficiency.ToString();
        }

        ///// Help messages
        //
        private void AppBarHelpBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("> TBC");
        }

        private void AppBarNextBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("> TBC");
        }

        private void SugarNameCoverup_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBox.Show("Sugar cannot be changed. Leave Bill at '0' if not used in Brew");
        }

        private void SugarLabExtCoverup_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBox.Show("Sugar cannot be changed. Leave Bill at '0' if not used in Brew");
        }
    }
}