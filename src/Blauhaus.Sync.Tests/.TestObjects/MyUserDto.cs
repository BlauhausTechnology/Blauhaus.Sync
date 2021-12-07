using System;
using Blauhaus.Common.Abstractions;

namespace Blauhaus.Sync.Tests.TestObjects.User
{
    public class MyUserDto : MyDto, IHasUserId
    {
        public Guid UserId { get; set; }
    }
}