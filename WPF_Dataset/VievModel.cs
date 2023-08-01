using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Collections.ObjectModel;
using PropertyChanged;
using System.Configuration;
using System.Windows.Input;

namespace WPF_Dataset
{
    [AddINotifyPropertyChangedInterface()]
    public class ViewModel
    {
        private readonly RelayCommand saveCommand;
        public ICommand SaveCommand => saveCommand;
        private readonly RelayCommand refreshCommand;
        public ICommand RefreshCommand => refreshCommand;
        private readonly RelayCommand filterCommand;
        public ICommand FilterCommand => filterCommand;
        private readonly RelayCommand deleteCommand;
        public ICommand DeleteCommand => deleteCommand;
        private string connStr = null;
        private SqlDataAdapter adapter = null;
        private DataSet dataSet = null;
        public DataView DataRows { get; set; } 
        private List<Role> roles { get; set; }
        public IEnumerable<Role> Roles => roles;
        public ViewModel()
        {
            saveCommand = new RelayCommand((o) => ClickSave());
            refreshCommand = new RelayCommand((o) => Refresh());
            filterCommand = new RelayCommand((o) => SetFilterByRole(int.Parse(o.ToString())));
            deleteCommand = new RelayCommand((o) => ExecuteProcedure(o.ToString()));
            connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            LoadTable();
            LoadRoles();
            dataSet.Tables[0].RowChanged += OnRowChanged;
            
        }
        private void LoadRoles()
        {
            roles = new List<Role>();
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                connection.Open();
                string cmd = "SELECT ID, [Role name] FROM Roles";
                using (SqlCommand command = new SqlCommand(cmd, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int roleId = reader.GetInt32(0);
                            string roleName = reader.GetString(1);
                            roles.Add(new Role { RoleID = roleId, RoleName = roleName });
                        }
                    }
                }
            }
        }

        private void OnRowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (e.Action == DataRowAction.Add)
            {
                DataRow addedRow = e.Row;
            }
            else if (e.Action == DataRowAction.Change)
            {
                DataRow changedRow = e.Row;
                int? str = e.Row["RoleID"] as int?;
                MessageBox.Show($"Рядок було змінено. {str}");
            }
            else if (e.Action == DataRowAction.Delete)
            {
                DataRow deletedRow = e.Row;
            }
        }
        //---------------------------------------------
        private void LoadTable()
        {
            string cmd = "select * from Users";
            adapter = new(cmd, connStr);
            new SqlCommandBuilder(adapter);
            adapter.TableMappings.Add("Users", "Users");
            dataSet = new DataSet();
            adapter.Fill(dataSet, "Users");

            DataRows = dataSet.Tables[0].DefaultView;
        }
        private void ClickSave()
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
        private void Refresh()
        {
            dataSet = new DataSet();
            adapter.Fill(dataSet, "Users");
            DataRows = dataSet.Tables[0].DefaultView;
        }

        //Local filter by roles
        private void SetFilterByRole(int roleID)
        {
            string filterExpression = $"RoleID = {roleID}";
            DataRows.RowFilter = filterExpression;
        }
        private void ClickFilterByAdmins(object sender, RoutedEventArgs e) => SetFilterByRole(1);
        private void ClickFilterByModerators(object sender, RoutedEventArgs e) => SetFilterByRole(2);
        private void ClickFilterByUsers(object sender, RoutedEventArgs e) => SetFilterByRole(3);

        //Procedure calls
        private void ExecuteProcedure(string sp_name)
        {
            SqlDataAdapter ad = new($"sp_delete_{sp_name}", connStr);
            ad.SelectCommand.CommandType = CommandType.StoredProcedure;
            new SqlCommandBuilder(ad);
            DataTable dataTable = new();
            ad.Fill(dataTable);
            Refresh();
        }
        private void ClickDeleteAdmins(object sender, RoutedEventArgs e) => ExecuteProcedure("sp_delete_admins");
        private void ClickDeleteModerators(object sender, RoutedEventArgs e) => ExecuteProcedure("sp_delete_moderators");
        private void ClickDeleteUsers(object sender, RoutedEventArgs e) => ExecuteProcedure("sp_delete_users");
    }
}
