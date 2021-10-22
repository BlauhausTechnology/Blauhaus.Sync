using System;
using Blauhaus.Domain.Abstractions.DtoHandlers;
using Blauhaus.Domain.Abstractions.Entities;
using Blauhaus.Domain.TestHelpers.EntityBuilders;
using Blauhaus.Orleans.Abstractions.Resolver;
using Blauhaus.Sync.TestHelpers.EfCore;
using Blauhaus.TestHelpers.Builders.Base;
using Moq;
using Orleans;

namespace Blauhaus.Sync.TestHelpers.Orleans
{
    
    public class GrainEntitySyncSet<TEntity, TEntityBuilder, TDto, TDtoBuilder, TGrain, TGrainResolver> : EntitySyncSet<TEntity, TEntityBuilder>
        where TEntityBuilder : BaseServerEntityBuilder<TEntityBuilder, TEntity>
        where TEntity : class, IServerEntity, IDtoOwner<TDto>
        where TGrain : class, IGrainWithGuidKey, IDtoOwner<TDto>
        where TDto : IClientEntity<Guid>
        where TDtoBuilder : class, IBuilder<TDtoBuilder, TDto>, new()
        where TGrainResolver: class, IGrainResolver
    {
        public GrainEntitySyncSet(
            DateTime runTime, 
            Mock<TGrainResolver> grainResolverMockBuilder) 
                : base(runTime)
        {
            DistantPastDtoArchivedBuilder = new TDtoBuilder()
                .With(x => x.Id, DistantPastArchivedId)
                .With(x => x.EntityState, EntityState.Archived)
                .With(x => x.ModifiedAtTicks, DistantPastArchivedTime.Ticks);
            MockDistantPastArchivedGrain = new Mock<TGrain>();
            MockDistantPastArchivedGrain.Setup(x => x.GetDtoAsync())
                .ReturnsAsync(() => DistantPastDtoArchivedBuilder.Object);
            grainResolverMockBuilder.Setup(x => x.Resolve<TGrain>(DistantPastArchivedId))
                .Returns(MockDistantPastArchivedGrain.Object);
            
            PastDtoBuilder = new TDtoBuilder()
                .With(x => x.EntityState, EntityState.Active)
                .With(x => x.Id, PastId)
                .With(x => x.ModifiedAtTicks, PastTime.Ticks);
            MockPastGrain = new Mock<TGrain>();
            MockPastGrain.Setup(x => x.GetDtoAsync())
                .ReturnsAsync(() => PastDtoBuilder.Object);
            grainResolverMockBuilder.Setup(x => x.Resolve<TGrain>(PastId))
                .Returns(MockPastGrain.Object);

            PastDraftDtoBuilder = new TDtoBuilder()
                .With(x => x.Id, PastDraftId)
                .With(x => x.EntityState, EntityState.Draft)
                .With(x => x.ModifiedAtTicks, PastDraftTime.Ticks);
            MockPastDraftGrain = new Mock<TGrain>();
            MockPastDraftGrain.Setup(x => x.GetDtoAsync())
                .ReturnsAsync(() => PastDraftDtoBuilder.Object);
            grainResolverMockBuilder.Setup(x => x.Resolve<TGrain>(PastDraftId))
                .Returns(MockPastDraftGrain.Object);
            
            PresentDtoBuilder = new TDtoBuilder()
                .With(x => x.Id, PresentId)
                .With(x => x.EntityState, EntityState.Active)
                .With(x => x.ModifiedAtTicks, PresentTime.Ticks);
            MockPresentGrain = new Mock<TGrain>();
            MockPresentGrain.Setup(x => x.GetDtoAsync())
                .ReturnsAsync(() => PresentDtoBuilder.Object);
            grainResolverMockBuilder.Setup(x => x.Resolve<TGrain>(PresentId))
                .Returns(MockPresentGrain.Object);

            FutureDtoBuilder = new TDtoBuilder()
                .With(x => x.Id, FutureId)
                .With(x => x.EntityState, EntityState.Active)
                .With(x => x.ModifiedAtTicks, FutureTime.Ticks);
            MockFutureGrain = new Mock<TGrain>();
            MockFutureGrain.Setup(x => x.GetDtoAsync())
                .ReturnsAsync(() => FutureDtoBuilder.Object);
            grainResolverMockBuilder.Setup(x => x.Resolve<TGrain>(FutureId))
                .Returns(MockFutureGrain.Object);

            FutureDeletedDtoBuilder = new TDtoBuilder()
                .With(x => x.Id, FutureDeletedId)
                .With(x => x.EntityState, EntityState.Deleted)
                .With(x => x.ModifiedAtTicks, FutureDeletedTime.Ticks);
            MockFutureDeletedGrain = new Mock<TGrain>();
            MockFutureDeletedGrain.Setup(x => x.GetDtoAsync())
                .ReturnsAsync(() => FutureDeletedDtoBuilder.Object);
            grainResolverMockBuilder.Setup(x => x.Resolve<TGrain>(FutureDeletedId))
                .Returns(MockFutureDeletedGrain.Object);

        }

        public Mock<TGrain> MockDistantPastArchivedGrain { get; }
        public Mock<TGrain> MockPastDraftGrain { get; }
        public Mock<TGrain> MockPastGrain { get; }
        public Mock<TGrain> MockPresentGrain { get; }
        public Mock<TGrain> MockFutureGrain { get; }
        public Mock<TGrain> MockFutureDeletedGrain { get; }


        public TDtoBuilder DistantPastDtoArchivedBuilder { get; }
        public TDtoBuilder PastDtoBuilder { get; }
        public TDtoBuilder PastDraftDtoBuilder { get; }
        public TDtoBuilder PresentDtoBuilder { get; }
        public TDtoBuilder FutureDtoBuilder { get; }
        public TDtoBuilder FutureDeletedDtoBuilder { get; }
    }

}