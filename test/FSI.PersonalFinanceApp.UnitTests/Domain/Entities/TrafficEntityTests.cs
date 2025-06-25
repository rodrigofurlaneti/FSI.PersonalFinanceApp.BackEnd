using FSI.PersonalFinanceApp.Domain.Entities;
using FluentAssertions;
using Xunit;
using System;

namespace FSI.PersonalFinanceApp.UnitTests.Domain.Entities
{
    public class TrafficEntityTests
    {
        [Fact]
        public void Should_Set_And_Get_Properties_Correctly()
        {
            var traffic = new TrafficEntity
            {
                Method = "POST",
                Action = "CreateExpense",
                BackEndCreatedAt = new DateTime(2025, 6, 25, 10, 0, 0)
            };

            traffic.Method.Should().Be("POST");
            traffic.Action.Should().Be("CreateExpense");
            traffic.BackEndCreatedAt.Should().Be(new DateTime(2025, 6, 25, 10, 0, 0));
        }

        [Fact]
        public void Should_Allow_Null_BackEndCreatedAt()
        {
            var traffic = new TrafficEntity
            {
                Method = "GET",
                Action = "FetchExpenses",
                BackEndCreatedAt = null
            };

            traffic.Method.Should().Be("GET");
            traffic.Action.Should().Be("FetchExpenses");
            traffic.BackEndCreatedAt.Should().BeNull();
        }
    }
}
