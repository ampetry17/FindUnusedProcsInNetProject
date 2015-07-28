using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace FindUnusedProcsInProject
{
    public partial class FrmFindUnusedProcs : Form, IFindProcsView
    {
        private FindProcsPresenter _presenter;

        public FrmFindUnusedProcs()
        {
            InitializeComponent();
            _presenter = new FindProcsPresenter(this);
        }

        public string SolutionPath()
        {
            return txtSolutionPath.Text;
        }

        public string ServerName()
        {
            return txtServerName.Text;
        }

        public string DatabaseName()
        {
            return txtDatabaseName.Text;
        }

        public string Username()
        {
            return txtUsername.Text;
        }

        public string Password()
        {
            return txtPassword.Text;
        }

        public void DisplayProcessingFile(string fileName, int progress)
        {
            lblFileProcessing.Text = fileName;
            progressBar.Value = progress;
        }

        public void DisplayResults(List<UnusedProcItem> list)
        {
            frmDisplayListOfProcs displayForm = new frmDisplayListOfProcs(list);
            displayForm.Show();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            _presenter.UserRequestedToFindUnusedProcs();
            progressBar.Visible = true;
        }


        public void DisplayErrorToUser(string title, string message)
        {
            MessageBox.Show(message, title);
        }
    }
}
