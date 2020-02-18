using System;
using GoodApp.Data.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoodApp.Data.UnitTest
{
    [TestClass]
    public class RepositoryProviderTest
    {
        [TestMethod]
        public void Refactor_OK()
        {
            try
            {
                var repository = RepositoryProvider.Create();
                var groupRepository = repository.Get<GroupRepository>();
                Assert.IsNotNull(groupRepository);
                Assert.IsInstanceOfType(groupRepository, typeof (GroupRepository));
            }
            catch (UnitTestAssertException exception)
            {
                throw;
            }
        }
    }
}
