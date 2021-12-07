using System;
using Blauhaus.Common.Abstractions;

namespace Blauhaus.Sync.Tests.Client.TestObjects
{
    public class MyTestUser : IHasId<Guid>
    {
        public MyTestUser()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
    }
}