using System.Data;
using System.Drawing.Printing;
using System.Windows.Forms;
using FamilyBudgetManager.Helpers;
using FamilyBudgetManager.TransactionsRepository;

namespace FamilyBudgetManager
{
    public partial class BudgetManagerForm : Form
    {
        private readonly ITransactionRepository _repository;
        private PrintDocument printDocument = new PrintDocument();
        private DataTable printTable;
        private string currentTableName;
        public BudgetManagerForm(ITransactionRepository repository)
        {
            _repository = repository;
            InitializeComponent();
            if (!AnyTableNames())
            {
                CreateDefaultTable();
            }
            LoadTableNames();
            UpdateTargetTableSelectorComboBox();
            DisplayTransactionsInDataTable();
            UpdateViewLabels();
            dataGridView.SelectionChanged += DataGridView_SelectionChanged;
            AmountTextBox.KeyPress += AmountTextBox_OnlyPositiveInteger_KeyPress;
            printDocument.PrintPage += PrintDocument_PrintPage;
            TableSelectorComboBox.SelectedIndexChanged += TableSelectorComboBox_SelectedIndexChanged;
        }

        private bool AnyTableNames()
        {
            var tables = _repository.GetAllTableNames();

            if (tables == null || tables.Count == 0)
            {
                MessageBox.Show("No tables found in the database.");
                TableSelectorComboBox.Items.Clear();
                return false;
            }

            return true;
        }

        private void TableSelectorComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            currentTableName = TableSelectorComboBox.SelectedItem?.ToString() ?? "";

            DisplayTransactionsInDataTable();
            UpdateViewLabels();

            UpdateTargetTableSelectorComboBox();
        }

