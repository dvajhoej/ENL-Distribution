using ENL_Distribution.Core;

namespace ENL_Distribution.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand DiscoveryViewCommand { get; set; }
        public RelayCommand MedarbejdereViewCommand { get; set; }



        public HomeViewModel HomeVm { get; set; }
        public DiscoveryViewModel DiscoveryVm { get; set; }
        public MedarbejdereViewModel MedarbejdereVm { get; set; }


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
            DiscoveryVm = new DiscoveryViewModel();
            MedarbejdereVm = new MedarbejdereViewModel();
            Currentview = HomeVm;

            HomeViewCommand = new RelayCommand(o =>
            {
                Currentview = HomeVm;
            });
            DiscoveryViewCommand = new RelayCommand(o =>
            {
                Currentview = DiscoveryVm;
            });
            MedarbejdereViewCommand = new RelayCommand(o =>
             {
                 Currentview = MedarbejdereVm;
             });
        }
    }
}
