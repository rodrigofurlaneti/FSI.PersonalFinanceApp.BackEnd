using FSI.PersonalFinanceApp.Domain.Entities;
using FluentAssertions;
using Xunit;
using System.Collections.Generic;

namespace FSI.PersonalFinanceApp.UnitTests.Domain.Entities
{
    public class ExpenseCategoryEntityTests
    {
        [Fact]
        public void Should_Initialize_With_Empty_Expenses_Collection()
        {
            var category = new ExpenseCategoryEntity();

            category.Expenses.Should().BeNull(); // padrão é null se não instanciado no construtor
        }

        [Fact]
        public void Should_Set_And_Get_Expenses_Correctly()
        {
            var expense1 = new ExpenseEntity { Amount = 100, Description = "Energia" };
            var expense2 = new ExpenseEntity { Amount = 200, Description = "Internet" };

            var expenses = new List<ExpenseEntity> { expense1, expense2 };
            var category = new ExpenseCategoryEntity
            {
                Name = "Contas Fixas",
                Expenses = expenses
            };

            category.Name.Should().Be("Contas Fixas");
            category.Expenses.Should().HaveCount(2);
            category.Expenses.Should().Contain(expense1);
            category.Expenses.Should().Contain(expense2);
        }
    }
}
