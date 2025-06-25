using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using System.Threading.Tasks;

namespace FSI.PersonalFinanceApp.UnitTests.Domain.Interfaces
{
    public class IBaseRepositoryTests
    {
        [Fact]
        public async Task Should_Call_AddAsync_And_Return_Id()
        {
            var mockRepo = new Mock<IBaseRepository<AccountEntity>>();
            mockRepo.Setup(r => r.AddAsync(It.IsAny<AccountEntity>())).ReturnsAsync(42L);

            var result = await mockRepo.Object.AddAsync(new AccountEntity { Name = "Conta XP" });

            result.Should().Be(42L);
            mockRepo.Verify(r => r.AddAsync(It.Is<AccountEntity>(a => a.Name == "Conta XP")), Times.Once);
        }

        [Fact]
        public void Should_Call_GetAllSync_And_Return_List()
        {
            var expected = new List<AccountEntity>
            {
                new AccountEntity { Id = 1, Name = "Conta 1" },
                new AccountEntity { Id = 2, Name = "Conta 2" }
            };

            var mockRepo = new Mock<IBaseRepository<AccountEntity>>();
            mockRepo.Setup(r => r.GetAllSync()).Returns(expected);

            var result = mockRepo.Object.GetAllSync();

            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task Should_Call_GetAllFilteredAsync()
        {
            var mockRepo = new Mock<IBaseRepository<AccountEntity>>();
            mockRepo.Setup(r => r.GetAllFilteredAsync("Name", "XP"))
                    .ReturnsAsync(new List<AccountEntity> { new AccountEntity { Name = "XP" } });

            var result = await mockRepo.Object.GetAllFilteredAsync("Name", "XP");

            result.Should().ContainSingle(a => a.Name == "XP");
        }
    }
}