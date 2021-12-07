using System;
using Blauhaus.Common.Abstractions;
using Blauhaus.Domain.Abstractions.CommandHandlers;
using Blauhaus.Domain.Abstractions.Entities;
using Blauhaus.SignalR.Abstractions.Client;
using Blauhaus.Sync.Abstractions.Client;
using Blauhaus.Sync.Abstractions.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Blauhaus.Sync.Client.SignalR.Ioc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSignalRSync<TDto, TId, TUser>(this IServiceCollection services)
            where TDto : class, IClientEntity<Guid>, IClientEntity<TId> 
            where TUser : IHasId<TId>
            where TId : IEquatable<TId>
        {
            
            services.TryAddSingleton<ISignalRSyncDtoClient<TDto, TId>, SignalRSyncDtoClient<TDto, TId, TUser>>();
            services.TryAddSingleton<ICommandHandler<DtoBatch<TDto, Guid>, DtoSyncCommand>>(sp => sp.GetRequiredService<ISignalRSyncDtoClient<TDto, Guid>>());
            
            //for the AppLifecycleManager to resolve
            services.AddSingleton<ISignalRDtoClient>(sp => sp.GetRequiredService<ISignalRSyncDtoClient<TDto, Guid>>());

            
            return services;
        }
    }
}