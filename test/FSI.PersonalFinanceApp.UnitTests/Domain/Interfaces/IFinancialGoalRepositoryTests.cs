using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSI.PersonalFinanceApp.UnitTests.Domain.Interfaces
{
    public class IFinancialGoalRepositoryTests
    {
        [Fact]
        public async Task Should_Add_FinancialGoalEntity()
        {
            var mockRepo = new Mock<IFinancialGoalRepository>();
            var goal = new FinancialGoalEntity { Name = "Viagem", TargetAmount = 5000 };

            mockRepo.Setup(r => r.AddAsync(goal)).ReturnsAsync(1L);

            var result = await mockRepo.Object.AddAsync(goal);

            result.Should().Be(1L);
            mockRepo.Verify(r => r.AddAsync(It.Is<FinancialGoalEntity>(g => g.Name == "Viagem")), Times.Once);
        }

        [Fact]
        public async Task Should_Get_All_FinancialGoals()
        {
            var mockRepo = new Mock<IFinancialGoalRepository>();
            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<FinancialGoalEntity>
            {
                new FinancialGoalEntity { Name = "Reserva de Emergência" },
                new FinancialGoalEntity { Name = "Curso" }
            });

            var result = await mockRepo.Object.GetAllAsync();

            result.Should().HaveCount(2);
            result.Should().Contain(g => g.Name == "Curso");
        }

        [Fact]
        public void Should_Get_All_FinancialGoals_Sync()
        {
            var mockRepo = new Mock<IFinancialGoalRepository>();
            mockRepo.Setup(r => r.GetAllSync()).Returns(new List<FinancialGoalEntity>
            {
                new FinancialGoalEntity { Name = "Investimento" }
            });

            var result = mockRepo.Object.GetAllSync();

            result.Should().ContainSingle(g => g.Name == "Investimento");
        }
    }
}