        private void AmountTextBox_OnlyPositiveInteger_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void DataGridView_SelectionChanged(object? sender, EventArgs e)
        {
            if (!DataGridHelper.IsRowSelected(dataGridView)) return;

            var row = dataGridView.SelectedRows[0];

            CategoryComboBox.SelectedItem = row.Cells["category"].Value.ToString();
            DescriptionTextBox.Text = row.Cells["description"].Value.ToString();
            AmountTextBox.Text = row.Cells["amount"].Value.ToString();
            DatePicker.Value = DateTime.TryParse(row.Cells["date"].Value.ToString(), out DateTime date)
                ? date
                : DateTime.Today;
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
            ExpensesSumTextLabel = new Label();
            ExpensesSumViewLabel = new Label();
            IncomesSumViewLabel = new Label();
            IncomesSumTextLabel = new Label();
            EstimatedAvailabilityViewLabel = new Label();
            EstimatedAvailabilityTextLabel = new Label();
            PrintButton = new Button();
            UpdateTransactionButton = new Button();
            TableSelectorComboBox = new ComboBox();
            CreateNewTableButton = new Button();
            NewTableNameInput = new TextBox();
            TargetTableSelectorComboBox = new ComboBox();
            TransferRecordButton = new Button();
            TransferArrowLabel = new Label();
            this.DeleteTableButton = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            SuspendLayout();
            // 
            // CategoryLabel
            // 
            CategoryLabel.AutoSize = true;
            CategoryLabel.Location = new Point(32, 438);
            CategoryLabel.Name = "CategoryLabel";
            CategoryLabel.Size = new Size(55, 15);
            CategoryLabel.TabIndex = 0;
            CategoryLabel.Text = "Category";
            // 
            // CategoryComboBox
            // 
            CategoryComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            CategoryComboBox.FormattingEnabled = true;
            CategoryComboBox.Items.AddRange(new object[] { "", "Income", "Expense" });
            CategoryComboBox.Location = new Point(104, 435);
            CategoryComboBox.Name = "CategoryComboBox";
            CategoryComboBox.Size = new Size(151, 23);
            CategoryComboBox.TabIndex = 1;
            // 
            // DescriptionLabel
            // 
            DescriptionLabel.AutoSize = true;
            DescriptionLabel.Location = new Point(32, 470);
            DescriptionLabel.Name = "DescriptionLabel";
            DescriptionLabel.Size = new Size(67, 15);
            DescriptionLabel.TabIndex = 2;
            DescriptionLabel.Text = "Description";
            // 
            // DescriptionTextBox
            // 
            DescriptionTextBox.Location = new Point(104, 467);
            DescriptionTextBox.Name = "DescriptionTextBox";
            DescriptionTextBox.Size = new Size(567, 23);
            DescriptionTextBox.TabIndex = 3;
            // 
            // AmountLabel
            // 
            AmountLabel.AutoSize = true;
            AmountLabel.Location = new Point(32, 499);
            AmountLabel.Name = "AmountLabel";
            AmountLabel.Size = new Size(51, 15);
            AmountLabel.TabIndex = 4;
            AmountLabel.Text = "Amount";
            // 
            // AmountTextBox
            // 
            AmountTextBox.Location = new Point(104, 496);
            AmountTextBox.Name = "AmountTextBox";
            AmountTextBox.Size = new Size(567, 23);
            AmountTextBox.TabIndex = 5;
            // 
            // DateLabel
            // 
            DateLabel.AutoSize = true;
            DateLabel.Location = new Point(32, 531);
            DateLabel.Name = "DateLabel";
            DateLabel.Size = new Size(31, 15);
            DateLabel.TabIndex = 6;
            DateLabel.Text = "Date";
            // 
            // DatePicker
            // 
            DatePicker.Location = new Point(104, 525);
            DatePicker.Name = "DatePicker";
            DatePicker.Size = new Size(200, 23);
            DatePicker.TabIndex = 7;
            // 
            // dataGridView
            // 
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Location = new Point(32, 70);
            dataGridView.Name = "dataGridView";
            dataGridView.RowTemplate.Height = 25;
            dataGridView.Size = new Size(639, 320);
            dataGridView.TabIndex = 8;
            // 
            // AddTransactionButton
            // 
            AddTransactionButton.Location = new Point(34, 560);
            AddTransactionButton.Name = "AddTransactionButton";
            AddTransactionButton.Size = new Size(113, 29);
            AddTransactionButton.TabIndex = 9;
            AddTransactionButton.Text = "Add Transaction";
            AddTransactionButton.UseVisualStyleBackColor = true;
            AddTransactionButton.Click += AddTransactionButton_Click;
            // 
            // RemoveTransactionButton
            // 
            RemoveTransactionButton.Location = new Point(153, 560);
            RemoveTransactionButton.Name = "RemoveTransactionButton";
            RemoveTransactionButton.Size = new Size(126, 29);
            RemoveTransactionButton.TabIndex = 10;
            RemoveTransactionButton.Text = "Remove Transaction";
            RemoveTransactionButton.UseVisualStyleBackColor = true;
            RemoveTransactionButton.Click += RemoveTransactionButton_Click;
            // 
            // ExpensesSumTextLabel
            // 
            ExpensesSumTextLabel.AutoSize = true;
            ExpensesSumTextLabel.Location = new Point(677, 70);
            ExpensesSumTextLabel.Name = "ExpensesSumTextLabel";
            ExpensesSumTextLabel.Size = new Size(58, 15);
            ExpensesSumTextLabel.TabIndex = 11;
            ExpensesSumTextLabel.Text = "Expenses:";
            // 
            // ExpensesSumViewLabel
            // 
            ExpensesSumViewLabel.AutoSize = true;
            ExpensesSumViewLabel.Location = new Point(812, 70);
            ExpensesSumViewLabel.Name = "ExpensesSumViewLabel";
            ExpensesSumViewLabel.Size = new Size(31, 15);
            ExpensesSumViewLabel.TabIndex = 12;
            ExpensesSumViewLabel.Text = "0000";
            // 
            // IncomesSumViewLabel
            // 
            IncomesSumViewLabel.AutoSize = true;
            IncomesSumViewLabel.Location = new Point(812, 85);
            IncomesSumViewLabel.Name = "IncomesSumViewLabel";
            IncomesSumViewLabel.Size = new Size(31, 15);
            IncomesSumViewLabel.TabIndex = 14;
            IncomesSumViewLabel.Text = "0000";
            // 
            // IncomesSumTextLabel
            // 
            IncomesSumTextLabel.AutoSize = true;
            IncomesSumTextLabel.Location = new Point(677, 85);
            IncomesSumTextLabel.Name = "IncomesSumTextLabel";
            IncomesSumTextLabel.Size = new Size(55, 15);
            IncomesSumTextLabel.TabIndex = 13;
            IncomesSumTextLabel.Text = "Incomes:";
            // 
            // EstimatedAvailabilityViewLabel
            // 
            EstimatedAvailabilityViewLabel.AutoSize = true;
            EstimatedAvailabilityViewLabel.Location = new Point(812, 123);
            EstimatedAvailabilityViewLabel.Name = "EstimatedAvailabilityViewLabel";
            EstimatedAvailabilityViewLabel.Size = new Size(31, 15);
            EstimatedAvailabilityViewLabel.TabIndex = 16;
            EstimatedAvailabilityViewLabel.Text = "0000";
            // 
            // EstimatedAvailabilityTextLabel
            // 
            EstimatedAvailabilityTextLabel.AutoSize = true;
            EstimatedAvailabilityTextLabel.Location = new Point(677, 123);
            EstimatedAvailabilityTextLabel.Name = "EstimatedAvailabilityTextLabel";
            EstimatedAvailabilityTextLabel.Size = new Size(121, 15);
            EstimatedAvailabilityTextLabel.TabIndex = 15;
            EstimatedAvailabilityTextLabel.Text = "Estimated availability:";
            // 
            // PrintButton
            // 
            PrintButton.Location = new Point(545, 560);
            PrintButton.Name = "PrintButton";
            PrintButton.Size = new Size(126, 29);
            PrintButton.TabIndex = 17;
            PrintButton.Text = "Print Table";
            PrintButton.UseVisualStyleBackColor = true;
            PrintButton.Click += PrintButton_Click;
            // 
            // UpdateTransactionButton
            // 
            UpdateTransactionButton.Location = new Point(285, 560);
            UpdateTransactionButton.Name = "UpdateTransactionButton";
            UpdateTransactionButton.Size = new Size(126, 29);
            UpdateTransactionButton.TabIndex = 18;
            UpdateTransactionButton.Text = "Update";
            UpdateTransactionButton.UseVisualStyleBackColor = true;
            UpdateTransactionButton.Click += UpdateTransactionButton_Click;
            // 
            // TableSelectorComboBox
            // 
            TableSelectorComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            TableSelectorComboBox.FormattingEnabled = true;
            TableSelectorComboBox.Location = new Point(34, 12);
            TableSelectorComboBox.Name = "TableSelectorComboBox";
            TableSelectorComboBox.Size = new Size(121, 23);
            TableSelectorComboBox.TabIndex = 19;
            // 
            // CreateNewTableButton
            // 
            CreateNewTableButton.Location = new Point(558, 8);
            CreateNewTableButton.Name = "CreateNewTableButton";
            CreateNewTableButton.Size = new Size(113, 29);
            CreateNewTableButton.TabIndex = 20;
            CreateNewTableButton.Text = "Create New Table";
            CreateNewTableButton.UseVisualStyleBackColor = true;
            CreateNewTableButton.Click += CreateNewTableButton_Click;
            // 
            // NewTableNameInput
            // 
            NewTableNameInput.Location = new Point(350, 12);
            NewTableNameInput.Name = "NewTableNameInput";
            NewTableNameInput.PlaceholderText = "New Table Name";
            NewTableNameInput.Size = new Size(202, 23);
            NewTableNameInput.TabIndex = 21;
            // 
            // TargetTableSelectorComboBox
            // 
            TargetTableSelectorComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            TargetTableSelectorComboBox.FormattingEnabled = true;
            TargetTableSelectorComboBox.Location = new Point(550, 434);
            TargetTableSelectorComboBox.Name = "TargetTableSelectorComboBox";
            TargetTableSelectorComboBox.Size = new Size(121, 23);
            TargetTableSelectorComboBox.TabIndex = 22;
            // 
            // TransferRecordButton
            // 
            TransferRecordButton.Location = new Point(405, 431);
            TransferRecordButton.Name = "TransferRecordButton";
            TransferRecordButton.Size = new Size(113, 29);
            TransferRecordButton.TabIndex = 23;
            TransferRecordButton.Text = "Transfer Record";
            TransferRecordButton.UseVisualStyleBackColor = true;
            TransferRecordButton.Click += TransferRecordButton_Click;
            // 
            // TransferArrowLabel
            // 
            TransferArrowLabel.AutoSize = true;
            TransferArrowLabel.Location = new Point(524, 437);
            TransferArrowLabel.Name = "TransferArrowLabel";
            TransferArrowLabel.Size = new Size(20, 15);
            TransferArrowLabel.TabIndex = 24;
            TransferArrowLabel.Text = "->";
            // 
            // DeleteTableButton
            // 
            this.DeleteTableButton.Location = new Point(166, 8);
            this.DeleteTableButton.Name = "DeleteTableButton";
            this.DeleteTableButton.Size = new Size(113, 29);
            this.DeleteTableButton.TabIndex = 25;
            this.DeleteTableButton.Text = "Delete Table";
            this.DeleteTableButton.UseVisualStyleBackColor = true;
            this.DeleteTableButton.Click += this.DeleteTableButton_Click;
            // 
            // BudgetManagerForm
            // 
            ClientSize = new Size(875, 611);
            Controls.Add(this.DeleteTableButton);
            Controls.Add(TransferArrowLabel);
            Controls.Add(TransferRecordButton);
            Controls.Add(TargetTableSelectorComboBox);
            Controls.Add(NewTableNameInput);
            Controls.Add(CreateNewTableButton);
            Controls.Add(TableSelectorComboBox);
            Controls.Add(UpdateTransactionButton);
            Controls.Add(PrintButton);
            Controls.Add(EstimatedAvailabilityViewLabel);
            Controls.Add(EstimatedAvailabilityTextLabel);
            Controls.Add(IncomesSumViewLabel);
            Controls.Add(IncomesSumTextLabel);
            Controls.Add(ExpensesSumViewLabel);
            Controls.Add(ExpensesSumTextLabel);
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

        private void PrintButton_Click(object sender, EventArgs e)
        {
            string tableName = TableSelectorComboBox.SelectedItem?.ToString() ?? "";
            printTable = _repository.ReadAllTransactions(currentTableName);

            PrintDialog dialog = new PrintDialog
            {
                Document = printDocument
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            float y = 20;
            int columnSpacing = 200;
            Font font = new Font("Consolas", 10);
            Brush brush = Brushes.Black;

            for (int i = 1; i < printTable.Columns.Count - 1; i++)
            {
                e.Graphics.DrawString(printTable.Columns[i].ColumnName, font, brush, i * columnSpacing, y);
            }

            y += 30;

            foreach (DataRow row in printTable.Rows)
            {
                for (int i = 1; i < printTable.Columns.Count - 1; i++)
                {
                    e.Graphics.DrawString(row[i]?.ToString(), font, brush, i * columnSpacing, y);
                }
                y += 20;
            }
        }

        private void AddTransactionButton_Click(object sender, EventArgs e)
        {
            AddTransaction();
            UpdateViewLabels();
        }

        private void RemoveTransactionButton_Click(object sender, EventArgs e)
        {
            RemoveTransaction();
            UpdateViewLabels();
        }

        private void AddTransaction()
        {
            string category = CategoryComboBox.SelectedItem?.ToString();
            string description = DescriptionTextBox.Text.Trim();
            string amount = AmountTextBox.Text.Trim();
            DateTime date = DatePicker.Value;

            if (!Validator.IsValidInput(category, description, amount))
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            _repository.Write(category, description, amount, date, currentTableName);
            DisplayTransactionsInDataTable();
        }

        private void UpdateTransaction()
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a transaction.");
            }
            string category = CategoryComboBox.SelectedItem?.ToString();
            string description = DescriptionTextBox.Text.Trim();
            string amount = AmountTextBox.Text.Trim();
            DateTime date = DatePicker.Value;
            int id = Convert.ToInt32(this.dataGridView.SelectedRows[0].Cells["id"].Value);

            if (!Validator.IsValidInput(category, description, amount))
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            _repository.Update(id, category, description, amount, date, currentTableName);
            DisplayTransactionsInDataTable();
        }

