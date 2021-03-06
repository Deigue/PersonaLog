using Caliburn.Micro;
using MahApps.Metro.Controls;
using System.ComponentModel;
using System.ComponentModel.Composition;
using Metro.Dialogs;

namespace PersonaLog.ViewModels {
    [Export(typeof(IShell))]
    public class ShellViewModel : INotifyPropertyChanged, IShell
    {
        //Private VM variables.
        private readonly IWindowsDialogs _windowDialogs;

        [ImportingConstructor]
        public ShellViewModel(IWindowsDialogs windowDialogs)
        {
            //Set Metro window dialogs.
            _windowDialogs = windowDialogs;

            //Default watermark message for file path.
            //TODO: Make this read-only. Pretty format with a label. Make a model to store file information.
            Watermark = "<Chosen file path>";
        }

        private string _watermark;

        public string Watermark
        {
            get { return _watermark; }
            set
            {
                _watermark = value;
                NotifyPropertyChanged("Watermark");
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}