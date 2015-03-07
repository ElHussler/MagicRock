using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicRock1.Models
{
    public class Grain : INotifyPropertyChanged
    {
        public string Name { get; set; }

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
