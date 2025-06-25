using FSI.PersonalFinanceApp.Domain.Entities;
using FluentAssertions;
using Xunit;
using System;

namespace FSI.PersonalFinanceApp.UnitTests.Domain.Entities
{
    public class AccountEntityTests
    {
        [Fact]
        public void Should_Have_Zero_Initial_Balance()
        {
            var account = new AccountEntity();
            account.Balance.Should().Be(0);
        }

        [Fact]
        public void Should_Set_And_Get_Description()
        {
            var account = new AccountEntity();
            account.Description = "Conta bancária principal";
            account.Description.Should().Be("Conta bancária principal");
        }

        [Fact]
        public void Should_Deposit_Amount_To_Balance()
        {
            var account = new AccountEntity();
            account.Deposit(100);
            account.Balance.Should().Be(100);
        }

        [Fact]
        public void Should_Withdraw_Amount_From_Balance()
        {
            var account = new AccountEntity();
            account.Deposit(200);
            account.Withdraw(50);
            account.Balance.Should().Be(150);
        }

        [Fact]
        public void Withdraw_With_Insufficient_Funds_Should_Throw()
        {
            var account = new AccountEntity();
            Action act = () => account.Withdraw(100);
            act.Should().Throw<InvalidOperationException>()
               .WithMessage("Insufficient funds.");
        }

        [Fact]
        public void Deposit_Should_Update_Timestamp()
        {
            var account = new AccountEntity();
            var before = account.UpdatedAt;
            System.Threading.Thread.Sleep(10);
            account.Deposit(10);
            account.UpdatedAt.Should().BeAfter(before);
        }

        [Fact]
        public void Withdraw_Should_Update_Timestamp()
        {
            var account = new AccountEntity();
            account.Deposit(100);
            var before = account.UpdatedAt;
            System.Threading.Thread.Sleep(10);
            account.Withdraw(50);
            account.UpdatedAt.Should().BeAfter(before);
        }
    }
}
