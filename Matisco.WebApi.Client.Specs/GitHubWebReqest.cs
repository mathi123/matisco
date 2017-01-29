using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Matisco.WebApi.Client.Specs
{
    [TestClass]
    public class GitHubWebReqest
    {
        private JsonWebApi _jsonWebApi;

        [TestInitialize]
        public void Initialize()
        {
            var configurationManager = new ConfigurationManager("https://api.github.com");
            _jsonWebApi = new JsonWebApi(new HttpClientFactory(configurationManager), new WebApiExceptionHandler(true));
        }

        [TestMethod]
        public void TestWithGithubUser()
        {
            var user = _jsonWebApi.GetAsync<GithubUser>("users/mathi123").Result;

            Assert.AreEqual(ServiceStatusEnum.Success, user.Status);
            Assert.AreEqual("Mathias Colpaert", user.Data.name);
        }
    }
}
