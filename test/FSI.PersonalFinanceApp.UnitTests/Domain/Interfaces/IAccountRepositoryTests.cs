using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using System.Threading.Tasks;

namespace FSI.PersonalFinanceApp.UnitTests.Domain.Interfaces
{
    public class IAccountRepositoryTests
    {
        [Fact]
        public async Task Should_Call_Add_Method_With_Valid_Account()
        {
            // Arrange
            var mockRepo = new Mock<IAccountRepository>();
            var account = new AccountEntity { Name = "Conta Teste" };

            // Act
            await mockRepo.Object.AddAsync(account);

            // Assert
            mockRepo.Verify(x => x.AddAsync(It.Is<AccountEntity>(a => a.Name == "Conta Teste")), Times.Once);
        }

        [Fact]
        public async Task Should_Call_GetByIdAsync_Method()
        {
            // Arrange
            var mockRepo = new Mock<IAccountRepository>();
            var expectedId = 1L;
            mockRepo.Setup(r => r.GetByIdAsync(expectedId))
                    .ReturnsAsync(new AccountEntity { Id = expectedId, Name = "Conta 1" });

            // Act
            var result = await mockRepo.Object.GetByIdAsync(expectedId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(expectedId);
            result.Name.Should().Be("Conta 1");
        }
    }
}