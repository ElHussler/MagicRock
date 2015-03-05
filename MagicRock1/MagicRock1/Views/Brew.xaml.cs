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