using System.Data;
using System.Data.SQLite;
using System.Linq;

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

        public void Write(string category, string description, string amount, DateTime date, string tableName)
        {
            using var connection = new SQLiteConnection(dbPath);
            connection.Open();
            string query = $@"INSERT INTO [{tableName}] (
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

        public void Write(string category, string description, string amount, DateTime date, string tableName,
                  SQLiteConnection connection, SQLiteTransaction transaction)
        {
            string query = $@"INSERT INTO [{tableName}] (
                        category, 
                        description, 
                        amount, 
                        date) 
                     VALUES (@category, @description, @amount, @date);";
            using var command = new SQLiteCommand(query, connection, transaction);
            command.Parameters.AddWithValue("@category", category);
            command.Parameters.AddWithValue("@description", description);
            command.Parameters.AddWithValue("@amount", amount);
            command.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
            command.ExecuteNonQuery();
        }

        public void Delete(int id, string tableName)
        {
            using var connection = new SQLiteConnection(dbPath);
            connection.Open();

            string query = $@"DELETE FROM [{tableName}] 
                             WHERE id = @id;";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        public void Delete(int id, string tableName, SQLiteConnection connection, SQLiteTransaction transaction)
        {
            string query = $@"DELETE FROM [{tableName}] WHERE id = @id;";
            using var command = new SQLiteCommand(query, connection, transaction);
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

        public double GetSumFromCategory(string typeOfTransaction, string tableName)
        {
            using var connection = new SQLiteConnection(dbPath);
            connection.Open();
            string query = $@"SELECT SUM(amount) 
                             FROM [{tableName}] 
                             WHERE category = @typeOfTransaction;";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@typeOfTransaction", typeOfTransaction);

            object result = command.ExecuteScalar();
            return result != DBNull.Value ? Convert.ToDouble(result) : 0.0;
        }

        public void Update(int id, string category, string description, string amount, DateTime date, string tableName)
        {
            using var connection = new SQLiteConnection(dbPath);
            connection.Open();
            string query = $@"UPDATE [{tableName}] 
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
                if (reader.GetString(0) != "sqlite_sequence")
                    tables.Add(reader.GetString(0));
            }
            return tables;
        }

        public void CreateNewTable(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException("Table name must not be empty.");

            if (!System.Text.RegularExpressions.Regex.IsMatch(tableName, @"^[A-Za-z0-9_ ]+$"))
                throw new ArgumentException("Table name contains invalid characters.");

            using var connection = new SQLiteConnection(dbPath);
            connection.Open();

            string query = $"CREATE TABLE IF NOT EXISTS [{tableName}] (" +
                           "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                           "category TEXT, " +
                           "description TEXT, " +
                           "amount REAL, " +
                           "date TEXT);";

            using var command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();
        }

        public void DeleteTable(string tableName)
        {
            using var connection = new SQLiteConnection(dbPath);
            connection.Open();

            string query = $"DROP TABLE IF EXISTS [{tableName}];";

            using var command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();
        }

        public void TransferRecord(int id, string sourceTable, string destinationTable)
        {
            using var connection = new SQLiteConnection(dbPath);
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                string selectSql = $"SELECT * FROM [{sourceTable}] WHERE id = @id";
                using var selectCmd = new SQLiteCommand(selectSql, connection, transaction);
                selectCmd.Parameters.AddWithValue("@id", id);

                using var reader = selectCmd.ExecuteReader();

                if (!reader.Read())
                    throw new Exception($"Record with id {id} not found in {sourceTable}.");

                string category = reader["category"].ToString();
                string description = reader["description"].ToString();
                string amount = reader["amount"].ToString();
                DateTime date = DateTime.Parse(reader["date"].ToString());

                reader.Close();

                Write(category, description, amount, date, destinationTable, connection, transaction);

                Delete(id, sourceTable, connection, transaction);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
