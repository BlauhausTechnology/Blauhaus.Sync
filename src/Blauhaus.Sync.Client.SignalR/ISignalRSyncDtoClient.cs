using System;
using Blauhaus.Domain.Abstractions.CommandHandlers;
using Blauhaus.Domain.Abstractions.Entities;
using Blauhaus.SignalR.Abstractions.Client;
using Blauhaus.Sync.Abstractions.Common;

namespace Blauhaus.Sync.Client.SignalR
{
    public interface ISignalRSyncDtoClient<TDto, TId> : ISignalRDtoClient<TDto>, ICommandHandler<DtoBatch<TDto, TId>, DtoSyncCommand> 
        where TDto : IClientEntity<TId> 
        where TId : IEquatable<TId>
    {
        
    }
}