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

        [TestInitialize]
        public void Setup()
        {
            SetupView();
            _presenter = new FindProcsPresenter(_view.Object);
        }

        private void SetupView()
        {
            _view = new Mock<IFindProcsView>();
            _view.Setup(x => x.ServerName()).Returns(serverName);
            _view.Setup(x => x.DatabaseName()).Returns(databaseName);
            _view.Setup(x => x.Password()).Returns(password);
            _view.Setup(x => x.Username()).Returns(userName);
        }

        [TestMethod]
        public void GetConnectionString_returns_correct_connection_string()
        {
            const string expected = "Server=Server;Database=Database;User Id=User;Password=Password";
            string actual = _presenter.GetConnectionString();
            Assert.AreEqual(expected, actual);
        }
    }
}
