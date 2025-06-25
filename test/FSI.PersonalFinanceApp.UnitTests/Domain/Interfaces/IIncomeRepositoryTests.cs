using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSI.PersonalFinanceApp.UnitTests.Domain.Interfaces
{
    public class IIncomeRepositoryTests
    {
        [Fact]
        public async Task Should_Add_IncomeEntity()
        {
            var mockRepo = new Mock<IIncomeRepository>();
            var income = new IncomeEntity { Amount = 3000, Description = "Salário Mensal" };

            mockRepo.Setup(r => r.AddAsync(income)).ReturnsAsync(1L);

            var result = await mockRepo.Object.AddAsync(income);

            result.Should().Be(1L);
            mockRepo.Verify(r => r.AddAsync(It.Is<IncomeEntity>(i => i.Description == "Salário Mensal")), Times.Once);
        }

        [Fact]
        public async Task Should_Get_All_Incomes()
        {
            var mockRepo = new Mock<IIncomeRepository>();
            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<IncomeEntity>
            {
                new IncomeEntity { Amount = 500, Description = "Freelance" },
                new IncomeEntity { Amount = 1200, Description = "Aluguel" }
            });

            var result = await mockRepo.Object.GetAllAsync();

            result.Should().HaveCount(2);
            result.Should().Contain(i => i.Description == "Freelance");
        }

        [Fact]
        public void Should_Get_All_Incomes_Sync()
        {
            var mockRepo = new Mock<IIncomeRepository>();
            mockRepo.Setup(r => r.GetAllSync()).Returns(new List<IncomeEntity>
            {
                new IncomeEntity { Amount = 150, Description = "Dividendos" }
            });

            var result = mockRepo.Object.GetAllSync();

            result.Should().ContainSingle(i => i.Description == "Dividendos");
        }
    }
}
