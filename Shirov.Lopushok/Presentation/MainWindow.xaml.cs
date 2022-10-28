using Shirov.Lopushok.Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Shirov.Lopushok.Presentation
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        public MainWindow()
        {
            InitializeComponent();
            _mainWindowViewModel=(MainWindowViewModel)DataContext;

        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _mainWindowViewModel.Search((sender as TextBox).Text);
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _mainWindowViewModel.Sort((sender as ComboBox).SelectedIndex);
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _mainWindowViewModel.Filter((sender as ComboBox).SelectedIndex);
        }

    }
}
