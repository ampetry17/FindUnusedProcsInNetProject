using System;
using System.Collections.Generic;
using System.Linq;
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
        private List<UnusedProcItem> _unusedProcItems;

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
            _unusedProcItems = new List<UnusedProcItem>
            {
                new UnusedProcItem
                {
                    ProcName = "ExpectedStoreProc"
                }
            };
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
            string[] expected = { string.Format(@"{0}\TestFileSubFolder", root) };
            string[] actual;
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

        [TestMethod]
        public void FindProcsInFileList_Finds_Proc_When_In_File()
        {
            List<string> fileList = new List<string>
            {
                string.Format(@"{0}\UnitTestContentFileHit.sql", root),
                string.Format(@"{0}\UnitTestContentFileMiss.sql", root),
                string.Format(@"{0}\UnitTestContentFileNotInList.txt", root)
            };
            _presenter.FindProcsInFileList(_unusedProcItems, fileList);
            foreach (UnusedProcItem procItem in _unusedProcItems)
            {
                Assert.IsTrue(procItem.CountOfHits == 1, "FindProcsInFileList found the proc too many times. It found it {0}", procItem.CountOfHits);
                Assert.IsTrue(procItem.CountOfHits == procItem.ContextList.Count, "FindProcsInFileList has a different number of counts ({0}) from items in the context list ({1})", procItem.CountOfHits, procItem.ContextList.Count );
                Assert.IsTrue(procItem.ContextList[0].Item1 == string.Format(@"{0}\UnitTestContentFileHit.sql", root), string.Format("Expected to find hit in: \n {0}\n. Found it in:\n{1}", string.Format(@"{0}\UnitTestContentFileHit.sql", root), procItem.ContextList[0].Item1));
                Assert.AreEqual(procItem.ContextList[0].Item2, 5, string.Format("FindProcsInFileList expected to match on line 7, but matched on {0}", procItem.ContextList[0].Item2));
                Assert.AreEqual(procItem.ContextList[0].Item3, "We will be looking for ExpectedStoreProc as the name of the proc that we're searching for.", string.Format("FindProcsInFileList should have matched this line:\nWe should not find this line which has expectednotproc.\n Instead, it matched {0}", procItem.ContextList[0].Item3));
            }            
        }

        [TestMethod]
        public void FindProcsInLine_Matches_Only_Lines_With_Proc()
        {
            List<UnusedProcItem> needles = GenerateProcListForLineTests();
            FindProcsPresenter.FindProcsInLine(needles, "programFile.cs", "This line contains a call to spMatch but not to that other proc.", 1);
            Assert.AreEqual(needles.Count(x => x.CountOfHits > 0), 1, string.Format("Meant to find one match in FindProcsInLine. Found {0}", needles.Count(x => x.CountOfHits > 0)));
            UnusedProcItem matchingItem = needles.FirstOrDefault(x => x.CountOfHits == 1);
            Assert.IsNotNull(matchingItem, "FindProcsInLine did not find a match. Should have found one on spMatch.");
            Assert.AreEqual(matchingItem.ProcName, "spMatch", "FindProcsInLine was meant to find a match on spMatch, but found it on {0}", matchingItem.ProcName);
        }

        [TestMethod]
        public void FindProcsInLine_Ignores_Files_That_Generate_Proc_In_Question()
        {
            List<UnusedProcItem> needles = GenerateProcListForLineTests();
            FindProcsPresenter.FindProcsInLine(needles, "spMatch.sql", "CREATE PROC spMatch", 1);
            Assert.AreEqual(needles.Count(x => x.CountOfHits > 0), 0, string.Format("Meant to find no matches in FindProcsInLine. Found {0}", needles.Count(x => x.CountOfHits > 0)));           
        }

        private List<UnusedProcItem> GenerateProcListForLineTests()
        {
            List<UnusedProcItem> ProcList = new List<UnusedProcItem>();
            UnusedProcItem match = new UnusedProcItem()
            {
                ProcName = "spMatch"
            };
            ProcList.Add(match);
            UnusedProcItem misMatch = new UnusedProcItem()
            {
                ProcName = "NotMatching"
            };
            ProcList.Add(misMatch);
            return ProcList;
        }
    }
}
