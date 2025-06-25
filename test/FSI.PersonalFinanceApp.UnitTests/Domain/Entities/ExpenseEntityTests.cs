using FSI.PersonalFinanceApp.Domain.Entities;
using FluentAssertions;
using Xunit;
using System;
using System.Threading;

namespace FSI.PersonalFinanceApp.UnitTests.Domain.Entities
{
    public class ExpenseEntityTests
    {
        [Fact]
        public void Should_Set_And_Get_Properties_Correctly()
        {
            var expense = new ExpenseEntity
            {
                Amount = 120.50m,
                DueDate = new DateTime(2025, 12, 31),
                Description = "Aluguel",
                ExpenseCategoryId = 1
            };

            expense.Amount.Should().Be(120.50m);
            expense.DueDate.Should().Be(new DateTime(2025, 12, 31));
            expense.Description.Should().Be("Aluguel");
            expense.PaidAt.Should().BeNull();
            expense.ExpenseCategoryId.Should().Be(1);
        }

        [Fact]
        public void Should_Mark_As_Paid_And_Update_Timestamp()
        {
            var expense = new ExpenseEntity();
            var paymentDate = new DateTime(2025, 6, 25);

            var before = expense.UpdatedAt;
            Thread.Sleep(10);

            expense.MarkAsPaid(paymentDate);

            expense.PaidAt.Should().Be(paymentDate);
            expense.UpdatedAt.Should().BeAfter(before);
        }
    }
}
