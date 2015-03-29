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
        
        //private GrainsViewModel gvm;  // NOT IN USE

        // 'Malts' Grain Data
        string[] grainNames = new string[26];
        double[] grainLabExtracts = new double[26];
        double sugarLabExtract = 340.0;

        // 'Malts' User Inputs
        double targetOG = 0;
        double startBoil = 0;
        double mashEfficiency = 0;
        double grainOneLabExt = 0;      // (Default to hardcoded values)
        double grainTwoLabExt = 0;      //  <-
        double grainThreeLabExt = 0;    //  <-
        double grainFourLabExt = 0;     //  <-
        double grainFiveLabExt = 0;     //  <-
        double grainSixLabExt = 0;      //  <-
        double grainOneBill = 0;
        double grainTwoBill = 0;
        double grainThreeBill = 0;
        double grainFourBill = 0;
        double grainFiveBill = 0;
        double grainSixBill = 0;
        double sugarBill = 0;

        // 'Malts' Calculated variables
        double grainOneLitreDegrees = 0;
        double grainTwoLitreDegrees = 0;
        double grainThreeLitreDegrees = 0;
        double grainFourLitreDegrees = 0;
        double grainFiveLitreDegrees = 0;
        double grainSixLitreDegrees = 0;
        double sugarLitreDegrees = 0;
        double totalLitreDegrees = 0;

        double volumeInFV = 0;
        double endVolumeInCopper = 0;

        double endOfBoilGravity = 0;
        double totalLiquorBackVol = 0;

        double potentialGravity = 0;
        double gravityWithEfficiency = 0;

        // 'Malts' (calculated & used), 'Mash'  (used)
        double totalMaltBill = 0;

        // 'Mash' User Inputs
        double maltTemp = 0;
        double liquorGrainRatio = 0;
        double mashTemp = 0;

        // 'Mash' Calculated variables
        double mashLiquorVol = 0;
        double dipFromTopOfMt = 0;
        double strikeTemp = 0;
        double mashSize = 0;

        // 'Hops' User Inputs
        double litres = 0;

        // 'Hops' Calculated variables
        double gravityAtBoil = 0;

        // Functionality Flags
        private bool ignoreListPickerSelectionChanged = true;
        private bool requiredMaltToMashVariablesGenerated = false;

        ///// Set up XAML components, Load Grain data from IS malts.txt
        //
        public Brew()
        {
            InitializeComponent();
            LoadMaltsPageData();
        }

        public void LoadMaltsPageData()
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

            grainNames[nameLabExIndex] = "None";
            grainLabExtracts[nameLabExIndex] = 0;

            nameLabExIndex++;

            foreach (string malt in fileMaltData)
            {
                // Formats txt file into array elements
                string[] tempMalt = malt.Split(new string[] { nameLabExtractSplitter }, StringSplitOptions.None);

                grainNames[nameLabExIndex] = tempMalt[0];
                grainLabExtracts[nameLabExIndex] = RoundUpTo2SigFigs( Convert.ToDouble(tempMalt[1]) );

                nameLabExIndex++;
            }

            Grain1LP.ItemsSource = grainNames;
            Grain2LP.ItemsSource = grainNames;
            Grain3LP.ItemsSource = grainNames;
            Grain4LP.ItemsSource = grainNames;
            Grain5LP.ItemsSource = grainNames;
            Grain6LP.ItemsSource = grainNames;

            //MessageBox.Show("Malts loaded from storage");
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
        // NOT IN USE

        ///// User Pivot Page navigation management
        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Contains(PivotMash))
            {
                TotalMaltBillTb.Text = totalMaltBill.ToString();

                if (requiredMaltToMashVariablesGenerated)
                {
                    TotalMaltBillTb.Text = totalMaltBill.ToString();
                }
                else
                {
                    MessageBox.Show("Required values from 'Malts' not generated, please return and check your inputs");
                }
            }
            else if (e.AddedItems.Contains(PivotHops) || e.AddedItems.Contains(PivotGravity))
            {
                MessageBox.Show("Beta version only contains 'Malts' and 'Mash' pages' functionality.");
            }
        }
        
        ///// AppBar Events
        //
        private void AppBarHelpBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To give app feedback, leave a review on the Windows Phone Store or email:\n11215245@students.lincoln.ac.uk");
        }

        ///// --- MALTS --- UI Control Events for gathering required input data
        //
        private void TargetOgTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TargetOgTb.Text == "0")
            {
                TargetOgTb.Text = "";
            }
        }
        private void TargetOgTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TargetOgTb.Text == "")
            {
                TargetOgTb.Text = "0";
            }
            try
            {
                targetOG = RoundUpTo2SigFigs( Convert.ToDouble(TargetOgTb.Text) );
                if (targetOG != 0 && startBoil != 0 && mashEfficiency != 0)
                {
                    UpdateAndDisplayFinalMaltsVariables();
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Target OG must be a numeric value");
                TargetOgTb.Text = "0";
            }
        }
        private void StartBoilTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (StartBoilTb.Text == "0")
            {
                StartBoilTb.Text = "";
            }
        }
        private void StartBoilTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (StartBoilTb.Text == "")
            {
                StartBoilTb.Text = "0";
            }
            try
            {
                startBoil = RoundUpTo2SigFigs( Convert.ToDouble(StartBoilTb.Text) );
                if (targetOG != 0 && startBoil != 0 && mashEfficiency != 0)
                {
                    UpdateAndDisplayFinalMaltsVariables();
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Start boil must be a numeric value");
                StartBoilTb.Text = "0";
            }
        }
        private void MashEfficiencyTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (MashEfficiencyTb.Text == "0")
            {
                MashEfficiencyTb.Text = "";
            }
        }
        private void MashEfficiencyTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (MashEfficiencyTb.Text == "")
            {
                MashEfficiencyTb.Text = "0";
            }
            try
            {
                mashEfficiency = RoundUpTo2SigFigs( Convert.ToDouble(MashEfficiencyTb.Text) );
                if (targetOG != 0 && startBoil != 0 && mashEfficiency != 0)
                {
                    UpdateAndDisplayFinalMaltsVariables();
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Efficiency of Mash must be a numeric value");
                MashEfficiencyTb.Text = "0";
            }
        }

        private void SugarBillTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SugarBillTb.Text == "0")
            {
                SugarBillTb.Text = "";
            }
        }
        private void SugarBillTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SugarBillTb.Text == "")
            {
                SugarBillTb.Text = "0";
            }
            try
            {
                sugarBill = RoundUpTo2SigFigs( Convert.ToDouble(SugarBillTb.Text) );
                UpdateGrainAndFinalVariables();
            }
            catch (FormatException)
            {
                MessageBox.Show("'Bill' must be a numeric value");
                SugarBillTb.Text = "0";
            }
        }

        private void Grain1LP_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ignoreListPickerSelectionChanged = false;
        }
        private void Grain2LP_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ignoreListPickerSelectionChanged = false;
        }
        private void Grain3LP_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ignoreListPickerSelectionChanged = false;
        }
        private void Grain4LP_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ignoreListPickerSelectionChanged = false;
        }
        private void Grain5LP_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ignoreListPickerSelectionChanged = false;
        }
        private void Grain6LP_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ignoreListPickerSelectionChanged = false;
        }

        private void Grain1LP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!ignoreListPickerSelectionChanged)
            {
                if (Grain1LP.SelectedIndex == 0)
                {
                    Grain1LabExtTb.Text = "";
                    Grain1BillTb.Text = "";
                    Grain1GristLbl.Text = "";
                }
                else
                {
                    Grain1LabExtTb.Text = GetDefaultMaltLabExtract(Grain1LP.SelectedIndex);
                    Grain1BillTb.Text = "0";
                    Grain1GristLbl.Text = "0";
                }
            }

            ignoreListPickerSelectionChanged = true;
        }
        private void Grain1LabExtTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Grain1LP.SelectedIndex > 0)
            {
                if (Grain1LabExtTb.Text == "")
                {
                    Grain1LabExtTb.Text = GetDefaultMaltLabExtract(Grain1LP.SelectedIndex);
                }
                try
                {
                    grainOneLabExt = RoundUpTo2SigFigs(Convert.ToDouble(Grain1LabExtTb.Text));
                    grainOneBill = RoundUpTo2SigFigs(Convert.ToDouble(Grain1BillTb.Text));
                    UpdateGrainAndFinalVariables();
                }
                catch (FormatException)
                {
                    MessageBox.Show("'Bill' and 'Lab Extract' must be numeric values");
                    Grain1LabExtTb.Text = GetDefaultMaltLabExtract(Grain1LP.SelectedIndex);
                }
            }
        }
        private void Grain1BillTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Grain1BillTb.Text == "0")
            {
                Grain1BillTb.Text = "";
            }
        }
        private void Grain1BillTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Grain1LP.SelectedIndex > 0)
            {
                if (Grain1BillTb.Text == "")
                {
                    Grain1BillTb.Text = "0";
                }
                try
                {
                    grainOneLabExt = RoundUpTo2SigFigs(Convert.ToDouble(Grain1LabExtTb.Text));
                    grainOneBill = RoundUpTo2SigFigs(Convert.ToDouble(Grain1BillTb.Text));
                    UpdateGrainAndFinalVariables();
                }
                catch (FormatException)
                {
                    MessageBox.Show("'Bill' and 'Lab Extract' must be numeric values");
                    Grain1BillTb.Text = "0";
                }
            }
        }
        
        private void Grain2LP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!ignoreListPickerSelectionChanged)
            {
                if (Grain2LP.SelectedIndex == 0)
                {
                    Grain2LabExtTb.Text = "";
                    Grain2BillTb.Text = "";
                    Grain2GristLbl.Text = "";
                }
                else
                {
                    Grain2LabExtTb.Text = GetDefaultMaltLabExtract(Grain2LP.SelectedIndex);
                    Grain2BillTb.Text = "0";
                    Grain2GristLbl.Text = "0";
                }
            }

            ignoreListPickerSelectionChanged = true;
        }
        private void Grain2LabExtTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Grain2LP.SelectedIndex > 0)
            {
                if (Grain2LabExtTb.Text == "")
                {
                    Grain2LabExtTb.Text = GetDefaultMaltLabExtract(Grain2LP.SelectedIndex);
                }
                try
                {
                    grainTwoLabExt = RoundUpTo2SigFigs(Convert.ToDouble(Grain2LabExtTb.Text));
                    grainTwoBill = RoundUpTo2SigFigs(Convert.ToDouble(Grain2BillTb.Text));
                    UpdateGrainAndFinalVariables();
                }
                catch (FormatException)
                {
                    MessageBox.Show("'Bill' and 'Lab Extract' must be numeric values");
                    Grain2LabExtTb.Text = GetDefaultMaltLabExtract(Grain2LP.SelectedIndex);
                }
            }
        }
        private void Grain2BillTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Grain2BillTb.Text == "0")
            {
                Grain2BillTb.Text = "";
            }
        }
        private void Grain2BillTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Grain2LP.SelectedIndex > 0)
            {
                if (Grain2BillTb.Text == "")
                {
                    Grain2BillTb.Text = "0";
                }
                try
                {
                    grainTwoLabExt = RoundUpTo2SigFigs(Convert.ToDouble(Grain2LabExtTb.Text));
                    grainTwoBill = RoundUpTo2SigFigs(Convert.ToDouble(Grain2BillTb.Text));
                    UpdateGrainAndFinalVariables();
                }
                catch (FormatException)
                {
                    MessageBox.Show("'Bill' and 'Lab Extract' must be numeric values");
                    Grain2BillTb.Text = "0";
                }
            }
        }

        private void Grain3LP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!ignoreListPickerSelectionChanged)
            {
                if (Grain3LP.SelectedIndex == 0)
                {
                    Grain3LabExtTb.Text = "";
                    Grain3BillTb.Text = "";
                    Grain3GristLbl.Text = "";
                }
                else
                {
                    Grain3LabExtTb.Text = GetDefaultMaltLabExtract(Grain3LP.SelectedIndex);
                    Grain3BillTb.Text = "0";
                    Grain3GristLbl.Text = "0";
                }
            }

            ignoreListPickerSelectionChanged = true;
        }
        private void Grain3LabExtTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Grain3LP.SelectedIndex > 0)
            {
                if (Grain3LabExtTb.Text == "")
                {
                    Grain3LabExtTb.Text = GetDefaultMaltLabExtract(Grain3LP.SelectedIndex);
                }
                try
                {
                    grainThreeLabExt = RoundUpTo2SigFigs(Convert.ToDouble(Grain3LabExtTb.Text));
                    grainThreeBill = RoundUpTo2SigFigs(Convert.ToDouble(Grain3BillTb.Text));
                    UpdateGrainAndFinalVariables();
                }
                catch (FormatException)
                {
                    MessageBox.Show("'Bill' and 'Lab Extract' must be numeric values");
                    Grain3LabExtTb.Text = GetDefaultMaltLabExtract(Grain3LP.SelectedIndex);
                }
            }
        }
        private void Grain3BillTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Grain3BillTb.Text == "0")
            {
                Grain3BillTb.Text = "";
            }
        }
        private void Grain3BillTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Grain3LP.SelectedIndex > 0)
            {
                if (Grain3BillTb.Text == "")
                {
                    Grain3BillTb.Text = "0";
                }
                try
                {
                    grainThreeLabExt = RoundUpTo2SigFigs(Convert.ToDouble(Grain3LabExtTb.Text));
                    grainThreeBill = RoundUpTo2SigFigs(Convert.ToDouble(Grain3BillTb.Text));
                    UpdateGrainAndFinalVariables();
                }
                catch (FormatException)
                {
                    MessageBox.Show("'Bill' and 'Lab Extract' must be numeric values");
                    Grain3BillTb.Text = "0";
                }
            }
        }

        private void Grain4LP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!ignoreListPickerSelectionChanged)
            {
                if (Grain4LP.SelectedIndex == 0)
                {
                    Grain4LabExtTb.Text = "";
                    Grain4BillTb.Text = "";
                    Grain4GristLbl.Text = "";
                }
                else
                {
                    Grain4LabExtTb.Text = GetDefaultMaltLabExtract(Grain4LP.SelectedIndex);
                    Grain4BillTb.Text = "0";
                    Grain4GristLbl.Text = "0";
                }
            }

            ignoreListPickerSelectionChanged = true;
        }
        private void Grain4LabExtTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Grain4LP.SelectedIndex > 0)
            {
                if (Grain4LabExtTb.Text == "")
                {
                    Grain4LabExtTb.Text = GetDefaultMaltLabExtract(Grain4LP.SelectedIndex);
                }
                try
                {
                    grainFourLabExt = RoundUpTo2SigFigs(Convert.ToDouble(Grain4LabExtTb.Text));
                    grainFourBill = RoundUpTo2SigFigs(Convert.ToDouble(Grain4BillTb.Text));
                    UpdateGrainAndFinalVariables();
                }
                catch (FormatException)
                {
                    MessageBox.Show("'Bill' and 'Lab Extract' must be numeric values");
                    Grain4LabExtTb.Text = GetDefaultMaltLabExtract(Grain4LP.SelectedIndex);
                }
            }
        }
        private void Grain4BillTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Grain4BillTb.Text == "0")
            {
                Grain4BillTb.Text = "";
            }
        }
        private void Grain4BillTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Grain4LP.SelectedIndex > 0)
            {
                if (Grain4BillTb.Text == "")
                {
                    Grain4BillTb.Text = "0";
                }
                try
                {
                    grainFourLabExt = RoundUpTo2SigFigs(Convert.ToDouble(Grain4LabExtTb.Text));
                    grainFourBill = RoundUpTo2SigFigs(Convert.ToDouble(Grain4BillTb.Text));
                    UpdateGrainAndFinalVariables();
                }
                catch (FormatException)
                {
                    MessageBox.Show("'Bill' and 'Lab Extract' must be numeric values");
                    Grain4BillTb.Text = "0";
                }
            }
        }

        private void Grain5LP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!ignoreListPickerSelectionChanged)
            {
                if (Grain5LP.SelectedIndex == 0)
                {
                    Grain5LabExtTb.Text = "";
                    Grain5BillTb.Text = "";
                    Grain5GristLbl.Text = "";
                }
                else
                {
                    Grain5LabExtTb.Text = GetDefaultMaltLabExtract(Grain5LP.SelectedIndex);
                    Grain5BillTb.Text = "0";
                    Grain5GristLbl.Text = "0";
                }
            }

            ignoreListPickerSelectionChanged = true;
        }
        private void Grain5LabExtTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Grain5LP.SelectedIndex > 0)
            {
                if (Grain5LabExtTb.Text == "")
                {
                    Grain5LabExtTb.Text = GetDefaultMaltLabExtract(Grain5LP.SelectedIndex);
                }
                try
                {
                    grainFiveLabExt = RoundUpTo2SigFigs(Convert.ToDouble(Grain5LabExtTb.Text));
                    grainFiveBill = RoundUpTo2SigFigs(Convert.ToDouble(Grain5BillTb.Text));
                    UpdateGrainAndFinalVariables();
                }
                catch (FormatException)
                {
                    MessageBox.Show("'Bill' and 'Lab Extract' must be numeric values");
                    Grain5LabExtTb.Text = GetDefaultMaltLabExtract(Grain5LP.SelectedIndex);
                }
            }
        }
        private void Grain5BillTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Grain5BillTb.Text == "0")
            {
                Grain5BillTb.Text = "";
            }
        }
        private void Grain5BillTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Grain5LP.SelectedIndex > 0)
            {
                if (Grain5BillTb.Text == "")
                {
                    Grain5BillTb.Text = "0";
                }
                try
                {
                    grainFiveLabExt = RoundUpTo2SigFigs(Convert.ToDouble(Grain5LabExtTb.Text));
                    grainFiveBill = RoundUpTo2SigFigs(Convert.ToDouble(Grain5BillTb.Text));
                    UpdateGrainAndFinalVariables();
                }
                catch (FormatException)
                {
                    MessageBox.Show("'Bill' and 'Lab Extract' must be numeric values");
                    Grain5BillTb.Text = "0";
                }
            }
        }

        private void Grain6LP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!ignoreListPickerSelectionChanged)
            {
                if (Grain6LP.SelectedIndex == 0)
                {
                    Grain6LabExtTb.Text = "";
                    Grain6BillTb.Text = "";
                    Grain6GristLbl.Text = "";
                }
                else
                {
                    Grain6LabExtTb.Text = GetDefaultMaltLabExtract(Grain6LP.SelectedIndex);
                    Grain6BillTb.Text = "0";
                    Grain6GristLbl.Text = "0";
                }
            }

            ignoreListPickerSelectionChanged = true;
        }
        private void Grain6LabExtTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Grain6LP.SelectedIndex > 0)
            {
                if (Grain6LabExtTb.Text == "")
                {
                    Grain6LabExtTb.Text = GetDefaultMaltLabExtract(Grain6LP.SelectedIndex);
                }
                try
                {
                    grainSixLabExt = RoundUpTo2SigFigs(Convert.ToDouble(Grain6LabExtTb.Text));
                    grainSixBill = RoundUpTo2SigFigs(Convert.ToDouble(Grain6BillTb.Text));
                    UpdateGrainAndFinalVariables();
                }
                catch (FormatException)
                {
                    MessageBox.Show("'Bill' and 'Lab Extract' must be numeric values");
                    Grain6LabExtTb.Text = GetDefaultMaltLabExtract(Grain6LP.SelectedIndex);
                }
            }
        }
        private void Grain6BillTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Grain6BillTb.Text == "0")
            {
                Grain6BillTb.Text = "";
            }
        }
        private void Grain6BillTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Grain6LP.SelectedIndex > 0)
            {
                if (Grain6BillTb.Text == "")
                {
                    Grain6BillTb.Text = "0";
                }
                try
                {
                    grainSixLabExt = RoundUpTo2SigFigs(Convert.ToDouble(Grain6LabExtTb.Text));
                    grainSixBill = RoundUpTo2SigFigs(Convert.ToDouble(Grain6BillTb.Text));
                    UpdateGrainAndFinalVariables();
                }
                catch (FormatException)
                {
                    MessageBox.Show("'Bill' and 'Lab Extract' must be numeric values");
                    Grain6BillTb.Text = "0";
                }
            }
        }

        ///// --- MALTS --- Custom calculation methods
        //
        public string GetDefaultMaltLabExtract(int maltIndex)
        {
            return grainLabExtracts[maltIndex].ToString();
        }

        public void UpdateGrainAndFinalVariables()
        {
            if (targetOG != 0 && startBoil != 0 && mashEfficiency != 0)
            {
                // Updates: TotalMaltBill and all current Grist & LitreDegrees values
                CalculateTotalMaltBill();

                if (Convert.ToDouble(SugarBillTb.Text) > 0)
                    SugarGristLbl.Text = GetGrist(sugarBill).ToString();
                    sugarLitreDegrees = GetLitreDegrees(sugarLabExtract, sugarBill);
                if (Grain1LP.SelectedIndex > 0)
                    Grain1GristLbl.Text = GetGrist(grainOneBill).ToString();
                    grainOneLitreDegrees = GetLitreDegrees(grainOneLabExt, grainOneBill);
                if (Grain2LP.SelectedIndex > 0)
                    Grain2GristLbl.Text = GetGrist(grainTwoBill).ToString();
                    grainTwoLitreDegrees = GetLitreDegrees(grainTwoLabExt, grainTwoBill);
                if (Grain3LP.SelectedIndex > 0)
                    Grain3GristLbl.Text = GetGrist(grainThreeBill).ToString();
                    grainThreeLitreDegrees = GetLitreDegrees(grainThreeLabExt, grainThreeBill);
                if (Grain4LP.SelectedIndex > 0)
                    Grain4GristLbl.Text = GetGrist(grainFourBill).ToString();
                    grainFourLitreDegrees = GetLitreDegrees(grainFourLabExt, grainFourBill);
                if (Grain5LP.SelectedIndex > 0)
                    Grain5GristLbl.Text = GetGrist(grainFiveBill).ToString();
                    grainFiveLitreDegrees = GetLitreDegrees(grainFiveLabExt, grainFiveBill);
                if (Grain6LP.SelectedIndex > 0)
                    Grain6GristLbl.Text = GetGrist(grainSixBill).ToString();
                    grainSixLitreDegrees = GetLitreDegrees(grainSixLabExt, grainSixBill);

                if (totalMaltBill != 0)
                {
                    // Updates: TotalLitreDegrees, PotentialGravity, GravityWithEfficiency,
                    //          EndVolumeInCopper, VolumeInFV, EndOfBoilGravity, TotalLiquorBackVol
                    UpdateAndDisplayFinalMaltsVariables();
                }
            }
            else
            {
                MessageBox.Show("Add Target OG, Start Boil & Mash Efficiency values to see this Brew's Grain and Gravity data");
            }
        }

            public void CalculateTotalMaltBill()
            {
                totalMaltBill = RoundUpTo2SigFigs( grainOneBill + grainTwoBill + grainThreeBill + 
                                                    grainFourBill + grainFiveBill + grainSixBill );
            }

            public double GetGrist(double inputBill)
            {
                double grist;

                if (totalMaltBill == 0)
                {
                    grist = 0;
                    //MessageBox.Show("Add grain(s) with Bill values to display Grist percentages");
                }
                else
                {
                    grist = (inputBill / totalMaltBill) * 100;
                }
                return RoundUpTo2SigFigs( grist );
            }

            public double GetLitreDegrees(double inputLabExtract, double inputBill)
            {
                return RoundUpTo2SigFigs( (inputLabExtract * inputBill) );
            }

        public void UpdateAndDisplayFinalMaltsVariables()
        {
            CalculateTotalLiquorBackVol();

            PotentialGravityTb.Text = potentialGravity.ToString();
            GravityWithEfficiencyTb.Text = gravityWithEfficiency.ToString();

            MessageBox.Show("Target OG:\t\t" + targetOG + "\n" +
                            "Start Boil:\t\t" + startBoil+ "\n" +
                            "Efficiency of Mash:\t" + mashEfficiency + "\n\n" + 
                            
                            "Total Litre Degrees:\t" + totalLitreDegrees + "\n" + 
                            "Total Malt Bill:\t\t" + totalMaltBill + "\n" +
                            "End of Boil Gravity:\t" + endOfBoilGravity + "\n" + 
                            "Total Liquor Back Vol:\t" + totalLiquorBackVol + "\n" + 
                            "Volume In FV:\t\t" + volumeInFV + "\n" +
                            "End Volume In Copper:\t" + endVolumeInCopper);

            requiredMaltToMashVariablesGenerated = true;
        }

            public void CalculateTotalLiquorBackVol()
            {
                CalculateEndOfBoilGravity();

                // GETS SLIGHTLY DIFFERENT (LOWER) VALUE TO SPREADSHEET, BUT WAS TESTED TO BE MATHEMATICALLY SOUND (???)
                totalLiquorBackVol = RoundUpTo2SigFigs( (volumeInFV * (endOfBoilGravity - 1000) / (targetOG - 1000)) - volumeInFV );
            }

                public void CalculateEndOfBoilGravity()
                {
                    CalculateGravityWithEfficiency();
                    CalculateVolumeInFV();

                    endOfBoilGravity = RoundUpTo1SigFig( (((startBoil * (gravityWithEfficiency - 1000)) / (endVolumeInCopper)) + 1000) );
                }

                    public void CalculateGravityWithEfficiency()
                    {
                        CalculatePotentialGravity();

                        gravityWithEfficiency = RoundUpTo2SigFigs( (potentialGravity - 1000) * (mashEfficiency / 100) +
                                                                   1000 + (sugarLitreDegrees / startBoil) );
                    }

                        public void CalculatePotentialGravity()
                        {
                            CalculateTotalLitreDegrees();
                            potentialGravity = RoundUpTo2SigFigs( (totalLitreDegrees / startBoil) + 1000 );
                        }

                            public void CalculateTotalLitreDegrees()
                            {
                                totalLitreDegrees = RoundUpTo2SigFigs( grainOneLitreDegrees + grainTwoLitreDegrees + 
                                                    grainThreeLitreDegrees + grainFourLitreDegrees + 
                                                    grainFiveLitreDegrees + grainSixLitreDegrees );
                            }

                    public void CalculateVolumeInFV()
                    {
                        CalculateEndVolumeInCopper();
                        volumeInFV = RoundUpTo2SigFigs( ((endVolumeInCopper - (0.04 * endVolumeInCopper)) - 1.5) );
                    }

                        public void CalculateEndVolumeInCopper()
                        {
                            endVolumeInCopper = RoundUpTo1SigFig( startBoil - (startBoil * 0.045) );
                        }

        public double RoundUpTo2SigFigs(double value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        public double RoundUpTo1SigFig(double value)
        {
            return Math.Round(value, 1, MidpointRounding.AwayFromZero);
        }

        ///// --- MASH --- UI Control Events for gathering required input data
        //
        private void MaltTempTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (MaltTempTb.Text == "0")
            {
                MaltTempTb.Text = "";
            }
        }
        private void MaltTempTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (MaltTempTb.Text == "")
            {
                MaltTempTb.Text = "0";
            }
            else
            {
                try
                {
                    maltTemp = RoundUpTo2SigFigs(Convert.ToDouble(MaltTempTb.Text));
                    if (maltTemp != 0 && liquorGrainRatio != 0)
                    {
                        UpdateAndDisplayFinalMashVariables();
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Malt Temp must be a numeric value");
                    MaltTempTb.Text = "0";
                }
            }
        }
        private void LiquorGrainRatioTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LiquorGrainRatioTb.Text == "0")
            {
                LiquorGrainRatioTb.Text = "";
            }
        }
        private void LiquorGrainRatioTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (LiquorGrainRatioTb.Text == "")
            {
                LiquorGrainRatioTb.Text = "0";
            }
            else
            {
                try
                {
                    liquorGrainRatio = RoundUpTo2SigFigs(Convert.ToDouble(LiquorGrainRatioTb.Text));
                    if (maltTemp != 0 && liquorGrainRatio != 0)
                    {
                        UpdateAndDisplayFinalMashVariables();
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Liquor-Grain Ratio must be a numeric value");
                    LiquorGrainRatioTb.Text = "0";
                }
            }
        }
        private void MashTempTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (MashTempTb.Text == "0")
            {
                MashTempTb.Text = "";
            }
        }
        private void MashTempTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (MashTempTb.Text == "")
            {
                MashTempTb.Text = "0";
            }
            else
            {
                try
                {
                    mashTemp = RoundUpTo2SigFigs(Convert.ToDouble(MashTempTb.Text));
                    if (maltTemp != 0 && liquorGrainRatio != 0)
                    {
                        UpdateAndDisplayFinalMashVariables();
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Mash Temp must be a numeric value");
                    MashTempTb.Text = "0";
                }
            }
        }

        ///// --- MASH --- Custom calculation methods
        //
        public void UpdateAndDisplayFinalMashVariables()
        {
            CalculateDipFromTopOfMt();
            MashLiquorVolumeTb.Text = mashLiquorVol.ToString();
            DipFromTopOfMtTb.Text = dipFromTopOfMt.ToString();

            if (mashTemp != 0)
            {
                CalculateStrikeTemp();
                CalculateMashSize();
                StrikeTempTb.Text = strikeTemp.ToString();
                MashSizeTb.Text = mashSize.ToString();
            }
        }

            public void CalculateDipFromTopOfMt()
            {
                CalculateMashLiquorVolume();

                double hardcodedValue1 = 45;
                double hardcodedValue2 = 27;
                double hardcodedValue3 = 0.25;
                double dipInnerCalculationResult = (3.1415 * hardcodedValue2 * hardcodedValue2 * hardcodedValue3);

                dipFromTopOfMt = (hardcodedValue1 - ((mashLiquorVol * 1000) / dipInnerCalculationResult));
            }

                public void CalculateMashLiquorVolume()
                {
                    mashLiquorVol = RoundUpTo2SigFigs(totalMaltBill * liquorGrainRatio);
                }
            
            public void CalculateStrikeTemp()
            {
                strikeTemp = RoundUpTo1SigFig( (((1525 * totalMaltBill * (mashTemp - maltTemp)) / (4184 * mashLiquorVol)) + mashTemp) );
            }

            public void CalculateMashSize()
            {
                mashSize = RoundUpTo1SigFig( ((totalMaltBill * 0.67) + mashLiquorVol + 1) );
            }

        ///// --- HOPS --- Custom calculation methods
        //
        public void UpdateAndDisplayFinalHopsVariables()
        {

        }
            public void CalculateIbu()
            {

            }
            public void CalculateTotalIbu()
            {

            }
                public void CalculateTotalWeight()
                {

                }

        ///// --- HOPS --- UI Control Events for gathering required input data
        //
        private void LitresTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LitresTb.Text == "0")
            {
                LitresTb.Text = "";
            }
        }
        private void LitresTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (LitresTb.Text == "")
            {
                LitresTb.Text = "0";
            }
            else
            {
                try
                {
                    litres = RoundUpTo2SigFigs(Convert.ToDouble(LitresTb.Text));
                    gravityAtBoil = gravityWithEfficiency;

                    if (litres != 0 && gravityAtBoil != 0)
                    {
                        UpdateAndDisplayFinalHopsVariables();
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Litres must be a numeric value");
                    LitresTb.Text = "0";
                }
            }
        }

        private void Hop1LP_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ignoreListPickerSelectionChanged = false;
        }
        private void Hop2LP_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ignoreListPickerSelectionChanged = false;
        }
        private void Hop3LP_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ignoreListPickerSelectionChanged = false;
        }
        private void Hop4LP_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ignoreListPickerSelectionChanged = false;
        }
        private void Hop5LP_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ignoreListPickerSelectionChanged = false;
        }
        private void Hop6LP_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ignoreListPickerSelectionChanged = false;
        }

        private void Hop1LP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Hop1AlphaTb_GotFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop1AlphaTb_LostFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop1UtilTb_GotFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop1UtilTb_LostFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop1WeightTb_GotFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop1WeightTb_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Hop2LP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Hop2AlphaTb_GotFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop2AlphaTb_LostFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop2UtilTb_GotFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop2UtilTb_LostFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop2WeightTb_GotFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop2WeightTb_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Hop3LP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Hop3AlphaTb_GotFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop3AlphaTb_LostFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop3UtilTb_GotFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop3UtilTb_LostFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop3WeightTb_GotFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop3WeightTb_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Hop4LP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Hop4AlphaTb_GotFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop4AlphaTb_LostFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop4UtilTb_GotFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop4UtilTb_LostFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop4WeightTb_GotFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop4WeightTb_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Hop5LP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Hop5AlphaTb_GotFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop5AlphaTb_LostFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop5UtilTb_GotFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop5UtilTb_LostFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop5WeightTb_GotFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop5WeightTb_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Hop6LP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Hop6AlphaTb_GotFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop6AlphaTb_LostFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop6UtilTb_GotFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop6UtilTb_LostFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop6WeightTb_GotFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Hop6WeightTb_LostFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}