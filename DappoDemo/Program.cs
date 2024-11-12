// See https://aka.ms/new-console-template for more information
using Dapper;
using System.Data;
using System.Data.SQLite;

Console.WriteLine("Hello, World!");

using (IDbConnection dbConnection = new SQLiteConnection("DataSource=D:\\ІТ. p21\\cafe.db"))
{
    dbConnection.Open();

    int id = 10;
    var result = dbConnection.Query<Waiter>("SELECT * FROM Waiters WHERE id = @id");
    foreach (Waiter waiter in result)
    {
        Console.WriteLine($"Waiter ID: {waiter.Id}, Name: {waiter.Name}");
    }

    dbConnection.Execute("DELETE FROM Waiters WHERE id = @id");
    Console.WriteLine($"Deleted waiter with ID {id}");

    string newName = "Updated Waiter Name";
    dbConnection.Execute("UPDATE Waiters WHERE id = @id", new { newName, id });
    Console.WriteLine($"Updated waiter with ID {id} to name {newName}");

     dbConnection.BeginTransaction();
    var dbCommand = dbConnection.CreateCommand();
    dbCommand.CommandText = "SELECT * FROM Waiters";
    var dbReader = dbCommand.ExecuteReader();
    while (dbReader.Read())
    {
        Console.WriteLine($"Waiter ID: {dbReader}, Name: {dbReader}");
    }

    dbConnection.Close();
}




public class Waiter
{
    public int Id { get; set; }
    public string Name { get; set; }
}