        private void DisplayTransactionsInDataTable()
        {
            if (!string.IsNullOrEmpty(currentTableName))
            {
                DataTable table = _repository.ReadAllTransactions(currentTableName);
                DataGridHelper.SetupDataGridView(dataGridView, table);
            }
        }

        private void RemoveTransaction()
        {
            if (!DataGridHelper.GetSelectedRowId(dataGridView, out int id))
                return;

            var confirm = MessageBox.Show(
                "Are you sure you want to remove this transaction?",
                "Remove Transaction",
                MessageBoxButtons.YesNo);

            if (confirm != DialogResult.Yes) return;

            _repository.Delete(id, currentTableName);
            DisplayTransactionsInDataTable();
        }

        private string GetExpenses()
        {
            var expensesSum = _repository.GetSumFromCategory("Expense", currentTableName);
            return expensesSum.ToString();
        }

        private string GetIncomes()
        {
            var incomesSum = _repository.GetSumFromCategory("Income", currentTableName);
            return incomesSum.ToString();
        }

        private void UpdateViewLabels()
        {
            var incomes = GetIncomes();
            var expenses = GetExpenses();

            var estimatedAvailability = int.Parse(incomes) - int.Parse(expenses);

            IncomesSumViewLabel.Text = incomes;
            ExpensesSumViewLabel.Text = expenses;
            EstimatedAvailabilityViewLabel.Text = estimatedAvailability.ToString();
        }

