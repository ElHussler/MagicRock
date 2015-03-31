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
        
        // 'Malts' Grain Data
        string[] grainNames = new string[26];
        double[] grainLabExtracts = new double[26];
        double sugarLabExtract = 340.0;

        // 'Malts' User Inputs
        double targetOG = 0;
        double startBoilVol = 0;
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
        double hopOneAlpha = 0;
        double hopTwoAlpha = 0;
        double hopThreeAlpha = 0;
        double hopFourAlpha = 0;
        double hopFiveAlpha = 0;
        double hopSixAlpha = 0;
        double hopOneUtil = 0;
        double hopTwoUtil = 0;
        double hopThreeUtil = 0;
        double hopFourUtil = 0;
        double hopFiveUtil = 0;
        double hopSixUtil = 0;
        double hopOneWeight = 0;
        double hopTwoWeight = 0;
        double hopThreeWeight = 0;
        double hopFourWeight = 0;
        double hopFiveWeight = 0;
        double hopSixWeight = 0;

        // 'Hops' Calculated variables
        double gravityAtBoil = 0;
        double hopOneIbu = 0;
        double hopTwoIbu = 0;
        double hopThreeIbu = 0;
        double hopFourIbu = 0;
        double hopFiveIbu = 0;
        double hopSixIbu = 0;
        double totalHopWeight = 0;
        double totalHopIbu = 0;

        // Functionality Flags
        private bool ignoreListPickerSelectionChanged = true;
        private bool requiredMashVariablesGenerated = false;
        private bool requiredHopsVariablesGenerated = false;
        private bool requiredGravityVariablesGenerated = false;

        string messageBoxTextDefault = "To give app feedback, leave a review on the Windows Phone Store or email:\n11215245@students.lincoln.ac.uk";
        string messageBoxText = "";

        ///// Set up XAML components, Load Grain data from IS malts.txt
        //
        public Brew()
        {
            InitializeComponent();
            LoadMaltsGrainData();
        }

        public void LoadMaltsGrainData()
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

        ///// User Pivot Page navigation management
        //
        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Contains(PivotMalts))
            {
                //messageBoxText = "'Clear Brew' option currently in development, manual deletion required until update.";
                //MessageBox.Show(messageBoxText);
            }
            else if (e.AddedItems.Contains(PivotMash))
            {
                if (requiredMashVariablesGenerated)
                {
                    TotalMaltBillTb.Text = totalMaltBill.ToString();
                    if (maltTemp != 0 && liquorGrainRatio != 0)
                    {
                        UpdateAndDisplayFinalMashVariables();
                    }
                }
                else
                {
                    MessageBox.Show("Required values from 'Malts' not generated, please return and check your inputs");
                }
            }
            else if (e.AddedItems.Contains(PivotHops))
            {
                if (requiredHopsVariablesGenerated)
                {
                    gravityAtBoil = gravityWithEfficiency;
                    GravityAtBoilTb.Text = gravityAtBoil.ToString();

                    UpdateHopAndFinalHopsVariables();
                }
                else
                {
                    MessageBox.Show("Required values from 'Malts' and 'Mash' not generated, please return and check your inputs");
                }
            }
            else if (e.AddedItems.Contains(PivotGravity))
            {
                if (requiredGravityVariablesGenerated)
                {
                    FinalPotentialGravityTb.Text = potentialGravity.ToString();
                    FinalGravityWithEfficiencyTb.Text = gravityWithEfficiency.ToString();
                    FinalTotalLiquorBackTb.Text = totalLiquorBackVol.ToString();
                    FinalEndOfBoilGravityTb.Text = endOfBoilGravity.ToString();
                }
                else
                {
                    MessageBox.Show("Required values from previous pages not generated, please return and check your inputs");
                }
            }
        }

        ///// Mathematic Rounding methods
        //
        public double RoundUpTo1SigFig(double value)
        {
            return Math.Round(value, 1, MidpointRounding.AwayFromZero);
        }
        public double RoundUpTo2SigFigs(double value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }
        
        ///// AppBar Events
        //
        private void AppBarHelpBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show(messageBoxTextDefault);
        }
        private void AppBarDetailsBtn_Click(object sender, EventArgs e)
        {
            ShowMoreBrewValues();
        }

        public void ShowMoreBrewValues()
        {
            MessageBox.Show("Target OG:\t\t" + targetOG + "\n" +
                            "Start Boil:\t\t" + startBoilVol + "\n" +
                            "Efficiency of Mash:\t" + mashEfficiency + "\n\n" +

                            "Total Litre Degrees:\t" + totalLitreDegrees + "\n" +
                            "Total Malt Bill:\t\t" + totalMaltBill + "\n\n" +

                            "End of Boil Gravity:\t" + endOfBoilGravity + "\n" +
                            "Total Liquor Back Vol:\t" + totalLiquorBackVol + "\n" +
                            "Volume In FV:\t\t" + volumeInFV + "\n" +
                            "End Volume In Copper:\t" + endVolumeInCopper);
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
                
                UpdateGrainAndFinalMaltsVariables();
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
                startBoilVol = RoundUpTo2SigFigs(Convert.ToDouble(StartBoilTb.Text));

                UpdateGrainAndFinalMaltsVariables();
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
                
                UpdateGrainAndFinalMaltsVariables();
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
                UpdateGrainAndFinalMaltsVariables();
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
                    grainOneBill = 0;
                }
                else
                {
                    Grain1LabExtTb.Text = GetDefaultMaltLabExtract(Grain1LP.SelectedIndex);
                    Grain1BillTb.Text = "0";
                    Grain1GristLbl.Text = "0";
                }

                UpdateGrainAndFinalMaltsVariables();
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
                    UpdateGrainAndFinalMaltsVariables();
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
                    UpdateGrainAndFinalMaltsVariables();
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
                    grainTwoBill = 0;
                }
                else
                {
                    Grain2LabExtTb.Text = GetDefaultMaltLabExtract(Grain2LP.SelectedIndex);
                    Grain2BillTb.Text = "0";
                    Grain2GristLbl.Text = "0";
                }

                UpdateGrainAndFinalMaltsVariables();
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
                    UpdateGrainAndFinalMaltsVariables();
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
                    UpdateGrainAndFinalMaltsVariables();
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
                    grainThreeBill = 0;
                }
                else
                {
                    Grain3LabExtTb.Text = GetDefaultMaltLabExtract(Grain3LP.SelectedIndex);
                    Grain3BillTb.Text = "0";
                    Grain3GristLbl.Text = "0";
                }

                UpdateGrainAndFinalMaltsVariables();
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
                    UpdateGrainAndFinalMaltsVariables();
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
                    UpdateGrainAndFinalMaltsVariables();
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
                    grainFourBill = 0;
                }
                else
                {
                    Grain4LabExtTb.Text = GetDefaultMaltLabExtract(Grain4LP.SelectedIndex);
                    Grain4BillTb.Text = "0";
                    Grain4GristLbl.Text = "0";
                }

                UpdateGrainAndFinalMaltsVariables();
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
                    UpdateGrainAndFinalMaltsVariables();
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
                    UpdateGrainAndFinalMaltsVariables();
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
                    grainFiveBill = 0;
                }
                else
                {
                    Grain5LabExtTb.Text = GetDefaultMaltLabExtract(Grain5LP.SelectedIndex);
                    Grain5BillTb.Text = "0";
                    Grain5GristLbl.Text = "0";
                }

                UpdateGrainAndFinalMaltsVariables();
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
                    UpdateGrainAndFinalMaltsVariables();
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
                    UpdateGrainAndFinalMaltsVariables();
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
                    grainSixBill = 0;
                }
                else
                {
                    Grain6LabExtTb.Text = GetDefaultMaltLabExtract(Grain6LP.SelectedIndex);
                    Grain6BillTb.Text = "0";
                    Grain6GristLbl.Text = "0";
                }

                UpdateGrainAndFinalMaltsVariables();
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
                    UpdateGrainAndFinalMaltsVariables();
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
                    UpdateGrainAndFinalMaltsVariables();
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

        public void UpdateGrainAndFinalMaltsVariables()
        {
            if (targetOG != 0 && startBoilVol != 0 && mashEfficiency != 0)
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

                // Updates: TotalLitreDegrees, PotentialGravity, GravityWithEfficiency,
                //          EndVolumeInCopper, VolumeInFV, EndOfBoilGravity, TotalLiquorBackVol
                UpdateAndDisplayFinalMaltsVariables();
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

                //ShowMoreBrewValues();

                if (totalMaltBill != 0)
                {
                    requiredMashVariablesGenerated = true;
                }
                if (gravityWithEfficiency != 0)
                {
                    requiredHopsVariablesGenerated = true;
                }
                if ((gravityWithEfficiency != 0) && (potentialGravity != 0) && (endOfBoilGravity != 0) && (totalLiquorBackVol != 0))
                {
                    requiredGravityVariablesGenerated = true;
                }
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

                        endOfBoilGravity = RoundUpTo1SigFig( (((startBoilVol * (gravityWithEfficiency - 1000)) / (endVolumeInCopper)) + 1000) );
                    }

                        public void CalculateGravityWithEfficiency()
                        {
                            CalculatePotentialGravity();

                            gravityWithEfficiency = RoundUpTo2SigFigs( (potentialGravity - 1000) * (mashEfficiency / 100) +
                                                                       1000 + (sugarLitreDegrees / startBoilVol) );
                        }

                            public void CalculatePotentialGravity()
                            {
                                CalculateTotalLitreDegrees();
                                potentialGravity = RoundUpTo2SigFigs( (totalLitreDegrees / startBoilVol) + 1000 );
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
                                endVolumeInCopper = RoundUpTo1SigFig( startBoilVol - (startBoilVol * 0.045) );
                            }

        ///// --- MASH  --- UI Control Events for gathering required input data
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

        ///// --- MASH  --- Custom calculation methods
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

        ///// --- HOPS  --- UI Control Events for gathering required input data
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
            try
            {
                litres = Convert.ToDouble(LitresTb.Text);

                UpdateHopAndFinalHopsVariables();
            }
            catch (FormatException)
            {
                MessageBox.Show("Litres must be a numeric value");
                LitresTb.Text = "0";
            }
        }

        private void Hop1Tb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Hop1Tb.Text == "")
            {
                Hop1AlphaTb.Text = "0";
                Hop1UtilTb.Text = "0";
                Hop1WeightTb.Text = "0";

                UpdateHopAndFinalHopsVariables();

                Hop1AlphaTb.Text = "";
                Hop1UtilTb.Text = "";
                Hop1WeightTb.Text = "";
                Hop1IbuLbl.Text = "";
            }
        }
        private void Hop1AlphaTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Hop1AlphaTb.Text == "0")
            {
                Hop1AlphaTb.Text = "";
            }
        }
        private void Hop1AlphaTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Hop1AlphaTb.Text == "0")
            {
                Hop1AlphaTb.Text = "";
            }
            try
            {
                hopOneAlpha = RoundUpTo2SigFigs( Convert.ToDouble(Hop1AlphaTb.Text) );

                UpdateHopAndFinalHopsVariables();
            }
            catch (FormatException)
            {
                MessageBox.Show("Alpha must be a numeric value");
                Hop1AlphaTb.Text = "0";
            }
        }
        private void Hop1UtilTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Hop1UtilTb.Text == "0")
            {
                Hop1UtilTb.Text = "";
            }
        }
        private void Hop1UtilTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Hop1UtilTb.Text == "0")
            {
                Hop1UtilTb.Text = "";
            }
            try
            {
                hopOneUtil = RoundUpTo2SigFigs( Convert.ToDouble(Hop1UtilTb.Text) );

                UpdateHopAndFinalHopsVariables();
            }
            catch (FormatException)
            {
                MessageBox.Show("Util must be a numeric value");
                Hop1UtilTb.Text = "0";
            }
        }
        private void Hop1WeightTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Hop1WeightTb.Text == "0")
            {
                Hop1WeightTb.Text = "";
            }
        }
        private void Hop1WeightTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Hop1WeightTb.Text == "0")
            {
                Hop1WeightTb.Text = "";
            }
            try
            {
                hopOneWeight = RoundUpTo2SigFigs( Convert.ToDouble(Hop1WeightTb.Text) );

                UpdateHopAndFinalHopsVariables();
            }
            catch (FormatException)
            {
                MessageBox.Show("Weight must be a numeric value");
                Hop1WeightTb.Text = "0";
            }
        }

        private void Hop2Tb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Hop2Tb.Text == "")
            {
                Hop2AlphaTb.Text = "0";
                Hop2UtilTb.Text = "0";
                Hop2WeightTb.Text = "0";

                UpdateHopAndFinalHopsVariables();

                Hop2AlphaTb.Text = "";
                Hop2UtilTb.Text = "";
                Hop2WeightTb.Text = "";
                Hop2IbuLbl.Text = "";
            }
        }
        private void Hop2AlphaTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Hop2AlphaTb.Text == "0")
            {
                Hop2AlphaTb.Text = "";
            }
        }
        private void Hop2AlphaTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Hop2AlphaTb.Text == "0")
            {
                Hop2AlphaTb.Text = "";
            }
            try
            {
                hopTwoAlpha = RoundUpTo2SigFigs(Convert.ToDouble(Hop2AlphaTb.Text));

                UpdateHopAndFinalHopsVariables();
            }
            catch (FormatException)
            {
                MessageBox.Show("Alpha must be a numeric value");
                Hop2AlphaTb.Text = "0";
            }
        }
        private void Hop2UtilTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Hop2UtilTb.Text == "0")
            {
                Hop2UtilTb.Text = "";
            }
        }
        private void Hop2UtilTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Hop2UtilTb.Text == "0")
            {
                Hop2UtilTb.Text = "";
            }
            try
            {
                hopTwoUtil = RoundUpTo2SigFigs(Convert.ToDouble(Hop2UtilTb.Text));

                UpdateHopAndFinalHopsVariables();
            }
            catch (FormatException)
            {
                MessageBox.Show("Util must be a numeric value");
                Hop2UtilTb.Text = "0";
            }
        }
        private void Hop2WeightTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Hop2WeightTb.Text == "0")
            {
                Hop2WeightTb.Text = "";
            }
        }
        private void Hop2WeightTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Hop2WeightTb.Text == "0")
            {
                Hop2WeightTb.Text = "";
            }
            try
            {
                hopTwoWeight = RoundUpTo2SigFigs(Convert.ToDouble(Hop2WeightTb.Text));

                UpdateHopAndFinalHopsVariables();
            }
            catch (FormatException)
            {
                MessageBox.Show("Weight must be a numeric value");
                Hop2WeightTb.Text = "0";
            }
        }

        private void Hop3Tb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Hop3Tb.Text == "")
            {
                Hop3AlphaTb.Text = "0";
                Hop3UtilTb.Text = "0";
                Hop3WeightTb.Text = "0";

                UpdateHopAndFinalHopsVariables();

                Hop3AlphaTb.Text = "";
                Hop3UtilTb.Text = "";
                Hop3WeightTb.Text = "";
                Hop3IbuLbl.Text = "";
            }
        }
        private void Hop3AlphaTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Hop3AlphaTb.Text == "0")
            {
                Hop3AlphaTb.Text = "";
            }
        }
        private void Hop3AlphaTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Hop3AlphaTb.Text == "0")
            {
                Hop3AlphaTb.Text = "";
            }
            try
            {
                hopThreeAlpha = RoundUpTo2SigFigs(Convert.ToDouble(Hop3AlphaTb.Text));

                UpdateHopAndFinalHopsVariables();
            }
            catch (FormatException)
            {
                MessageBox.Show("Alpha must be a numeric value");
                Hop3AlphaTb.Text = "0";
            }
        }
        private void Hop3UtilTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Hop3UtilTb.Text == "0")
            {
                Hop3UtilTb.Text = "";
            }
        }
        private void Hop3UtilTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Hop3UtilTb.Text == "0")
            {
                Hop3UtilTb.Text = "";
            }
            try
            {
                hopThreeUtil = RoundUpTo2SigFigs(Convert.ToDouble(Hop3UtilTb.Text));

                UpdateHopAndFinalHopsVariables();
            }
            catch (FormatException)
            {
                MessageBox.Show("Util must be a numeric value");
                Hop3UtilTb.Text = "0";
            }
        }
        private void Hop3WeightTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Hop3WeightTb.Text == "0")
            {
                Hop3WeightTb.Text = "";
            }
        }
        private void Hop3WeightTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Hop3WeightTb.Text == "0")
            {
                Hop3WeightTb.Text = "";
            }
            try
            {
                hopThreeWeight = RoundUpTo2SigFigs(Convert.ToDouble(Hop3WeightTb.Text));

                UpdateHopAndFinalHopsVariables();
            }
            catch (FormatException)
            {
                MessageBox.Show("Weight must be a numeric value");
                Hop3WeightTb.Text = "0";
            }
        }

        private void Hop4Tb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Hop4Tb.Text == "")
            {
                Hop4AlphaTb.Text = "0";
                Hop4UtilTb.Text = "0";
                Hop4WeightTb.Text = "0";

                UpdateHopAndFinalHopsVariables();

                Hop4AlphaTb.Text = "";
                Hop4UtilTb.Text = "";
                Hop4WeightTb.Text = "";
                Hop4IbuLbl.Text = "";
            }
        }
        private void Hop4AlphaTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Hop4AlphaTb.Text == "0")
            {
                Hop4AlphaTb.Text = "";
            }
        }
        private void Hop4AlphaTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Hop4AlphaTb.Text == "0")
            {
                Hop4AlphaTb.Text = "";
            }
            try
            {
                hopFourAlpha = RoundUpTo2SigFigs(Convert.ToDouble(Hop4AlphaTb.Text));

                UpdateHopAndFinalHopsVariables();
            }
            catch (FormatException)
            {
                MessageBox.Show("Alpha must be a numeric value");
                Hop4AlphaTb.Text = "0";
            }
        }
        private void Hop4UtilTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Hop4UtilTb.Text == "0")
            {
                Hop4UtilTb.Text = "";
            }
        }
        private void Hop4UtilTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Hop4UtilTb.Text == "0")
            {
                Hop4UtilTb.Text = "";
            }
            try
            {
                hopFourUtil = RoundUpTo2SigFigs(Convert.ToDouble(Hop4UtilTb.Text));

                UpdateHopAndFinalHopsVariables();
            }
            catch (FormatException)
            {
                MessageBox.Show("Util must be a numeric value");
                Hop4UtilTb.Text = "0";
            }
        }
        private void Hop4WeightTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Hop4WeightTb.Text == "0")
            {
                Hop4WeightTb.Text = "";
            }
        }
        private void Hop4WeightTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Hop4WeightTb.Text == "0")
            {
                Hop4WeightTb.Text = "";
            }
            try
            {
                hopFourWeight = RoundUpTo2SigFigs(Convert.ToDouble(Hop4WeightTb.Text));

                UpdateHopAndFinalHopsVariables();
            }
            catch (FormatException)
            {
                MessageBox.Show("Weight must be a numeric value");
                Hop4WeightTb.Text = "0";
            }
        }

        private void Hop5Tb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Hop5Tb.Text == "")
            {
                Hop5AlphaTb.Text = "0";
                Hop5UtilTb.Text = "0";
                Hop5WeightTb.Text = "0";

                UpdateHopAndFinalHopsVariables();

                Hop5AlphaTb.Text = "";
                Hop5UtilTb.Text = "";
                Hop5WeightTb.Text = "";
                Hop5IbuLbl.Text = "";
            }
        }
        private void Hop5AlphaTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Hop5AlphaTb.Text == "0")
            {
                Hop5AlphaTb.Text = "";
            }
        }
        private void Hop5AlphaTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Hop5AlphaTb.Text == "0")
            {
                Hop5AlphaTb.Text = "";
            }
            try
            {
                hopFiveAlpha = RoundUpTo2SigFigs(Convert.ToDouble(Hop5AlphaTb.Text));

                UpdateHopAndFinalHopsVariables();
            }
            catch (FormatException)
            {
                MessageBox.Show("Alpha must be a numeric value");
                Hop5AlphaTb.Text = "0";
            }
        }
        private void Hop5UtilTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Hop5UtilTb.Text == "0")
            {
                Hop5UtilTb.Text = "";
            }
        }
        private void Hop5UtilTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Hop5UtilTb.Text == "0")
            {
                Hop5UtilTb.Text = "";
            }
            try
            {
                hopFiveUtil = RoundUpTo2SigFigs(Convert.ToDouble(Hop5UtilTb.Text));

                UpdateHopAndFinalHopsVariables();
            }
            catch (FormatException)
            {
                MessageBox.Show("Util must be a numeric value");
                Hop5UtilTb.Text = "0";
            }
        }
        private void Hop5WeightTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Hop5WeightTb.Text == "0")
            {
                Hop5WeightTb.Text = "";
            }
        }
        private void Hop5WeightTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Hop5WeightTb.Text == "0")
            {
                Hop5WeightTb.Text = "";
            }
            try
            {
                hopFiveWeight = RoundUpTo2SigFigs(Convert.ToDouble(Hop5WeightTb.Text));

                UpdateHopAndFinalHopsVariables();
            }
            catch (FormatException)
            {
                MessageBox.Show("Weight must be a numeric value");
                Hop5WeightTb.Text = "0";
            }
        }

        private void Hop6Tb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Hop6Tb.Text == "")
            {
                Hop6AlphaTb.Text = "0";
                Hop6UtilTb.Text = "0";
                Hop6WeightTb.Text = "0";

                UpdateHopAndFinalHopsVariables();

                Hop6AlphaTb.Text = "";
                Hop6UtilTb.Text = "";
                Hop6WeightTb.Text = "";
                Hop6IbuLbl.Text = "";
            }
        }
        private void Hop6AlphaTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Hop6AlphaTb.Text == "0")
            {
                Hop6AlphaTb.Text = "";
            }
        }
        private void Hop6AlphaTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Hop6AlphaTb.Text == "0")
            {
                Hop6AlphaTb.Text = "";
            }
            try
            {
                hopSixAlpha = RoundUpTo2SigFigs(Convert.ToDouble(Hop6AlphaTb.Text));

                UpdateHopAndFinalHopsVariables();
            }
            catch (FormatException)
            {
                MessageBox.Show("Alpha must be a numeric value");
                Hop6AlphaTb.Text = "0";
            }
        }
        private void Hop6UtilTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Hop6UtilTb.Text == "0")
            {
                Hop6UtilTb.Text = "";
            }
        }
        private void Hop6UtilTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Hop6UtilTb.Text == "0")
            {
                Hop6UtilTb.Text = "";
            }
            try
            {
                hopSixUtil = RoundUpTo2SigFigs(Convert.ToDouble(Hop6UtilTb.Text));

                UpdateHopAndFinalHopsVariables();
            }
            catch (FormatException)
            {
                MessageBox.Show("Util must be a numeric value");
                Hop6UtilTb.Text = "0";
            }
        }
        private void Hop6WeightTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Hop6WeightTb.Text == "0")
            {
                Hop6WeightTb.Text = "";
            }
        }
        private void Hop6WeightTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Hop6WeightTb.Text == "0")
            {
                Hop6WeightTb.Text = "";
            }
            try
            {
                hopSixWeight = RoundUpTo2SigFigs(Convert.ToDouble(Hop6WeightTb.Text));

                UpdateHopAndFinalHopsVariables();
            }
            catch (FormatException)
            {
                MessageBox.Show("Weight must be a numeric value");
                Hop6WeightTb.Text = "0";
            }
        }

        ///// --- HOPS  --- Custom calculation methods
        //
        public void UpdateHopAndFinalHopsVariables()
        {
            if ((gravityAtBoil != 0) && (litres != 0))
            {
                // Updates: All present Hops' Alpha, Util, Weight, IBU
                //          Weight & IBU totals

                if ((Hop1AlphaTb.Text != "") && (Hop1UtilTb.Text != "") && (Hop1WeightTb.Text != ""))
                {
                    hopOneAlpha = Convert.ToDouble(Hop1AlphaTb.Text);
                    hopOneUtil = Convert.ToDouble(Hop1UtilTb.Text);
                    hopOneWeight = Convert.ToDouble(Hop1WeightTb.Text);
                    hopOneIbu = GetIbu(hopOneAlpha, hopOneUtil, hopOneWeight);
                    Hop1IbuLbl.Text = hopOneIbu.ToString();
                }

                if ((Hop2AlphaTb.Text != "") && (Hop2UtilTb.Text != "") && (Hop2WeightTb.Text != ""))
                {
                    hopTwoAlpha = Convert.ToDouble(Hop2AlphaTb.Text);
                    hopTwoUtil = Convert.ToDouble(Hop2UtilTb.Text);
                    hopTwoWeight = Convert.ToDouble(Hop2WeightTb.Text);
                    hopTwoIbu = GetIbu(hopTwoAlpha, hopTwoUtil, hopTwoWeight);
                    Hop2IbuLbl.Text = hopTwoIbu.ToString();
                }

                if ((Hop3AlphaTb.Text != "") && (Hop3UtilTb.Text != "") && (Hop3WeightTb.Text != ""))
                {
                    hopThreeAlpha = Convert.ToDouble(Hop3AlphaTb.Text);
                    hopThreeUtil = Convert.ToDouble(Hop3UtilTb.Text);
                    hopThreeWeight = Convert.ToDouble(Hop3WeightTb.Text);
                    hopThreeIbu = GetIbu(hopThreeAlpha, hopThreeUtil, hopThreeWeight);
                    Hop3IbuLbl.Text = hopThreeIbu.ToString();
                }

                if ((Hop4AlphaTb.Text != "") && (Hop4UtilTb.Text != "") && (Hop4WeightTb.Text != ""))
                {
                    hopFourAlpha = Convert.ToDouble(Hop4AlphaTb.Text);
                    hopFourUtil = Convert.ToDouble(Hop4UtilTb.Text);
                    hopFourWeight = Convert.ToDouble(Hop4WeightTb.Text);
                    hopFourIbu = GetIbu(hopFourAlpha, hopFourUtil, hopFourWeight);
                    Hop4IbuLbl.Text = hopFourIbu.ToString();
                }

                if ((Hop5AlphaTb.Text != "") && (Hop5UtilTb.Text != "") && (Hop5WeightTb.Text != ""))
                {
                    hopFiveAlpha = Convert.ToDouble(Hop5AlphaTb.Text);
                    hopFiveUtil = Convert.ToDouble(Hop5UtilTb.Text);
                    hopFiveWeight = Convert.ToDouble(Hop5WeightTb.Text);
                    hopFiveIbu = GetIbu(hopFiveAlpha, hopFiveUtil, hopFiveWeight);
                    Hop5IbuLbl.Text = hopFiveIbu.ToString();
                }

                if ((Hop6AlphaTb.Text != "") && (Hop6UtilTb.Text != "") && (Hop6WeightTb.Text != ""))
                {
                    hopSixAlpha = Convert.ToDouble(Hop6AlphaTb.Text);
                    hopSixUtil = Convert.ToDouble(Hop6UtilTb.Text);
                    hopSixWeight = Convert.ToDouble(Hop6WeightTb.Text);
                    hopSixIbu = GetIbu(hopSixAlpha, hopSixUtil, hopSixWeight);
                    Hop6IbuLbl.Text = hopSixIbu.ToString();
                }

                UpdateAndDisplayFinalHopsVariables();
            }
            else
            {
                MessageBox.Show("Make sure you have input a 'Litres' value and have completed the previous pages");
            }
        }
        
            public double GetIbu(double inputAlpha, double inputUtil, double inputWeight)
            {
                double outputIbu = ((inputWeight * 1000 * (inputUtil / 100) * (inputAlpha / 100))
                                                             /
                                    (litres * (1 + (((gravityAtBoil / 1000) - 1.05) / 0.2))));

                return RoundUpTo1SigFig( outputIbu );
            }

            public void UpdateAndDisplayFinalHopsVariables()
            {
                CalculateTotalWeight();
                CalculateTotalIbu();

                TotalIbuTb.Text = totalHopIbu.ToString();
                TotalWeightTb.Text = totalHopWeight.ToString();
            }

                public void CalculateTotalWeight()
                {
                    totalHopWeight = RoundUpTo2SigFigs( hopOneWeight + hopTwoWeight + hopThreeWeight + hopFourWeight + hopFiveWeight + hopSixWeight );
                }
        
                public void CalculateTotalIbu()
                {
                    totalHopIbu = RoundUpTo2SigFigs( hopOneIbu + hopTwoIbu + hopThreeIbu + hopFourIbu + hopFiveIbu );
                }
    }
}