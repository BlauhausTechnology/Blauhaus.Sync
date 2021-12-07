using System;
using System.Collections.Generic;
using System.Linq;
using Blauhaus.Domain.Abstractions.Entities;
using Blauhaus.Sync.Abstractions.Common;

namespace Blauhaus.Sync.TestHelpers.EfCore
{

    public class SyncTestResult<TDto,TId> 
        where TId : IEquatable<TId> 
        where TDto : IClientEntity<TId>
    {
        public SyncTestResult(List<DtoBatch<TDto, TId>> dtoBatches)
        {
            DtoBatches = dtoBatches;
            Dtos = dtoBatches.SelectMany(x => x.Dtos).ToList();
        }

        public List<DtoBatch<TDto, TId>> DtoBatches { get; }
        public List<TDto> Dtos { get; }
    }
     
}