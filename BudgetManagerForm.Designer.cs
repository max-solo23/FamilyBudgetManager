﻿namespace FamilyBudgetManager
{
    partial class BudgetManagerForm
    {

        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private Label CategoryLabel;
        private Label DescriptionLabel;
        private TextBox DescriptionTextBox;
        private Label AmountLabel;
        private TextBox AmountTextBox;
        private Label DateLabel;
        private DateTimePicker DatePicker;
        private DataGridView dataGridView;
        private Button AddTransactionButton;
        private ComboBox CategoryComboBox;
        private Button RemoveTransactionButton;
        private Label ExpensesSumTextLabel;
        private Label ExpensesSumViewLabel;
        private Label IncomesSumViewLabel;
        private Label IncomesSumTextLabel;
        private Label EstimatedAvailabilityViewLabel;
        private Label EstimatedAvailabilityTextLabel;
        private Button PrintButton;
        private Button UpdateTransactionButton;
        private ComboBox TableSelectorComboBox;
        private Button CreateNewTableButton;
        private TextBox NewTableNameInput;
        private ComboBox TargetTableSelectorComboBox;
        private Button TransferRecordButton;
        private Label TransferArrowLabel;
        private Button DeleteTableButton;
        private Button CopyRecordButton;
        private Label TotalAmountForDescriptionLabel;
        private CheckBox TotalAmountForDescriptionCheckBox;
    }
}
