using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;
using MagicRock1.Models;
using System.Windows;

namespace MagicRock1.ViewModels
{
    public class MaltViewModel
    {
        public ObservableCollection<Malt> Malts { get; set; }

        public void GetMalts()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Count > 0)
            {
                GetSavedMalts();
            }
            else
            {
                GetDefaultMalts();
            }
        }

        public void GetDefaultMalts()
        {
            ObservableCollection<Malt> a = new ObservableCollection<Malt>();

            a.Add(new Malt() { MaltName = "-", MaltLabExtract = 0 });
            a.Add(new Malt() { MaltName = "Sugar", MaltLabExtract = 340.0 });
            a.Add(new Malt() { MaltName = "Golden Promise", MaltLabExtract = 295.0 });

            Malts = a;

            MessageBox.Show("Got malts from default");
        }

        public void GetSavedMalts()
        {
            ObservableCollection<Malt> a = new ObservableCollection<Malt>();

            foreach (Object o in IsolatedStorageSettings.ApplicationSettings.Values)
            {
                a.Add((Malt)o);
            }

            Malts = a;

            MessageBox.Show("Got malts from storage");
        }
    }
}
