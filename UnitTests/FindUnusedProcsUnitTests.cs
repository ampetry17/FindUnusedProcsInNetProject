using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FindUnusedProcsInProject;

namespace UnitTests
{
    [TestClass]
    public class FindUnusedProcsUnitTests
    {
        private FindProcsPresenter _presenter;
        private Mock<IFindProcsView> _view;
        private const string serverName = "Server";
        private const string databaseName = "Database";
        private const string userName = "User";
        private const string password = "Password";
        private string root = "";

        private void SetupView()
        {
            _view = new Mock<IFindProcsView>();
            _view.Setup(x => x.ServerName()).Returns(serverName);
            _view.Setup(x => x.DatabaseName()).Returns(databaseName);
            _view.Setup(x => x.Password()).Returns(password);
            _view.Setup(x => x.Username()).Returns(userName);
            root = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf(@"\TestResults"));
            root = string.Format("{0}\\UnitTests\\TestFiles", root);
            _view.Setup(x => x.SolutionPath()).Returns(root);  
        }

        [TestInitialize]
        public void before_each()
        {
            SetupView();
            _presenter = new FindProcsPresenter(_view.Object);
        }

        [TestMethod]
        public void GetConnectionString_returns_correct_connection_string()
        {
            const string expected = "Server=Server;Database=Database;User Id=User;Password=Password";
            string actual = _presenter.GetConnectionString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WalkDirectory_returns_immediately_if_no_items_to_search_for()
        {
            List<UnusedProcItem> SearchList = new List<UnusedProcItem>();
            _presenter.WalkDirectory(SearchList);
            _view.Verify(x => x.SolutionPath(), Times.Never());
        }

        [TestMethod]
        public void GetFileList_Gets_Exactly_The_Files_Expected()
        {
            List<string> expected = new List<string>
            {
                string.Format(@"{0}\UnitTestContentFileHit.sql", root), 
                string.Format(@"{0}\UnitTestContentFileMiss.sql", root),
                string.Format(@"{0}\TestFileSubFolder\UnitTestContentSubFileMiss.sql", root)
            };
            List<string> actual = _presenter.GetFileList();
            Assert.IsTrue(actual.Count == expected.Count, string.Format("GetFileList was meant to return {0}. It returned {1}", expected.Count, actual.Count));
            foreach (string a in actual)
            {
                Assert.IsTrue(expected.Any(x => x == a), string.Format("GetFileList returned {0} which wasn't found in expected.", a));
            }
        }

        [TestMethod]
        public void BuildFileList_Ignores_Files_Not_Ending_In_sql()
        {
            List<string> actual = _presenter.GetFileList();
            Assert.IsTrue(actual.Count > 0, "BuildFileList didn't return anything.");
            Assert.IsFalse(actual.Any(x => x.Contains("UnitTestContentFileNotInList.txt")), "BuildFileList included a text file.");
            foreach (string a in actual)
            {
                Assert.IsTrue(a.EndsWith(".sql") || a.EndsWith(".vb") || a.EndsWith(".cs"), string.Format("Actual has a file that doesn't end in .sql, .vb. or .cs. File: {0}", a));
            }
        }

        [TestMethod]
        public void GetSubDirectories_Finds_Sub_Directories()
        {
            string[] expected = new string[] { string.Format(@"{0}\TestFileSubFolder", root) };
            string[] actual = null;
            bool resultBool = _presenter.GetSubDirectories(root, out actual);

            Assert.IsTrue(resultBool, "GetSubDirectories returned false, and should not.");
            Assert.IsTrue(actual.Count() == expected.Count(), string.Format("GetSubDirectories finds the wrong number of subdirectories. It found {0} and should have {1}", actual.Count(), expected.Count()));
            foreach (string s in actual)
            {
                Assert.IsTrue(expected.Contains(s), string.Format("Actual contains {0} which is not in expected results.", s));
            }
        }

        [TestMethod]
        public void GetFiles_Returns_All_Files_In_A_Directory()
        {
            string[] expected = new string[]
            {
                string.Format(@"{0}\UnitTestContentFileHit.sql", root),
                string.Format(@"{0}\UnitTestContentFileMiss.sql", root),
                string.Format(@"{0}\UnitTestContentFileNotInList.txt", root)
            };
            string[] actual = null;
            bool result = _presenter.GetFiles(root, out actual);
            
            Assert.IsTrue(result, "GetFiles returned false, and should not.");
            Assert.IsTrue(actual.Count() == expected.Count(), string.Format("GetFiles returns {0} and is expected to return {1}", actual.Count(), expected.Count()));
            foreach (string s in actual)
            {
                Assert.IsTrue(expected.Contains(s), string.Format("Actual contains {0} which is not in expected results.", s));
            }
        }
        
    }
}
