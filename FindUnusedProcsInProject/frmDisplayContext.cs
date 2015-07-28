using System;
using System.Windows.Forms;

namespace FindUnusedProcsInProject
{
    public partial class frmDisplayContext : Form
    {
        private UnusedProcItem _itemToDisplay;

        public frmDisplayContext(UnusedProcItem ItemToDisplay)
        {
            InitializeComponent();
            _itemToDisplay = ItemToDisplay;
            Text = string.Format("{0} hit context.", ItemToDisplay.ProcName);
            FillDataGrid();
        }

        private void FillDataGrid()
        {
            foreach (Tuple<string, int, string> contextItem in _itemToDisplay.ContextList)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.Cells.Add(new DataGridViewTextBoxCell { Value = contextItem.Item1 });
                row.Cells.Add(new DataGridViewTextBoxCell { Value = contextItem.Item2 });
                row.Cells.Add(new DataGridViewTextBoxCell { Value = contextItem.Item3 });
                dgHitList.Rows.Add(row);
            }
        }
    }
}
