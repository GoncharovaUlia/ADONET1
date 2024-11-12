using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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

namespace task1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<User> users { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            users = new ObservableCollection<User>();
            this.DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (IDbConnection dbConnection = new System.Data.SQLite.SQLiteConnection("DataSource=D:\\ІТ. p21\\cafe.db"))
            {
                dbConnection.Open();
                users.Clear();
                dg.ItemsSource = users;
                users.Add(new User { Id = 1, Name = "User", Password = "*****" });
                dbConnection.BeginTransaction(); //початок транзакцій
                var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = "select * from Users";
                var dbReader = dbCommand.ExecuteReader();
                while (dbReader.Read())
                {
                    users.Add(new User(dbReader.GetInt32(0), dbReader.GetString(1), dbReader.GetString(2)));
                }
                dbConnection.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (dg.SelectedItem is User SelectedId)
            {
                int selectedUserId = SelectedId.Id;

                using (IDbConnection dbConnection = new System.Data.SQLite.SQLiteConnection("DataSource=D:\\ІТ. p21\\cafe.db"))
                {
                    dbConnection.Open();
                    var deleteCommand = dbConnection.CreateCommand();
                    deleteCommand.CommandText = $"DELETE FROM Users WHERE Id = {selectedUserId}";
                    deleteCommand.ExecuteNonQuery();
                    dbConnection.Close();
                }

                users.Remove(SelectedId);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NameforAdd.Text) && !string.IsNullOrWhiteSpace(Password.Text))
            {
                using (IDbConnection dbConnection = new System.Data.SQLite.SQLiteConnection("DataSource=D:\\ІТ. p21\\cafe.db"))
                {
                    dbConnection.Open();
                    var insertCommand = dbConnection.CreateCommand();
                    insertCommand.CommandText = $"INSERT INTO Users (Name, Password) VALUES ('{NameforAdd.Text}', '{Password.Text}')";
                    insertCommand.ExecuteNonQuery();
                    dbConnection.Close();
                }
                users.Add(new User { Name=NameforAdd.Text,Password= Password.Text });
            }
            NameforAdd.Clear();
            Password.Clear();
        }

        private void dg_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            if (e.Row.Item is User user)
            {
                var edit = e.Column as DataGridBoundColumn;
                var Path = (edit.Binding as Binding)?.Path.Path;
                var Element = e.EditingElement as TextBox;

                if (Path == "Name") { user.Name = Element.Text; }
                else if (Path == "Password") { user.Password = Element.Text; }
                using (IDbConnection dbConnection = new System.Data.SQLite.SQLiteConnection("DataSource=D:\\ІТ. p21\\cafe.db"))
                {
                    dbConnection.Open();
                    var updateCommand = dbConnection.CreateCommand();
                    updateCommand.CommandText = $"UPDATE Users SET Name = '{user.Name}', Password = '{user.Password}' WHERE Id = {user.Id}";
                    updateCommand.ExecuteNonQuery();
                    dbConnection.Close();
                }
            }
        }
    }
}
