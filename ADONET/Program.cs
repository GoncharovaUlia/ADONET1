// See https://aka.ms/new-console-template for more information
using System.Data;
Console.WriteLine("Hello, World!");

using (IDbConnection dbConnection = new System.Data.SQLite.SQLiteConnection("DataSource=D:\\ІТ. p21\\cafe.db"))
{
    dbConnection.Open();
    dbConnection.BeginTransaction(); //початок транзакцій
    var dbCommand = dbConnection.CreateCommand();
    dbCommand.CommandText = "select * from User";

    dbConnection.Close ();
}