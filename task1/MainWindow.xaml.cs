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

            this.DataContext = users;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (IDbConnection dbConnection = new System.Data.SQLite.SQLiteConnection("DataSource=D:\\ІТ. p21\\cafe.db"))
            {
                dbConnection.Open();
                users.Clear();
                
                users.Add(new User { Id = 1, Name = "User", Password = "*****" });
                List_Box 
                dbConnection.BeginTransaction(); //початок транзакцій
                var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = "select * from Users";
                var dbReader = dbCommand.ExecuteReader();


                while(dbReader.Read())
                {
                    users.Add(new User (dbReader.GetInt32(0), dbReader.GetString(1), dbReader.GetString(2)));
                }
                dbConnection.Close();
            }

            
        }
    }
}
