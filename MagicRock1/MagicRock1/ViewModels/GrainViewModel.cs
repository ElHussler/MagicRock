using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MagicRock1.Resources;

namespace MagicRock1.ViewModels
{
    public class GrainViewModel : INotifyPropertyChanged
    {
        public GrainViewModel()
        {
            this.Items = new ObservableCollection<GrainItemViewModel>();
        }

        public ObservableCollection<GrainItemViewModel> Items { get; private set; }

        /*private string _sampleProperty = "Sample Runtime Property Value";

        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        public string LocalizedSampleProperty
        {
            get
            {
                return AppResources.SampleProperty;
            }
        }*/

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        public void LoadData()
        {
            this.Items.Add(new GrainItemViewModel() { Name = "Sugar", LabExtract = 340.0 });

            this.Items.Add(new GrainItemViewModel() { Name = "Low Colour Maris Otter", LabExtract = 291.0 });
            this.Items.Add(new GrainItemViewModel() { Name = "Golden Promise", LabExtract = 295.0 });
            this.Items.Add(new GrainItemViewModel() { Name = "Wheat malt", LabExtract = 291.0 });
            this.Items.Add(new GrainItemViewModel() { Name = "Vienna", LabExtract = 289.0 });
            this.Items.Add(new GrainItemViewModel() { Name = "Munich", LabExtract = 285.0 });
            this.Items.Add(new GrainItemViewModel() { Name = "Pilsner", LabExtract = 298.0 });
            this.Items.Add(new GrainItemViewModel() { Name = "Acidulated malt", LabExtract = 0.0 });
            this.Items.Add(new GrainItemViewModel() { Name = "Rye malt", LabExtract = 260.0 });
            this.Items.Add(new GrainItemViewModel() { Name = "Smoked malt", LabExtract = 295.0 });
            this.Items.Add(new GrainItemViewModel() { Name = "Oat Malt", LabExtract = 267.4 });
            this.Items.Add(new GrainItemViewModel() { Name = "Carapils", LabExtract = 285.0 });
            this.Items.Add(new GrainItemViewModel() { Name = "Carared", LabExtract = 268.0 });
            this.Items.Add(new GrainItemViewModel() { Name = "Caramalt", LabExtract = 268.4 });
            this.Items.Add(new GrainItemViewModel() { Name = "Melanoiden malt", LabExtract = 287.0 });
            this.Items.Add(new GrainItemViewModel() { Name = "Crystal", LabExtract = 260.8 });
            this.Items.Add(new GrainItemViewModel() { Name = "Caramunich II", LabExtract = 285.0 });
            this.Items.Add(new GrainItemViewModel() { Name = "Dark Crystal", LabExtract = 270.8 });
            this.Items.Add(new GrainItemViewModel() { Name = "Cara Aroma", LabExtract = 266.0 });
            this.Items.Add(new GrainItemViewModel() { Name = "Amber", LabExtract = 266.1 });
            this.Items.Add(new GrainItemViewModel() { Name = "Special B", LabExtract = 266.0 });
            this.Items.Add(new GrainItemViewModel() { Name = "Brown", LabExtract = 270.0 });
            this.Items.Add(new GrainItemViewModel() { Name = "Chocolate", LabExtract = 267.2 });
            this.Items.Add(new GrainItemViewModel() { Name = "Roast Barley", LabExtract = 264.0 });
            this.Items.Add(new GrainItemViewModel() { Name = "Black", LabExtract = 265.7 });
            this.Items.Add(new GrainItemViewModel() { Name = "Carafa Special III", LabExtract = 250.0 });

            this.IsDataLoaded = true;
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
