using System.Windows;
using Diplom.DataBase;
using Diplom.ViewModel;

namespace Diplom
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ElasticSearchDataModel _elasticSearchDataModel;

        public MainWindow()
        {
            _elasticSearchDataModel = new ElasticSearchDataModel();

            var view = new MainViewModel();
            DataContext = view;
            InitializeComponent();

        }
    }
}
