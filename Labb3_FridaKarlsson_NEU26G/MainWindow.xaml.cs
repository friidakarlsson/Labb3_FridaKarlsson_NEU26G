
using System.Windows;
using Labb3_FridaKarlsson_NEU26G.ViewModels;

namespace Labb3_FridaKarlsson_NEU26G
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();
        }
    }
}