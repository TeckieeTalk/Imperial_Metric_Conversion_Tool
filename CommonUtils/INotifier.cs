using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ImperialMetricConversion.CommonUtils
{
    public class INotifier : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void SetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = null)
        {
            member = val;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void NotifyProperty([CallerMemberName] string ObjectName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(ObjectName));
            }
        }
    }
}