        private void UpdateTransactionButton_Click(object sender, EventArgs e)
        {
            UpdateTransaction();
            UpdateViewLabels();
        }

        private void LoadTableNames()
        {
            var tables = _repository.GetAllTableNames();
            TableSelectorComboBox.Items.Clear();
            TableSelectorComboBox.Items.AddRange(tables.ToArray());

            if (tables.Any())
            {
                TableSelectorComboBox.SelectedItem = tables[0];
                currentTableName = tables[0];
            }
        }

        private void UpdateTargetTableSelectorComboBox()
        {
            var tables = _repository.GetAllTableNames();
            TargetTableSelectorComboBox.Items.Clear();
            var transferTables = tables.Where(t => t != currentTableName).ToArray();
            TargetTableSelectorComboBox.Items.AddRange(transferTables);

            if (transferTables.Any())
                TargetTableSelectorComboBox.SelectedIndex = 0;
        }

        private void CreateNewTableButton_Click(object sender, EventArgs e)
        {
            string newTableName = NewTableNameInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(newTableName))
            {
                MessageBox.Show("Please enter a valid table name.");
                return;
            }

            try
            {
                _repository.CreateNewTable(newTableName);
                MessageBox.Show($"Table '{newTableName}' created successfully.");

                LoadTableNames();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create table: {ex.Message}");
            }
        }

        private void TransferRecord()
        {
            string destinationTable = TargetTableSelectorComboBox.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(destinationTable))
            {
                MessageBox.Show("Please select a destination table.");
                return;
            }

            if (currentTableName == "" || destinationTable == "")
                return;

            if (!DataGridHelper.GetSelectedRowId(dataGridView, out int id))
                return;

            var confirm = MessageBox.Show(
                $"Are you sure you want to transfer this record to {destinationTable}?",
                "Transfer record",
                MessageBoxButtons.YesNo);

            if (confirm != DialogResult.Yes) return;

            _repository.TransferRecord(id, currentTableName, destinationTable);
        }

        private void TransferRecordButton_Click(object sender, EventArgs e)
        {
            TransferRecord();
            DisplayTransactionsInDataTable();
        }

        private void DeleteTableButton_Click(object sender, EventArgs e)
        {
            DeleteTable();
            LoadTableNames();
            DisplayTransactionsInDataTable();
            UpdateViewLabels();
        }

        private void DeleteTable()
        {
            string tableToDelete = TableSelectorComboBox.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tableToDelete))
            {
                MessageBox.Show("Please select a table to delete.");
                return;
            }

            var confirm = MessageBox.Show(
                $"Are you sure you want to delete this table: {tableToDelete}?",
                "Delete table",
                MessageBoxButtons.YesNo);

            if (confirm != DialogResult.Yes) return;

            try
            {
                _repository.DeleteTable(tableToDelete);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting table: {ex.Message}");
            }
        }

        private void CreateDefaultTable()
        {
            try
            {
                _repository.CreateDefaultTable();
                MessageBox.Show("Default table was created.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating default table: {ex.Message}");
            }
        }
    }
}
