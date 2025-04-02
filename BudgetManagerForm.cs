using System.Data;
using System.Data.SQLite;

namespace FamilyBudgetManager
{
    public partial class BudgetManagerForm : Form
    {
        

        public BudgetManagerForm()
        {
            InitializeComponent();
            CreateDataBaseIfNotExists();
            LoadTransactions();
        }

        private void CreateDataBaseIfNotExists()
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

        private void InitializeComponent()
        {
            CategoryLabel = new Label();
            CategoryComboBox = new ComboBox();
            DescriptionLabel = new Label();
            DescriptionTextBox = new TextBox();
            AmountLabel = new Label();
            AmountTextBox = new TextBox();
            DateLabel = new Label();
            DatePicker = new DateTimePicker();
            dataGridView = new DataGridView();
            AddTransactionButton = new Button();
            RemoveTransactionButton = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            SuspendLayout();
            // 
            // CategoryLabel
            // 
            CategoryLabel.AutoSize = true;
            CategoryLabel.Location = new Point(25, 380);
            CategoryLabel.Name = "CategoryLabel";
            CategoryLabel.Size = new Size(55, 15);
            CategoryLabel.TabIndex = 0;
            CategoryLabel.Text = "Category";
            // 
            // CategoryComboBox
            // 
            CategoryComboBox.FormattingEnabled = true;
            CategoryComboBox.Items.AddRange(new object[] { "", "Income", "Expense" });
            CategoryComboBox.Location = new Point(97, 377);
            CategoryComboBox.Name = "CategoryComboBox";
            CategoryComboBox.Size = new Size(151, 23);
            CategoryComboBox.TabIndex = 1;
            // 
            // DescriptionLabel
            // 
            DescriptionLabel.AutoSize = true;
            DescriptionLabel.Location = new Point(25, 412);
            DescriptionLabel.Name = "DescriptionLabel";
            DescriptionLabel.Size = new Size(67, 15);
            DescriptionLabel.TabIndex = 2;
            DescriptionLabel.Text = "Description";
            // 
            // DescriptionTextBox
            // 
            DescriptionTextBox.Location = new Point(97, 409);
            DescriptionTextBox.Name = "DescriptionTextBox";
            DescriptionTextBox.Size = new Size(567, 23);
            DescriptionTextBox.TabIndex = 3;
            // 
            // AmountLabel
            // 
            AmountLabel.AutoSize = true;
            AmountLabel.Location = new Point(25, 441);
            AmountLabel.Name = "AmountLabel";
            AmountLabel.Size = new Size(51, 15);
            AmountLabel.TabIndex = 4;
            AmountLabel.Text = "Amount";
            // 
            // AmountTextBox
            // 
            AmountTextBox.Location = new Point(97, 438);
            AmountTextBox.Name = "AmountTextBox";
            AmountTextBox.Size = new Size(567, 23);
            AmountTextBox.TabIndex = 5;
            // 
            // DateLabel
            // 
            DateLabel.AutoSize = true;
            DateLabel.Location = new Point(25, 473);
            DateLabel.Name = "DateLabel";
            DateLabel.Size = new Size(31, 15);
            DateLabel.TabIndex = 6;
            DateLabel.Text = "Date";
            // 
            // DatePicker
            // 
            DatePicker.Location = new Point(97, 467);
            DatePicker.Name = "DatePicker";
            DatePicker.Size = new Size(200, 23);
            DatePicker.TabIndex = 7;
            // 
            // dataGridView
            // 
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Location = new Point(25, 12);
            dataGridView.Name = "dataGridView";
            dataGridView.RowTemplate.Height = 25;
            dataGridView.Size = new Size(639, 320);
            dataGridView.TabIndex = 8;
            // 
            // AddTransactionButton
            // 
            AddTransactionButton.Location = new Point(27, 502);
            AddTransactionButton.Name = "AddTransactionButton";
            AddTransactionButton.Size = new Size(113, 29);
            AddTransactionButton.TabIndex = 9;
            AddTransactionButton.Text = "Add Transaction";
            AddTransactionButton.UseVisualStyleBackColor = true;
            AddTransactionButton.Click += AddTransactionButton_Click;
            // 
            // RemoveTransactionButton
            // 
            RemoveTransactionButton.Location = new Point(146, 502);
            RemoveTransactionButton.Name = "RemoveTransactionButton";
            RemoveTransactionButton.Size = new Size(126, 29);
            RemoveTransactionButton.TabIndex = 10;
            RemoveTransactionButton.Text = "Remove Transaction";
            RemoveTransactionButton.UseVisualStyleBackColor = true;
            RemoveTransactionButton.Click += RemoveTransactionButton_Click;
            // 
            // BudgetManagerForm
            // 
            ClientSize = new Size(688, 543);
            Controls.Add(RemoveTransactionButton);
            Controls.Add(AddTransactionButton);
            Controls.Add(dataGridView);
            Controls.Add(DatePicker);
            Controls.Add(DateLabel);
            Controls.Add(AmountTextBox);
            Controls.Add(AmountLabel);
            Controls.Add(DescriptionTextBox);
            Controls.Add(DescriptionLabel);
            Controls.Add(CategoryComboBox);
            Controls.Add(CategoryLabel);
            Name = "BudgetManagerForm";
            Text = "Family Budget Manager";
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private void AddTransactionButton_Click(object sender, EventArgs e)
        {
            AddTransaction();
        }

        private void RemoveTransactionButton_Click(object sender, EventArgs e)
        {
            RemoveTransaction();
        }

        private void AddTransaction()
        {
            string category = CategoryComboBox.SelectedItem?.ToString();
            string description = DescriptionTextBox.Text.Trim();
            string amount = AmountTextBox.Text.Trim();
            DateTime date = DatePicker.Value;

            if (isValidInput(category!, description, amount))
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }
            using var connection = new SQLiteConnection(dbPath);
            connection.Open();
            string query = "INSERT INTO Transactions (category, description, amount, date) VALUES (@category, @description, @amount, @date);";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@category", category);
            command.Parameters.AddWithValue("@description", DescriptionTextBox.Text);
            command.Parameters.AddWithValue("@amount", AmountTextBox.Text);
            command.Parameters.AddWithValue("@date", DatePicker.Value.ToString("yyyy-MM-dd"));
            command.ExecuteNonQuery();
            LoadTransactions();
        }

