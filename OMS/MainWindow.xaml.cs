using OMS.ViewModels;
using System.Windows;

namespace OMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();


            try
            {
                using var context = new OMS.Data.OMSDbContext();
                var productCount = context.Products.Count();
                MessageBox.Show($"Connection successful! Found {productCount} products.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database connection failed: {ex.Message}");
            }
        }
    }
}