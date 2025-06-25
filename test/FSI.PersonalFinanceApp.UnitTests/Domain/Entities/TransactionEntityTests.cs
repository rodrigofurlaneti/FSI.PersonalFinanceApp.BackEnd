using FSI.PersonalFinanceApp.Domain.Entities;
using FluentAssertions;
using Xunit;
using System;

namespace FSI.PersonalFinanceApp.UnitTests.Domain.Entities
{
    public class TransactionEntityTests
    {
        [Fact]
        public void Should_Set_And_Get_All_Properties_Correctly()
        {
            var transaction = new TransactionEntity
            {
                AccountFromId = 1,
                AccountToId = 2,
                ExpenseId = 10,
                IncomeId = 20,
                TransactionDate = new DateTime(2025, 6, 25),
                Amount = 150.75m,
                Description = "Transferência entre contas"
            };

            transaction.AccountFromId.Should().Be(1);
            transaction.AccountToId.Should().Be(2);
            transaction.ExpenseId.Should().Be(10);
            transaction.IncomeId.Should().Be(20);
            transaction.TransactionDate.Should().Be(new DateTime(2025, 6, 25));
            transaction.Amount.Should().Be(150.75m);
            transaction.Description.Should().Be("Transferência entre contas");
        }

        [Fact]
        public void Should_Allow_Nullable_Ids()
        {
            var transaction = new TransactionEntity
            {
                AccountFromId = null,
                AccountToId = null,
                ExpenseId = null,
                IncomeId = null
            };

            transaction.AccountFromId.Should().BeNull();
            transaction.AccountToId.Should().BeNull();
            transaction.ExpenseId.Should().BeNull();
            transaction.IncomeId.Should().BeNull();
        }
    }
}
