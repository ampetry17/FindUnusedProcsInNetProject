namespace FindUnusedProcsInProject
{
    partial class frmDisplayListOfProcs
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dgUnusedProcs = new System.Windows.Forms.DataGridView();
            this.procNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.countOfHitsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unusedProcItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgUnusedProcs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unusedProcItemBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dgUnusedProcs
            // 
            this.dgUnusedProcs.AllowUserToAddRows = false;
            this.dgUnusedProcs.AllowUserToDeleteRows = false;
            this.dgUnusedProcs.AutoGenerateColumns = false;
            this.dgUnusedProcs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgUnusedProcs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgUnusedProcs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.procNameDataGridViewTextBoxColumn,
            this.countOfHitsDataGridViewTextBoxColumn});
            this.dgUnusedProcs.DataSource = this.unusedProcItemBindingSource;
            this.dgUnusedProcs.Location = new System.Drawing.Point(5, 12);
            this.dgUnusedProcs.Name = "dgUnusedProcs";
            this.dgUnusedProcs.ReadOnly = true;
            this.dgUnusedProcs.Size = new System.Drawing.Size(979, 652);
            this.dgUnusedProcs.TabIndex = 0;
            this.dgUnusedProcs.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgUnusedProcs_CellDoubleClick);
            // 
            // procNameDataGridViewTextBoxColumn
            // 
            this.procNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.procNameDataGridViewTextBoxColumn.DataPropertyName = "ProcName";
            this.procNameDataGridViewTextBoxColumn.HeaderText = "Stored Procedure Name";
            this.procNameDataGridViewTextBoxColumn.Name = "procNameDataGridViewTextBoxColumn";
            this.procNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.procNameDataGridViewTextBoxColumn.Width = 133;
            // 
            // countOfHitsDataGridViewTextBoxColumn
            // 
            this.countOfHitsDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.countOfHitsDataGridViewTextBoxColumn.DataPropertyName = "CountOfHits";
            this.countOfHitsDataGridViewTextBoxColumn.HeaderText = "Number of Hits";
            this.countOfHitsDataGridViewTextBoxColumn.Name = "countOfHitsDataGridViewTextBoxColumn";
            this.countOfHitsDataGridViewTextBoxColumn.ReadOnly = true;
            this.countOfHitsDataGridViewTextBoxColumn.Width = 78;
            // 
            // unusedProcItemBindingSource
            // 
            this.unusedProcItemBindingSource.DataSource = typeof(FindUnusedProcsInProject.UnusedProcItem);
            // 
            // frmDisplayListOfProcs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(996, 676);
            this.Controls.Add(this.dgUnusedProcs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmDisplayListOfProcs";
            this.Text = "Unused Proc List";
            ((System.ComponentModel.ISupportInitialize)(this.dgUnusedProcs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unusedProcItemBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgUnusedProcs;
        private System.Windows.Forms.DataGridViewTextBoxColumn procNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn countOfHitsDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource unusedProcItemBindingSource;

    }
}