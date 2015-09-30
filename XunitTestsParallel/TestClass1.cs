using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.ServiceLocation.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Moq;
using System;
using Xunit;

namespace XunitTestsParallel {
    public class TestClass1
    {
        private readonly Mock<IServiceLocator> mockServiceLocator;

        public TestClass1() {
            var mock = new Mock<IDataStore>();
            mock.Setup(m => m.GetData())
                .Returns(1);

            mockServiceLocator = new Mock<IServiceLocator>();
            mockServiceLocator.Setup(ms => ms.GetInstance<IDataStore>())
                .Returns(mock.Object);

            ServiceLocator.SetLocatorProvider(() => mockServiceLocator.Object);            
        } 


        [Fact]
        private void TestClass1TestMethod1() {
            using (ShimsContext.Create()) {
                ShimServiceLocator.CurrentGet = () => mockServiceLocator.Object;
                Assert.Equal<int>(1, ServiceLocator.Current.GetInstance<IDataStore>().GetData());
            }
        }

        [Fact]
        private void TestClass1TestMethod2() {
            using (ShimsContext.Create()) {
                ShimServiceLocator.CurrentGet = () => mockServiceLocator.Object;
                Assert.Equal<int>(1, ServiceLocator.Current.GetInstance<IDataStore>().GetData());
            }
        }
    }
}
