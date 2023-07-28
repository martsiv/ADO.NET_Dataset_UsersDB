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

namespace WPF_Dataset
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string connStr = null;
        private SqlDataAdapter adapter = null;
        private DataSet dataSet = null;
        private DataTable mixedTable = null;
        
        public MainWindow()
        {
            InitializeComponent();
            connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            LoadTable();
        }
        private void LoadTable()
        {
            string cmd = "select * from Users";
            adapter = new(cmd, connStr);
            new SqlCommandBuilder(adapter);
            adapter.TableMappings.Add("Users", "Users");
            dataSet = new DataSet();
            adapter.Fill(dataSet, "Users");
            grid.ItemsSource = dataSet.Tables[0].DefaultView;
        }
        private void ClickSave(object sender, RoutedEventArgs e)
        {
            try
            {
                adapter.Update(dataSet, "Users");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ClickRefresh(object sender, RoutedEventArgs e)
        {
            dataSet = new DataSet();
            adapter.Fill(dataSet, "Users");
            grid.ItemsSource = dataSet.Tables[0].DefaultView;
        }

        //Local filter by roles
        private void SetFilterByRole(int roleID)
        {
            DataViewManager dvm = new DataViewManager(dataSet);
            dvm.DataViewSettings["Users"].RowFilter = $"RoleID = {roleID}";
            DataView dataView1 = dvm.CreateDataView(dataSet.Tables["Users"]);

            grid.ItemsSource = null;
            grid.ItemsSource = dataView1;
        }
        private void ClickFilterByAdmins(object sender, RoutedEventArgs e) => SetFilterByRole(1);
        private void ClickFilterByModerators(object sender, RoutedEventArgs e) => SetFilterByRole(2);
        private void ClickFilterByUsers(object sender, RoutedEventArgs e) => SetFilterByRole(3);

        //Procedure calls
        private void ExecuteProcedure(string sp_name)
        {
            SqlDataAdapter ad = new(sp_name, connStr);
            ad.SelectCommand.CommandType = CommandType.StoredProcedure;
            //arguments possible e.g.
            //adapt.SelectCommand.Parameters.Add(new SqlParameter("@ChatRoomID", SqlDbType.VarChar, 100));
            //adapt.SelectCommand.Parameters["@ChatRoomID"].Value = chatroomidno;
            new SqlCommandBuilder(ad);
            DataTable dataTable = new();
            ad.Fill(dataTable);
        }
        private void ClickDeleteAdmins(object sender, RoutedEventArgs e) => ExecuteProcedure("sp_delete_admins");
        private void ClickDeleteModerators(object sender, RoutedEventArgs e) => ExecuteProcedure("sp_delete_moderators");
        private void ClickDeleteUsers(object sender, RoutedEventArgs e) => ExecuteProcedure("sp_delete_users");
    }
}
