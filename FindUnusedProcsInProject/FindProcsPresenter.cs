using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace FindUnusedProcsInProject
{
    public class FindProcsPresenter
    {
        private IFindProcsView _view;
        private BackgroundWorker _bw;
        
        public FindProcsPresenter(IFindProcsView View)
        {
            _view = View;
            _bw = new BackgroundWorker();
            _bw.DoWork += _bw_DoWork;
            _bw.RunWorkerCompleted += _bw_RunWorkerCompleted;
            _bw.ProgressChanged += _bw_ProgressChanged;
            _bw.WorkerReportsProgress = true;
        }

        void _bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _view.DisplayProcessingFile((string)e.UserState, e.ProgressPercentage);
        }

        void _bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _view.DisplayResults(((List<UnusedProcItem>)e.Result).OrderBy(x => x.CountOfHits).ToList());
        }

        void _bw_DoWork(object sender, DoWorkEventArgs e)
        {
            WalkDirectory((List<UnusedProcItem>)e.Argument);
            e.Result = e.Argument;
        }

        public void UserRequestedToFindUnusedProcs()
        {
            List<UnusedProcItem> UnusedProcs = FindProcsModel.ProcList(GetConnectionString());
            _bw.RunWorkerAsync(UnusedProcs);
        }

        public string GetConnectionString()
        {
            return string.Format("Server={0};Database={1};User Id={2};Password={3}", _view.ServerName(), _view.DatabaseName(), _view.Username(), _view.Password());
        }

        public void WalkDirectory(List<UnusedProcItem> ItemsToSearchFor)
        {
            if (ItemsToSearchFor.Count == 0)
            {
                return;
            }
            
            List<string> FileList = GetFileList();

            FindProcsInFileList(ItemsToSearchFor, FileList);
        }

        public List<string> GetFileList()
        {
            List<string> FileList = new List<string>();
            Stack<string> dirStack = new Stack<string>();
            if (!Directory.Exists(_view.SolutionPath()))
            {
                throw new Exception("Solution directory does not exist.");
            }
            dirStack.Push(_view.SolutionPath());

            while (dirStack.Count > 0)
            {
                BuildFileList(dirStack, FileList);
            }
            return FileList;
        }

        public void BuildFileList(Stack<string> dirStack, List<string> FileList)
        {
            string currentDirectory = dirStack.Pop();
            string[] subDirectories;
            if (!GetSubDirectories(currentDirectory, out subDirectories))
                return;

            string[] files;
            if (!GetFiles(currentDirectory, out files))
                return;
            FileList.AddRange(files.Where(x => x.EndsWith(".cs") || x.EndsWith(".vb") || x.EndsWith(".sql")));

            foreach (string subDirectory in subDirectories)
            {
                dirStack.Push(subDirectory);
            }
        }

        public bool GetFiles(string currentDirectory, out string[] files)
        {
            try
            {
                files = Directory.GetFiles(currentDirectory);
            }
            catch (Exception e)
            {
                _view.DisplayErrorToUser(e.Message, string.Format("{0} - {1}", currentDirectory, e.GetType()));
                files = new string[] {};
                return false;
            }
            return true;
        }

        public bool GetSubDirectories(string currentDirectory, out string[] subDirectories)
        {
            try
            {
                subDirectories = Directory.GetDirectories(currentDirectory);
            }
            catch (Exception e)
            {
                _view.DisplayErrorToUser(e.Message, string.Format("{0} - {1}", currentDirectory, e.GetType()));
                subDirectories = new string[] {};
                return false;
            }
            return true;
        }

        public void FindProcsInFileList(List<UnusedProcItem> itemsToSearchFor, List<string> fileList)
        {
            int i = 0;
            foreach (string file in fileList)
            {
                _bw.ReportProgress((i * 100) / fileList.Count, string.Format("{0} of {1} files: {2}", i, fileList.Count, file.Replace(_view.SolutionPath(), "")));
                FindProcsInFile(itemsToSearchFor, file);
                i += 1;
            }
            _bw.ReportProgress(100, "Finished");
        }

        public static void FindProcsInFile(List<UnusedProcItem> needleList, string file)
        {
            //Before this is ready for Appointments or general use, I need to create a black-list file of files that can be safely ignored.
            if (file.EndsWith("CompassFramework_InitialSchema.sql"))
            {
                return;
            }
            int lineCount = 1;
            StreamReader fileStream = new StreamReader(file);
            string line;
            while ((line = fileStream.ReadLine()) != null)
            {
                FindProcsInLine(needleList, file, line, lineCount);
                lineCount += 1;
            }
        }

        public static void FindProcsInLine(IEnumerable<UnusedProcItem> needleList, string file, string line, int lineCount)
        {
            //Future versions will use a regex here to m/([/w|\'|\"|\.]{ProcName}[/w|\'|\"|\.])/ to find cases where we match 
            //whitespace, a dot, or single or double quotes around our needle. That should cut back on false positives.
            string lineText = line;
            foreach (UnusedProcItem needle in needleList.Where(x => lineText.Contains(x.ProcName) && !file.EndsWith(string.Format("{0}.sql", x.ProcName))))
            {
                AddLineToContextList(needle, file, lineCount, line);
            }
        }

        public static void AddLineToContextList(UnusedProcItem needle, string file, int lineNumber, string line)
        {
            needle.CountOfHits += 1;
            needle.ContextList.Add(new Tuple<string, int, string>(file, lineNumber, line));
        }
    }
}
