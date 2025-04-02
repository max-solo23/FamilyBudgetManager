namespace FamilyBudgetManager
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
    }
}
