using FSI.PersonalFinanceApp.Domain.Entities;
using FluentAssertions;
using Xunit;
using System.Collections.Generic;

namespace FSI.PersonalFinanceApp.UnitTests.Domain.Entities
{
    public class IncomeCategoryEntityTests
    {
        [Fact]
        public void Should_Have_Null_Incomes_By_Default()
        {
            var category = new IncomeCategoryEntity();
            category.Incomes.Should().BeNull();
        }

        [Fact]
        public void Should_Set_And_Get_Incomes_Correctly()
        {
            var income1 = new IncomeEntity { Amount = 1500, Description = "Freelance" };
            var income2 = new IncomeEntity { Amount = 2500, Description = "Salário" };

            var incomes = new List<IncomeEntity> { income1, income2 };
            var category = new IncomeCategoryEntity
            {
                Name = "Rendimentos",
                Incomes = incomes
            };

            category.Name.Should().Be("Rendimentos");
            category.Incomes.Should().HaveCount(2);
            category.Incomes.Should().Contain(income1);
            category.Incomes.Should().Contain(income2);
        }
    }
}
