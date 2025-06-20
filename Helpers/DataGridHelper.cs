using System.Data;

namespace FamilyBudgetManager.Helpers
{
    public static class DataGridHelper
    {
        public static void SetupDataGridView(DataGridView grid, DataTable table)
        {
            grid.DataSource = table;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.RowHeadersVisible = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.ReadOnly = true;
        }

        public static bool GetSelectedRowId(DataGridView grid, out int id)
        {
            id = -1;

            if (!IsRowSelected(grid))
            {
                MessageBox.Show("Please select a row.");
                return false;
            }

            DataGridViewRow selectedRow = grid.SelectedRows[0];

            if (!grid.Columns.Contains("id"))
            {
                MessageBox.Show("The table doesn't contain an 'id' column.");
                return false;
            }

            object? idValue = selectedRow.Cells["id"].Value;
            if (idValue == null || !int.TryParse(idValue.ToString(), out id))
            {
                MessageBox.Show("Invalid row selected.");
                return false;
            }

            return true;
        }

        public static bool IsRowSelected(DataGridView grid)
        {
            return grid.SelectedRows.Count != 0;
        }
    }
}
