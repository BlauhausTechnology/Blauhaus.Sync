﻿using System;
using System.Collections.Generic;
using System.Linq;
using Blauhaus.Sync.Abstractions;
using Blauhaus.Sync.Abstractions.Client;
using Newtonsoft.Json;

namespace Blauhaus.Sync.Client
{
    public class OverallSyncStatus : IOverallSyncStatus
    {
        [JsonConstructor]
        public OverallSyncStatus(Dictionary<string, DtoSyncStatus>? dtoStatuses = null)
        {
            DtoStatuses = new Dictionary<string, DtoSyncStatus>();
            if (dtoStatuses != null)
            {
                foreach (var dtoStatus in dtoStatuses)
                {
                    DtoStatuses[dtoStatus.Key] = dtoStatus.Value;
                }
            }
        }

        public OverallSyncStatus Update(DtoSyncStatus newDtoSyncStatus)
        {
            return new OverallSyncStatus(new Dictionary<string, DtoSyncStatus>(DtoStatuses)
            {
                [newDtoSyncStatus.DtoName] = newDtoSyncStatus
            });
        }
        
        public Dictionary<string, DtoSyncStatus> DtoStatuses { get; }
         
        public int DownloadedDtoCount => DtoStatuses.Values.Sum(x => x.DownloadedDtoCount);
        public int TotalDtoCount => DtoStatuses.Values.Sum(x => x.TotalDtoCount);
        public float Progress => DownloadedDtoCount / (float)TotalDtoCount;

        
        public override string ToString()
        {
            return $"{DownloadedDtoCount} / {TotalDtoCount} [{Math.Round(Progress*100, 1)} %]";
        }
    }
}