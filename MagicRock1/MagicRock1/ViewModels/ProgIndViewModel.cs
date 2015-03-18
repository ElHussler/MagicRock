using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MagicRock1.ViewModels
{
    public class ProgIndViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        /*private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }*/

        private ProgressIndicator progressIndicator;

        public ProgressIndicator ProgressIndicator
        {
            get
            {
                return this.progressIndicator;
            }
            set
            {
                if (value == this.progressIndicator)
                {
                    return;
                }

                this.progressIndicator = value;
                this.OnPropertyChanged();
            }
        }
    }

}
