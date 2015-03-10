using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using MagicRock1.Models;

namespace MagicRock1.ViewModels
{
    public class GrainsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Grain> Grains { get; set; }

        public GrainsViewModel()
        {
            this.Grains = new ObservableCollection<Grain>();
        }

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

        public void GetDefaultGrains()
        {
            ObservableCollection<Grain> a = new ObservableCollection<Grain>();

            a.Add(new Grain() { GrainName = "-", LabExtract = 0 });
            a.Add(new Grain() { GrainName = "Sugar", LabExtract = 340.0 });
            a.Add(new Grain() { GrainName = "Golden Promise", LabExtract = 295.0 });

            Grains = a;

            MessageBox.Show("Got grains from default");
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
