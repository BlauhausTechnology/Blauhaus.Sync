using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.TestHelpers.MockBuilders;
using Blauhaus.TestHelpers.BaseTests;
using NUnit.Framework;

namespace Blauhaus.Sync.Tests.Client.Base
{
    public abstract class BaseClientSyncTest<TSut> : BaseServiceTest<TSut> where TSut : class
    {
        [SetUp]
        public virtual void Setup()
        {
            Cleanup();

            AddService(MockAnalyticsService.Object);
            AddService(MockLogger.Object);
            AddService(MockAnalyticsContext.Object);
        }

        protected AnalyticsLoggerMockBuilder<TSut> MockLogger => AddMock<AnalyticsLoggerMockBuilder<TSut>, IAnalyticsLogger<TSut>>().Invoke();
        protected AnalyticsServiceMockBuilder MockAnalyticsService => AddMock<AnalyticsServiceMockBuilder, IAnalyticsService>().Invoke();
        protected AnalyticsContextMockBuilder MockAnalyticsContext => AddMock<AnalyticsContextMockBuilder, IAnalyticsContext>().Invoke(); 
    }
}