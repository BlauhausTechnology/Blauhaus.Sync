using System;
using Blauhaus.DeviceServices.Abstractions.Connectivity;
using Blauhaus.DeviceServices.TestHelpers.MockBuilders;
using Blauhaus.SignalR.Client.Connection.Proxy;
using Blauhaus.Sync.Client.SignalR;
using Blauhaus.Sync.Tests.Client.Base;
using Blauhaus.Sync.Tests.TestObjects;

namespace Blauhaus.Sync.Tests.Client.SignalRSyncDtoClientTests.Base
{
    public abstract class BaseSignalRSyncDtoClientTest : BaseClientSyncTest<SignalRSyncDtoClient<MyDto, Guid>>
    { 
        public override void Setup()
        {
            base.Setup();
            
            AddService(MockConnectivityService.Object);
            AddService(MockSignalRConnectionProxy.Object);
        }

        protected ConnectivityServiceMockBuilder MockConnectivityService => AddMock<ConnectivityServiceMockBuilder, IConnectivityService>().Invoke();
        protected SignalRConnectionProxyMockBuilder MockSignalRConnectionProxy => AddMock<SignalRConnectionProxyMockBuilder, ISignalRConnectionProxy>().Invoke(); 
    }
}