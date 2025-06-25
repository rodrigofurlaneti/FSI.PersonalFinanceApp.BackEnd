using FSI.PersonalFinanceApp.Domain.Entities;
using FluentAssertions;
using Xunit;
using System;
using System.Threading;

namespace FSI.PersonalFinanceApp.UnitTests.Domain.Entities
{
    public class TestEntity : BaseEntity { }

    public class BaseEntityTests
    {
        [Fact]
        public void Should_Have_Default_Values_On_Creation()
        {
            var entity = new TestEntity();

            entity.Id.Should().Be(0);
            entity.Name.Should().BeEmpty();
            entity.IsActive.Should().BeTrue();
            entity.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            entity.UpdatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void Should_Deactivate_Entity()
        {
            var entity = new TestEntity();
            entity.Deactivate();
            entity.IsActive.Should().BeFalse();
        }

        [Fact]
        public void Should_Activate_Entity()
        {
            var entity = new TestEntity();
            entity.Deactivate(); // garantir que esteja false
            entity.Activate();
            entity.IsActive.Should().BeTrue();
        }

        [Fact]
        public void Should_Update_UpdatedAt_When_UpdateTimestamp_Called()
        {
            var entity = new TestEntity();
            var before = entity.UpdatedAt;
            Thread.Sleep(10); // garantir diferença perceptível
            entity.UpdateTimestamp();
            entity.UpdatedAt.Should().BeAfter(before);
        }
    }
}
