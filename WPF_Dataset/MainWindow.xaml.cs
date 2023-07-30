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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Collections.ObjectModel;
using PropertyChanged;

namespace WPF_Dataset
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel viewModel = null;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new();
        }
    }
}
