namespace FamilyBudgetManager.Helpers
{
    public static class Validator
    {
        public static bool IsValidInput(string category, string description, string amount)
        {
            if (string.IsNullOrEmpty(category) ||
               string.IsNullOrEmpty(description) ||
               string.IsNullOrEmpty(amount))
            {
                return false;
            }
            return true;
        }
    }
}
