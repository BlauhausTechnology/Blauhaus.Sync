﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blauhaus.Sync.Tests.Client.SyncDtoCacheTests.Base;
using Blauhaus.Sync.Tests.TestObjects;
using NUnit.Framework;

namespace Blauhaus.Sync.Tests.Client.SyncDtoCacheTests
{
    public class HandleAsyncTests : BaseSyncDtoCacheTest
    {
        [Test]
        public async Task SHOULD_add_Dto_to_Cache()
        {
            //Act
            await Sut.HandleAsync(DtoOne);
            
            //Assert
            Assert.That(await Sut.GetOneAsync(DtoOne.Id), Is.Not.Null);
        }

        [Test]
        public async Task SHOULD_populate_extra_properties()
        {
            //Act
            await Sut.HandleAsync(DtoOne);
            
            //Assert
            var dtpOne = await Sut.GetAllAsync();
            Assert.That(dtpOne.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task SHOULD_not_add_duplicate()
        {
            //Act
            await Sut.HandleAsync(DtoOne);
            await Sut.HandleAsync(DtoOne);
            
            //Assert
            var all = await Sut.GetAllAsync();
            Assert.That(all.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task SHOULD_update_subscribers_to_that_id()
        {
            //Arrange
            var publishedDtos = new List<MyDto>();
            await Sut.SubscribeAsync(d =>
            {
                publishedDtos.Add(d);
                return Task.CompletedTask;
            }, dto => dto.Id == DtoOne.Id);
            
            //Act
            await Sut.HandleAsync(DtoOne);
            
            //Assert
            Assert.That(publishedDtos.Count, Is.EqualTo(1));
            Assert.That(publishedDtos.First(), Is.EqualTo(DtoOne));
        }

        [Test]
        public async Task SHOULD_NOT_update_subscribers_to_different_id()
        {
            //Arrange
            var publishedDtos = new List<MyDto>();
            await Sut.SubscribeAsync(d =>
            {
                publishedDtos.Add(d);
                return Task.CompletedTask;
            }, dto => dto.Id == DtoOne.Id);
            
            //Act
            await Sut.HandleAsync(DtoThree);
            
            //Assert
            Assert.That(publishedDtos.Count, Is.EqualTo(0));
        }
    }
}