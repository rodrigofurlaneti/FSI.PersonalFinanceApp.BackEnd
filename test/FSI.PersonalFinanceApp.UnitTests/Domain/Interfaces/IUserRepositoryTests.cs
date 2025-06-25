using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSI.PersonalFinanceApp.UnitTests.Domain.Interfaces
{
    public class IUserRepositoryTests
    {
        [Fact]
        public async Task Should_Add_UserEntity()
        {
            var mockRepo = new Mock<IUserRepository>();
            var user = new UserEntity { Email = "user@example.com", PasswordHash = "hashed123" };

            mockRepo.Setup(r => r.AddAsync(user)).ReturnsAsync(1L);

            var result = await mockRepo.Object.AddAsync(user);

            result.Should().Be(1L);
            mockRepo.Verify(r => r.AddAsync(It.Is<UserEntity>(u => u.Email == "user@example.com")), Times.Once);
        }

        [Fact]
        public async Task Should_Get_All_Users()
        {
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<UserEntity>
            {
                new UserEntity { Email = "a@example.com" },
                new UserEntity { Email = "b@example.com" }
            });

            var result = await mockRepo.Object.GetAllAsync();

            result.Should().HaveCount(2);
            result.Should().Contain(u => u.Email == "b@example.com");
        }

        [Fact]
        public void Should_Get_All_Users_Sync()
        {
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetAllSync()).Returns(new List<UserEntity>
            {
                new UserEntity { Email = "sync@example.com" }
            });

            var result = mockRepo.Object.GetAllSync();

            result.Should().ContainSingle(u => u.Email == "sync@example.com");
        }
    }
}
