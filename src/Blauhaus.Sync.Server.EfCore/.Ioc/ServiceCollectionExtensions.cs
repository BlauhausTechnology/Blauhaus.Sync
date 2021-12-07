using System;
using System.Security.Cryptography;
using Blauhaus.Domain.Abstractions.Entities;
using Blauhaus.Sync.Abstractions.Common;
using Microsoft.Extensions.DependencyInjection;
using Blauhaus.Domain.Abstractions.CommandHandlers;

namespace Blauhaus.Sync.Server.EfCore.Ioc
{
    public static class ServiceCollectionExtensions
{
        public static IServiceCollection AddDtoSyncCommandHandler<TDto, THandler, TId, TUser>(this IServiceCollection services)
            where TDto : IClientEntity<TId> 
            where THandler : class, IAuthenticatedCommandHandler<DtoBatch<TDto, TId>, DtoSyncCommand, TUser>
            where TId : IEquatable<TId>
        {

            services.AddTransient<IAuthenticatedCommandHandler<DtoBatch<TDto, TId>, DtoSyncCommand, TUser>, THandler>();

            return services;
        }
    }
}