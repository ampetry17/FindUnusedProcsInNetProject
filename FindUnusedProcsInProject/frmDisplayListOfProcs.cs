using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FindUnusedProcsInProject
{
    public partial class frmDisplayListOfProcs : Form
    {
        private List<UnusedProcItem> _unusedProcs;
        
        public frmDisplayListOfProcs(List<UnusedProcItem> unusedProcs)
        {
            InitializeComponent();
            _unusedProcs = unusedProcs;
            dgUnusedProcs.DataSource = _unusedProcs;
        }

        private void dgUnusedProcs_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 0 && sender as DataGridViewHeaderCell != null)
            {
                return;
            }
            UnusedProcItem Item = _unusedProcs.FirstOrDefault(x => x.ProcName == dgUnusedProcs.Rows[e.RowIndex].Cells[0].Value.ToString());
            if (Item == null)
                return;
            frmDisplayContext displayContext = new frmDisplayContext(Item);
            displayContext.Show();
        }
    }
}
