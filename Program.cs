using FamilyBudgetManager.TransactionsRepository;

namespace FamilyBudgetManager
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new BudgetManagerForm(
                new SqliteTransactionRepository()));
        }
    }
}