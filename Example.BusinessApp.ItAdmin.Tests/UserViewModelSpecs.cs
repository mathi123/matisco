using System;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using Example.BusinessApp.Infrastructure.Models;
using Example.BusinessApp.Infrastructure.Services;
using Example.BusinessApp.ItAdmin.ViewModels;
using Example.BusinessApp.ItAdmin.Views;
using Matisco.Wpf;
using Matisco.Wpf.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prism.Regions;

namespace Example.BusinessApp.ItAdmin.Specs
{
    [TestClass]
    public class UserViewModelSpecs
    {
        private Bootstrapper _bootstrapper;
        private UserViewModel _userViewModel;

        private User _testUser = new User()
        {
            Id = 1,
            Name = "Test user"
        };

        [TestInitialize]
        public void Initialize()
        {
            _bootstrapper = new Bootstrapper();
            _bootstrapper.Run();

            var userServiceMoq = new Mock<IUserService>();
            userServiceMoq.Setup(ser => ser.GetById(_testUser.Id)).Returns(_testUser);

            _userViewModel = _bootstrapper.Container.Resolve<UserViewModel>(new TypedParameter(typeof(IUserService), userServiceMoq.Object));
        }

        [TestMethod]
        public void IsNotInEditModeAfterCreating()
        {
            Assert.IsFalse(_userViewModel.EditMode);
        }

        [TestMethod]
        public void CannotBeInitializedWithoudId()
        {
            var navService = _bootstrapper.Container.Resolve<IRegionNavigationService>();
            navService.Region = new Region();

            var badParameters = new NavigationParameters
            {
                { "BlaBlaBla", 1 }
            };

            var navigationContext = new NavigationContext(navService, new Uri("/", UriKind.Relative), badParameters);

            Assert.IsFalse(_userViewModel.IsNavigationTarget(navigationContext));
        }

        [TestMethod]
        public void CanBeInitializedWithIdInContext()
        {
            var navService = _bootstrapper.Container.Resolve<IRegionNavigationService>();
            navService.Region = new Region();

            var goodParameters = new NavigationParameters
            {
                { "Id", 1 }
            };

            var navigationContext = new NavigationContext(navService, new Uri("/", UriKind.Relative), goodParameters);

            Assert.IsTrue(_userViewModel.IsNavigationTarget(navigationContext));
        }

        [TestMethod]
        public void IsNotInEditModeAfterNavigating()
        {
            NavigateToRecord();
            Assert.IsFalse(_userViewModel.EditMode);
        }

        [TestMethod]
        public async Task LoadsDataCorrectly()
        {
            await _userViewModel.LoadUserAsync(_testUser.Id);

            Assert.AreEqual(_testUser.Name, _userViewModel.Name);
        }

        [TestMethod]
        public void GoesIntoEditModeOnWenEditOrSaveIsExecuted()
        {
            _userViewModel.EditSaveCommand.Execute(null);

            Assert.IsTrue(_userViewModel.EditMode);
        }

        [TestMethod]
        public void GoesOutOfEditModeOnWenCancelIsExecuted()
        {
            _userViewModel.EditSaveCommand.Execute(null);
            _userViewModel.CancelCloseCommand.Execute(null);

            Assert.IsFalse(_userViewModel.EditMode);
        }

        private void NavigateToRecord()
        {
            var navService = _bootstrapper.Container.Resolve<IRegionNavigationService>();
            navService.Region = new Region();

            var goodParameters = new NavigationParameters
            {
                { "Id", 1 }
            };

            var navigationContext = new NavigationContext(navService, new Uri("/", UriKind.Relative), goodParameters);

            _userViewModel.OnNavigatedTo(navigationContext);
        }
    }
}
