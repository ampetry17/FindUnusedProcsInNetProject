using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindUnusedProcsInProject
{
    public interface IFindProcsView
    {
        string SolutionPath();
        string ServerName();
        string DatabaseName();
        string Username();
        string Password();
        void DisplayProcessingFile(string fileName, int progress);
        void DisplayErrorToUser(string title, string message);
        void DisplayResults(List<UnusedProcItem> list);
    }
}
