using System.Data;

namespace FamilyBudgetManager.TransactionsRepository
{
    public interface ITransactionRepository
    {
        DataTable ReadAllTransactions(string tableName);
        void Write(string category, string description, string amount, DateTime date, string tableName);
        void Update(int id, string category, string description, string amount, DateTime date, string tableName);
        void Delete(int id, string tableName);
        void CreateDefaultTable();
        double GetSumFromCategory(string typeOfTransaction, string tableName);
        public List<string> GetAllTableNames();
        void TransferRecord(int id, string sourceTable, string destinationTable);
        void CreateNewTable(string tableName);
        void DeleteTable(string tableName);
    }
}
