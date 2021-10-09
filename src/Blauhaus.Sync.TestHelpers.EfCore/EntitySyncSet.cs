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
            DistantPastId = Guid.NewGuid();
            DistantPastTime = runTime.AddMinutes(-20);
            DistantPastEntityBuilder = ((TEntityBuilder)Activator.CreateInstance(typeof(TEntityBuilder), DistantPastTime)!)
                .With(x => x.Id, DistantPastId)
                .With(x => x.EntityState, EntityState.Active);

            PastId = Guid.NewGuid();
            PastTime = runTime.AddMinutes(-10);
            PastEntityBuilder = ((TEntityBuilder)Activator.CreateInstance(typeof(TEntityBuilder), PastTime)!)
                .With(x => x.Id, PastId)
                .With(x => x.EntityState, EntityState.Active);
            
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
                DistantPastEntityBuilder,
                PastEntityBuilder, 
                PresentEntityBuilder, 
                FutureEntityBuilder,
                FutureDeletedEntityBuilder,
            };
        }

        public TEntityBuilder[] Builders { get; }

        public Guid DistantPastId { get; }
        public DateTime DistantPastTime { get; }
        public TEntityBuilder DistantPastEntityBuilder { get; }
        
        public Guid PastId { get; }
        public DateTime PastTime { get; }
        public TEntityBuilder PastEntityBuilder { get; }
        
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