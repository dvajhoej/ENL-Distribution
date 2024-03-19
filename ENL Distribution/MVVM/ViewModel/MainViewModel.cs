using ENL_Distribution.Core;
using System.Windows.Input;

namespace ENL_Distribution.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand ProdukterViewCommand { get; set; }
        public RelayCommand MedarbejdereViewCommand { get; set; }
        public RelayCommand OrdreViewCommand { get; set; }


        public HomeViewModel HomeVm { get; set; }
        public ProdukterViewModel ProdukterVm { get; set; }
        public MedarbejdereViewModel MedarbejdereVm { get; set; }
        public OrdreViewModel OrdreVm { get; set; }


        private object _currentview;

        public object Currentview
        {
            get { return _currentview; }
            set
            {
                _currentview = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            HomeVm = new HomeViewModel();
            ProdukterVm = new ProdukterViewModel();
            MedarbejdereVm = new MedarbejdereViewModel();
            OrdreVm = new OrdreViewModel();
            Currentview = HomeVm;

            HomeViewCommand = new RelayCommand(o =>
            {
                Currentview = HomeVm;
            });
            ProdukterViewCommand = new RelayCommand(o =>
            {
                Currentview = ProdukterVm;
            });
            MedarbejdereViewCommand = new RelayCommand(o =>
             {
                 Currentview = MedarbejdereVm;
             });
            OrdreViewCommand = new RelayCommand(o =>
            {
                Currentview = OrdreVm;
            });
        }

      

    }
}
