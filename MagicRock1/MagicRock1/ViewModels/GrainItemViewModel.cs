using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MagicRock1.ViewModels
{
    public class GrainItemViewModel : INotifyPropertyChanged
    {
        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private double _labExtract;

        public double LabExtract
        {
            get
            {
                return _labExtract;
            }
            set
            {
                if (value != _labExtract)
                {
                    _labExtract = value;
                    NotifyPropertyChanged("LabExtract");
                }
            }
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