        private bool isValidInput(string category, string description, string amount)
        {
            if(string.IsNullOrEmpty(category) || 
               string.IsNullOrEmpty(DescriptionTextBox.Text) || 
               string.IsNullOrEmpty(AmountTextBox.Text))
            {
                return false;
            }
            return true;
        }

        private void LoadTransactions()
        {
            using var connection = new SQLiteConnection(dbPath);
            connection.Open();

            using var adapter = new SQLiteDataAdapter("SELECT * FROM Transactions", connection);
            DataTable table = new DataTable();
            adapter.Fill(table);

            dataGridView.DataSource = table;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.RowHeadersVisible = false;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.MultiSelect = false;
        }

        private void RemoveTransaction()
        {
            if(dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to remove.");
                return;
            }

            DataGridViewRow selectedRow = dataGridView.SelectedRows[0];

            if (!dataGridView.Columns.Contains("id"))
            {
                MessageBox.Show("The table doesn't contain an 'id' column.");
                return;
            }

            object? idValue = selectedRow.Cells["id"].Value;
            if (idValue == null || !int.TryParse(idValue.ToString(), out int id))
            {
                MessageBox.Show("Invalid row selected.");
                return;
            }

            var confirm = MessageBox.Show(
                "Are you sure you want to remove this transaction?", 
                "Remove Transaction", 
                MessageBoxButtons.YesNo);
            if (confirm != DialogResult.Yes) return;         

            LoadTransactions();
        }
    }

    public interface IDataReader
    {
        void Read();
        void Write();
        void Delete(int id);
    }

    public class DatabaseReader : IDataReader
    {
        private string dbPath = "Data Source=budget.db;";

        public void Read()
        {
            // Read data from the database
        }

        public void Write()
        {
            // Write data to the database
        }

        public void Delete(int id)
        {
            using var connection = new SQLiteConnection(dbPath);
            connection.Open();

            string query = "DELETE FROM Transactions WHERE id = @id;";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }
}
