using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using System.Threading.Tasks;

namespace FSI.PersonalFinanceApp.UnitTests.Domain.Interfaces
{
    public class IExpenseCategoryRepositoryTests
    {
        [Fact]
        public async Task Should_Call_AddAsync_With_ExpenseCategory()
        {
            var mockRepo = new Mock<IExpenseCategoryRepository>();
            var category = new ExpenseCategoryEntity { Name = "Alimentação" };

            mockRepo.Setup(r => r.AddAsync(category)).ReturnsAsync(1L);

            var result = await mockRepo.Object.AddAsync(category);

            result.Should().Be(1L);
            mockRepo.Verify(r => r.AddAsync(It.Is<ExpenseCategoryEntity>(c => c.Name == "Alimentação")), Times.Once);
        }

        [Fact]
        public async Task Should_Get_By_Id_Async()
        {
            var mockRepo = new Mock<IExpenseCategoryRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(new ExpenseCategoryEntity { Id = 10, Name = "Saúde" });

            var result = await mockRepo.Object.GetByIdAsync(10);

            result.Should().NotBeNull();
            result!.Id.Should().Be(10);
            result.Name.Should().Be("Saúde");
        }

        [Fact]
        public void Should_Get_All_Categories_Synchronously()
        {
            var mockRepo = new Mock<IExpenseCategoryRepository>();
            var expectedList = new List<ExpenseCategoryEntity>
            {
                new ExpenseCategoryEntity { Name = "Transporte" },
                new ExpenseCategoryEntity { Name = "Educação" }
            };

            mockRepo.Setup(r => r.GetAllSync()).Returns(expectedList);

            var result = mockRepo.Object.GetAllSync();

            result.Should().HaveCount(2);
        }
    }
}
