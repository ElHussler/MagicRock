using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagicRock1.Models;
using System.Windows;

namespace MagicRock1.ViewModels
{
    public class GrainsViewModel
    {
        public ObservableCollection<Grain> Grains { get; set; }

        public void GetGrains()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Count > 0)
            {
                GetSavedGrains();
            }
            else
            {
                GetDefaultGrains();
            }
        }

        public void GetSavedGrains()
        {
            ObservableCollection<Grain> a = new ObservableCollection<Grain>();

            foreach (Object o in IsolatedStorageSettings.ApplicationSettings.Values)
            {
                a.Add((Grain)o);
            }

            Grains = a;

            MessageBox.Show("Got grains from storage");
        }

        public void GetDefaultGrains()
        {
            ObservableCollection<Grain> a = new ObservableCollection<Grain>();

            // Items to collect
            a.Add(new Grain() { Name = "", LabExtract = 0 });
            a.Add(new Grain() { Name = "Sugar", LabExtract = 340.0 });
            a.Add(new Grain() { Name = "Golden Promise", LabExtract = 295.0 });

            Grains = a;

            MessageBox.Show("Got grains from default");
        }
    }
}
