using System;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Abstractions.Extensions;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.Abstractions;
using Blauhaus.DeviceServices.Abstractions.Connectivity;
using Blauhaus.Domain.Abstractions.Entities;
using Blauhaus.Errors;
using Blauhaus.Responses;
using Blauhaus.SignalR.Abstractions.Client;
using Blauhaus.SignalR.Client.Clients.Base;
using Blauhaus.SignalR.Client.Connection.Proxy;
using Blauhaus.Sync.Abstractions.Client;
using Blauhaus.Sync.Abstractions.Common;
using Microsoft.Extensions.Logging;

namespace Blauhaus.Sync.Client.SignalR
{
    public class SignalRSyncDtoClient<TDto, TId, TUser> : BaseSignalRDtoClient<TDto,TId, ISyncDtoCache<TDto, TId, TUser>>, ISignalRSyncDtoClient<TDto, TId>
        where TDto : class, IClientEntity<TId> 
        where TId : IEquatable<TId>
        where TUser : IHasId<TId>
    {
        private readonly IAnalyticsContext _analyticsContext;

        public SignalRSyncDtoClient(
            IAnalyticsLogger<SignalRSyncDtoClient<TDto, TId, TUser>> logger, 
            IAnalyticsContext analyticsContext,
            IConnectivityService connectivityService,
            ISyncDtoCache<TDto, TId, TUser> syncDtoCache,
            ISignalRConnectionProxy connection) 
                : base(logger, analyticsContext, connectivityService, syncDtoCache, connection)
        {
            _analyticsContext = analyticsContext;
        }
         
        public async Task<Response<DtoBatch<TDto, TId>>> HandleAsync(DtoSyncCommand command)
        {
            if (!ConnectivityService.IsConnectedToInternet)
            {
                Logger.LogWarning("SignalR hub could not be invoked because there is no internet connection");
                return Response.Failure<DtoBatch<TDto, TId>>(SignalRErrors.NoInternet);
            }
            
            await Locker.WaitAsync();
            try
            {
                var result = await Connection.InvokeAsync<Response<DtoBatch<TDto, TId>>>($"Handle{typeof(TDto).Name}SyncCommandAsync", command, _analyticsContext.GetAllValues());

                if (result.IsFailure)
                {
                    return Response.Failure<DtoBatch<TDto, TId>>(result.Error);
                }
                
                Logger.LogTrace("Successfully handled DtoSyncCommand");

                var dtoBatch = result.Value;

                await DtoCache.SaveSyncedDtosAsync(dtoBatch);
                  
                foreach (var dto in dtoBatch.Dtos)
                {
                    await UpdateSubscribersAsync(dto); 
                }

                return Response.Success(dtoBatch);

            }
            catch (ErrorException errorException)
            {
                return Logger.LogErrorResponse<DtoBatch<TDto, TId>>(errorException.Error);
            }
            catch (Exception e)
            {
                return Logger.LogErrorResponse<DtoBatch<TDto, TId>>(SignalRErrors.InvocationFailure(e), e);
            }
            finally
            {
                Locker.Release();
            }
        }
    }
}