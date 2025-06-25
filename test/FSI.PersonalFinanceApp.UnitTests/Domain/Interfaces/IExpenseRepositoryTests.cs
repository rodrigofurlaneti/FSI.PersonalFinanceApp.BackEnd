using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSI.PersonalFinanceApp.UnitTests.Domain.Interfaces
{
    public class IExpenseRepositoryTests
    {
        [Fact]
        public async Task Should_Call_AddAsync_With_ExpenseEntity()
        {
            var mockRepo = new Mock<IExpenseRepository>();
            var expense = new ExpenseEntity { Amount = 99.99m, Description = "Internet" };

            mockRepo.Setup(r => r.AddAsync(expense)).ReturnsAsync(1L);

            var result = await mockRepo.Object.AddAsync(expense);

            result.Should().Be(1L);
            mockRepo.Verify(r => r.AddAsync(It.Is<ExpenseEntity>(e => e.Description == "Internet")), Times.Once);
        }

        [Fact]
        public async Task Should_Get_All_Ordered_Async()
        {
            var mockRepo = new Mock<IExpenseRepository>();
            var orderedList = new List<ExpenseEntity>
            {
                new ExpenseEntity { Amount = 10, Description = "Água" },
                new ExpenseEntity { Amount = 20, Description = "Energia" }
            };

            mockRepo.Setup(r => r.GetAllOrderedAsync("Amount", "asc")).ReturnsAsync(orderedList);

            var result = await mockRepo.Object.GetAllOrderedAsync("Amount", "asc");

            result.Should().HaveCount(2);
            result.Should().Contain(e => e.Description == "Água");
        }

        [Fact]
        public void Should_Get_All_Ordered_Sync()
        {
            var mockRepo = new Mock<IExpenseRepository>();
            var orderedList = new List<ExpenseEntity>
            {
                new ExpenseEntity { Amount = 200, Description = "Aluguel" },
                new ExpenseEntity { Amount = 100, Description = "Telefone" }
            };

            mockRepo.Setup(r => r.GetAllOrderedSync("Amount", "desc")).Returns(orderedList);

            var result = mockRepo.Object.GetAllOrderedSync("Amount", "desc");

            result.Should().HaveCount(2);
            result.Should().Contain(e => e.Description == "Aluguel");
        }
    }
}
