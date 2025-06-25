using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace FSI.PersonalFinanceApp.UnitTests.Domain.Interfaces
{
    public class ITransactionRepositoryTests
    {
        [Fact]
        public async Task Should_Add_TransactionEntity()
        {
            var mockRepo = new Mock<ITransactionRepository>();
            var transaction = new TransactionEntity
            {
                Amount = 100,
                Description = "Transferência",
                TransactionDate = DateTime.UtcNow
            };

            mockRepo.Setup(r => r.AddAsync(transaction)).ReturnsAsync(1L);

            var result = await mockRepo.Object.AddAsync(transaction);

            result.Should().Be(1L);
            mockRepo.Verify(r => r.AddAsync(It.Is<TransactionEntity>(t => t.Amount == 100)), Times.Once);
        }

        [Fact]
        public async Task Should_Get_All_Transactions()
        {
            var mockRepo = new Mock<ITransactionRepository>();
            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<TransactionEntity>
            {
                new TransactionEntity { Amount = 200, Description = "Pagamento Conta" },
                new TransactionEntity { Amount = 150, Description = "Cartão Crédito" }
            });

            var result = await mockRepo.Object.GetAllAsync();

            result.Should().HaveCount(2);
            result.Should().Contain(t => t.Description == "Cartão Crédito");
        }

        [Fact]
        public void Should_Get_All_Transactions_Sync()
        {
            var mockRepo = new Mock<ITransactionRepository>();
            mockRepo.Setup(r => r.GetAllSync()).Returns(new List<TransactionEntity>
            {
                new TransactionEntity { Amount = 50, Description = "Pix" }
            });

            var result = mockRepo.Object.GetAllSync();

            result.Should().ContainSingle(t => t.Description == "Pix");
        }
    }
}
