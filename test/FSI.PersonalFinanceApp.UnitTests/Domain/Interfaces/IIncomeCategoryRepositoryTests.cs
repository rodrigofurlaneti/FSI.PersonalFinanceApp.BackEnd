using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSI.PersonalFinanceApp.UnitTests.Domain.Interfaces
{
    public class IIncomeCategoryRepositoryTests
    {
        [Fact]
        public async Task Should_Add_IncomeCategoryEntity()
        {
            var mockRepo = new Mock<IIncomeCategoryRepository>();
            var category = new IncomeCategoryEntity { Name = "Salário" };

            mockRepo.Setup(r => r.AddAsync(category)).ReturnsAsync(1L);

            var result = await mockRepo.Object.AddAsync(category);

            result.Should().Be(1L);
            mockRepo.Verify(r => r.AddAsync(It.Is<IncomeCategoryEntity>(c => c.Name == "Salário")), Times.Once);
        }

        [Fact]
        public async Task Should_Get_All_IncomeCategories()
        {
            var mockRepo = new Mock<IIncomeCategoryRepository>();
            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<IncomeCategoryEntity>
            {
                new IncomeCategoryEntity { Name = "Freelance" },
                new IncomeCategoryEntity { Name = "Rendimentos" }
            });

            var result = await mockRepo.Object.GetAllAsync();

            result.Should().HaveCount(2);
            result.Should().Contain(c => c.Name == "Rendimentos");
        }

        [Fact]
        public void Should_Get_All_IncomeCategories_Sync()
        {
            var mockRepo = new Mock<IIncomeCategoryRepository>();
            mockRepo.Setup(r => r.GetAllSync()).Returns(new List<IncomeCategoryEntity>
            {
                new IncomeCategoryEntity { Name = "Dividendos" }
            });

            var result = mockRepo.Object.GetAllSync();

            result.Should().ContainSingle(c => c.Name == "Dividendos");
        }
    }
}
