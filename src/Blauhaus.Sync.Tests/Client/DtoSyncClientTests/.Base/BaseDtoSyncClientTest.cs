using System;
using Blauhaus.Common.Abstractions;
using Blauhaus.Domain.TestHelpers.MockBuilders.Common.CommandHandlers;
using Blauhaus.Sync.Abstractions.Common;
using Blauhaus.Sync.Client;
using Blauhaus.Sync.TestHelpers;
using Blauhaus.Sync.TestHelpers.MockBuilders;
using Blauhaus.Sync.Tests.Client.Base;
using Blauhaus.Sync.Tests.Client.TestObjects;
using Blauhaus.Sync.Tests.TestObjects;
using Blauhaus.TestHelpers.MockBuilders;

namespace Blauhaus.Sync.Tests.Client.DtoSyncClientTests.Base
{
    public class BaseDtoSyncClientTest : BaseClientSyncTest<DtoSyncHandler<MyDto, Guid, MyTestUser>>
    {
        protected CommandHandlerMockBuilder<DtoBatch<MyDto, Guid>, DtoSyncCommand> MockSyncCommandHandler = null!;
        protected SyncDtoCacheMockBuilder<MyDto, Guid, MyTestUser> MockSyncDtoCache = null!;
        protected MyTestUser TestUser = new();

        public override void Setup()
        {
            base.Setup();

            MockSyncCommandHandler = new CommandHandlerMockBuilder<DtoBatch<MyDto, Guid>, DtoSyncCommand>();
            AddService(MockSyncCommandHandler.Object);

            MockSyncDtoCache = new SyncDtoCacheMockBuilder<MyDto, Guid, MyTestUser>();
            AddService(MockSyncDtoCache.Object);
        }
    }
}