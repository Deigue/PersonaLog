using Caliburn.Micro;
using MahApps.Metro.Controls;

namespace PersonaLog.Views
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ShellView : PropertyChangedBase
    {
        public ShellView()
        {
            InitializeComponent();
        }

        private string _watermark;

        public string Watermark
        {
            get { return _watermark; }
            set {
                _watermark = value;
                NotifyOfPropertyChange(() => Watermark);
            }
        }
    }
}
