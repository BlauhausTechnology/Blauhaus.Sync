using System;
using Blauhaus.Domain.Abstractions.Entities;
using Blauhaus.Domain.TestHelpers.EntityBuilders;

namespace Blauhaus.Sync.TestHelpers.EfCore
{
    
    public class EntitySyncSet<TEntity, TEntityBuilder> 
        where TEntityBuilder : BaseServerEntityBuilder<TEntityBuilder, TEntity>
        where TEntity : class, IServerEntity
    {

        public EntitySyncSet(DateTime runTime)
        {
            DistantPastArchivedId = Guid.NewGuid();
            DistantPastArchivedTime = runTime.AddMinutes(-20);
            DistantPastArchivedEntityBuilder = ((TEntityBuilder)Activator.CreateInstance(typeof(TEntityBuilder), DistantPastArchivedTime)!)
                .With(x => x.Id, DistantPastArchivedId)
                .With(x => x.EntityState, EntityState.Archived);

            PastId = Guid.NewGuid();
            PastTime = runTime.AddMinutes(-10);
            PastEntityBuilder = ((TEntityBuilder)Activator.CreateInstance(typeof(TEntityBuilder), PastTime)!)
                .With(x => x.Id, PastId)
                .With(x => x.EntityState, EntityState.Active);
            
            PastDraftId = Guid.NewGuid();
            PastDraftTime = runTime.AddMinutes(-10);
            PastDraftEntityBuilder = ((TEntityBuilder)Activator.CreateInstance(typeof(TEntityBuilder), PastDraftTime)!)
                .With(x => x.Id, PastId)
                .With(x => x.EntityState, EntityState.Draft);
            
            PresentId = Guid.NewGuid();
            PresentTime = runTime;
            PresentEntityBuilder = ((TEntityBuilder)Activator.CreateInstance(typeof(TEntityBuilder), PresentTime)!)
                .With(x => x.Id, PresentId)
                .With(x => x.EntityState, EntityState.Active);

            FutureId = Guid.NewGuid();
            FutureTime = runTime.AddMinutes(10);
            FutureEntityBuilder = ((TEntityBuilder)Activator.CreateInstance(typeof(TEntityBuilder), FutureTime)!)
                .With(x => x.Id, FutureId)
                .With(x => x.EntityState, EntityState.Active);
            
            FutureDeletedId = Guid.NewGuid();
            FutureDeletedTime = runTime.AddMinutes(20);
            FutureDeletedEntityBuilder = ((TEntityBuilder)Activator.CreateInstance(typeof(TEntityBuilder), FutureDeletedTime)!)
                .With(x => x.Id, FutureDeletedId)
                .With(x => x.EntityState, EntityState.Deleted);

            Builders = new[]
            {
                DistantPastArchivedEntityBuilder,
                PastEntityBuilder, 
                PastDraftEntityBuilder,
                PresentEntityBuilder, 
                FutureEntityBuilder,
                FutureDeletedEntityBuilder,
            };
        }

        public TEntityBuilder[] Builders { get; }

        public Guid DistantPastArchivedId { get; }
        public DateTime DistantPastArchivedTime { get; }
        public TEntityBuilder DistantPastArchivedEntityBuilder { get; }
        
        public Guid PastId { get; }
        public DateTime PastTime { get; }
        public TEntityBuilder PastEntityBuilder { get; }

        public Guid PastDraftId { get; }
        public DateTime PastDraftTime { get; }
        public TEntityBuilder PastDraftEntityBuilder { get; }
        
        public Guid PresentId { get; }
        public DateTime PresentTime { get; }
        public TEntityBuilder PresentEntityBuilder{ get; }
        
        public Guid FutureId { get; }
        public DateTime FutureTime { get; }
        public TEntityBuilder FutureEntityBuilder { get; }

        public Guid FutureDeletedId { get; }
        public DateTime FutureDeletedTime { get; }
        public TEntityBuilder FutureDeletedEntityBuilder { get; }

    }
}