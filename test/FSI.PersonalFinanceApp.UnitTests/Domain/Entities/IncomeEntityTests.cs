using FSI.PersonalFinanceApp.Domain.Entities;
using FluentAssertions;
using Xunit;
using System;
using System.Threading;

namespace FSI.PersonalFinanceApp.UnitTests.Domain.Entities
{
    public class IncomeEntityTests
    {
        [Fact]
        public void Should_Set_And_Get_Properties_Correctly()
        {
            var income = new IncomeEntity
            {
                Amount = 3500.75m,
                IncomeDate = new DateTime(2025, 7, 1),
                Description = "Salário",
                IncomeCategoryId = 2
            };

            income.Amount.Should().Be(3500.75m);
            income.IncomeDate.Should().Be(new DateTime(2025, 7, 1));
            income.Description.Should().Be("Salário");
            income.ReceivedAt.Should().BeNull();
            income.IncomeCategoryId.Should().Be(2);
        }

        [Fact]
        public void Should_Mark_As_Received_And_Update_Timestamp()
        {
            var income = new IncomeEntity();
            var receivedDate = new DateTime(2025, 6, 25);

            var before = income.UpdatedAt;
            Thread.Sleep(10);

            income.MarkAsReceived(receivedDate);

            income.ReceivedAt.Should().Be(receivedDate);
            income.UpdatedAt.Should().BeAfter(before);
        }
    }
}
