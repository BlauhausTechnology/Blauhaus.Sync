using System;
using System.Threading.Tasks;
using Blauhaus.Domain.Abstractions.DtoHandlers;
using Blauhaus.Domain.Abstractions.Entities;
using Blauhaus.Domain.Server.Entities;
using Blauhaus.Sync.Tests.TestObjects;

namespace Blauhaus.Sync.Tests.Server.TestObjects
{
    public class MyServerEntity : BaseServerEntity, IDtoOwner<MyDto>
    {
        public MyServerEntity()
        {
        }

        public MyServerEntity(string name, DateTime createdAt, Guid id, EntityState entityState = EntityState.Active) : base(createdAt, id, entityState)
        {
            Name = name;
        }

        public string Name { get; private set; } = null!;


        public Task<MyDto> GetDtoAsync()
        {
            return Task.FromResult(new MyDto
            {
                EntityState = EntityState,
                Id = Id,
                ModifiedAtTicks = ModifiedAt.Ticks,
                Name = Name
            });
        }
    }
}