using Blauhaus.Domain.Abstractions.CommandHandlers;
using Blauhaus.Orleans.Abstractions.Grains;
using Blauhaus.Orleans.Abstractions.Handlers;
using Blauhaus.SignalR.Abstractions.Auth;
using System;
using System.Threading.Tasks;
using Blauhaus.Domain.Abstractions.Entities;
using Blauhaus.Sync.Abstractions.Common;
using Orleans.Concurrency;

namespace Blauhaus.Sync.Server.Orleans.Abstractions
{
    public interface IDtoSyncGrain<TDto> : IGrainSingleton,  IConnectedUserHandler,
        IAuthenticatedCommandHandler<DtoBatch<TDto, Guid>, DtoSyncCommand, IConnectedUser>
        where TDto : IClientEntity<Guid>  
    {
        Task SetBatchSizeAsync(int batchSize);

        [OneWay]
        Task UpdateDtoAsync(TDto dto);
    }
}