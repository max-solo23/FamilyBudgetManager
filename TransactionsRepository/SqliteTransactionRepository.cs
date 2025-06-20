using System.Data;
using System.Data.SQLite;

namespace FamilyBudgetManager.TransactionsRepository
{
    public class SqliteTransactionRepository : ITransactionRepository
    {
        private readonly string dbPath = "Data Source=budget.db;";

        public DataTable ReadAllTransactions(string tableName)
        {
            using var connection = new SQLiteConnection(dbPath);
            connection.Open();

            string query = $"SELECT * FROM [{tableName}]";
            using var adapter = new SQLiteDataAdapter(query, connection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        public void Write(string category, string description, string amount, DateTime date)
        {
            using var connection = new SQLiteConnection(dbPath);
            connection.Open();
            string query = @"INSERT INTO Transactions (
                                category, 
                                description, 
                                amount, 
                                date) 
                             VALUES (@category, @description, @amount, @date);";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@category", category);
            command.Parameters.AddWithValue("@description", description);
            command.Parameters.AddWithValue("@amount", amount);
            command.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
            command.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var connection = new SQLiteConnection(dbPath);
            connection.Open();

            string query = @"DELETE FROM Transactions 
                             WHERE id = @id;";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        public void CreateNewIfNotExists()
        {
            using var connection = new SQLiteConnection(dbPath);
            connection.Open();
            string query = @"CREATE TABLE IF NOT EXISTS Transactions (
                                id INTEGER PRIMARY KEY AUTOINCREMENT,
                                category TEXT NOT NULL CHECK (category IN ('Income', 'Expense')),
                                description TEXT NOT NULL,
                                amount REAL NOT NULL,
                                date TEXT NOT NULL
             );";
            using var command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();
        }

        public double GetSumFromCategory(string typeOfTransaction)
        {
            using var connection = new SQLiteConnection(dbPath);
            connection.Open();
            string query = @"SELECT SUM(amount) 
                             FROM Transactions 
                             WHERE category = @typeOfTransaction;";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@typeOfTransaction", typeOfTransaction);

            object result = command.ExecuteScalar();
            return result != DBNull.Value ? Convert.ToDouble(result) : 0.0;
        }

        public void Update(int id, string category, string description, string amount, DateTime date)
        {
            using var connection = new SQLiteConnection(dbPath);
            connection.Open();
            string query = @"UPDATE Transactions 
                             SET category = @category, 
                                 description = @description, 
                                 amount = @amount, 
                                 date = @date 
                             WHERE id = @id;";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@category", category);
            command.Parameters.AddWithValue("@description", description);
            command.Parameters.AddWithValue("@amount", amount);
            command.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        public List<string> GetAllTableNames()
        {
            var tables = new List<string>();
            using var connection = new SQLiteConnection(dbPath);
            connection.Open();
            string query = "SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;";
            using var command = new SQLiteCommand(query, connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                tables.Add(reader.GetString(0));
            }
            return tables;
        }
    }
}
