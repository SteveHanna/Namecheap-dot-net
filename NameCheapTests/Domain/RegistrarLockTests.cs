using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NameCheapTests.Domain
{
    /// <summary>
    /// Tests the registrar lock methods.
    /// </summary>
    /// <remarks>
    /// As the tests ultimately affect external APIs, it's difficult to set up
    /// isolated, repeatable tests. As such, we constructed a single test
    /// that explores all scenarios based on an initial state.
    /// </remarks>
    [TestClass]
    public class RegistrarLockTests : TestBase
    {
        [TestInitialize]
        public void BeforeEachTest()
        {
            // hack to avoid Too many requests errors.
            // TODO: remove with https://github.com/SteveHanna/Namecheap-dot-net/issues/10
            System.Threading.Thread.Sleep(TestThrottleMilliseconds.Value);
        }
        
        [TestMethod]
        public void CanLockAndUnlockTheRegistrarStatus()
        {
            bool isLocked = _api.Domains.GetRegistrarLock(_domainName.Value);

            if (isLocked)
            {
                TestUnlock();
                TestLock();
            }
            else
            {
                TestLock();
                TestUnlock();
            }
        }

        private void TestUnlock()
        {
            _api.Domains.SetRegistrarUnlock(_domainName.Value);
            var isLocked = _api.Domains.GetRegistrarLock(_domainName.Value);
            Assert.IsFalse(isLocked, "Expected registrar lock status to be unlocked because the test just unlocked it.");
        }

        private void TestLock()
        {
            _api.Domains.SetRegistrarLock(_domainName.Value);
            var isLocked = _api.Domains.GetRegistrarLock(_domainName.Value);
            Assert.IsTrue(isLocked, "Expected registrar lock status to be locked because the test just locked it.");
        }
    }
}
