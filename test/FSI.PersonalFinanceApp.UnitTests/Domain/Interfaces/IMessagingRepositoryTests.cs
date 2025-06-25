using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSI.PersonalFinanceApp.UnitTests.Domain.Interfaces
{
    public class IMessagingRepositoryTests
    {
        [Fact]
        public async Task Should_Add_MessagingEntity()
        {
            var mockRepo = new Mock<IMessagingRepository>();
            var message = new MessagingEntity("CREATE", "queue1", "{ id:1 }", false, "");

            mockRepo.Setup(r => r.AddAsync(message)).ReturnsAsync(1L);

            var result = await mockRepo.Object.AddAsync(message);

            result.Should().Be(1L);
            mockRepo.Verify(r => r.AddAsync(It.Is<MessagingEntity>(m => m.Action == "CREATE" && m.QueueName == "queue1")), Times.Once);
        }

        [Fact]
        public async Task Should_Get_All_Messages()
        {
            var mockRepo = new Mock<IMessagingRepository>();
            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<MessagingEntity>
            {
                new MessagingEntity("UPDATE", "queue2", "{}", true, ""),
                new MessagingEntity("DELETE", "queue3", "{}", false, "Error")
            });

            var result = await mockRepo.Object.GetAllAsync();

            result.Should().HaveCount(2);
            result.Should().Contain(m => m.QueueName == "queue3" && m.ErrorMessage == "Error");
        }

        [Fact]
        public void Should_Get_All_Messages_Sync()
        {
            var mockRepo = new Mock<IMessagingRepository>();
            mockRepo.Setup(r => r.GetAllSync()).Returns(new List<MessagingEntity>
            {
                new MessagingEntity("READ", "queue4", "{}", true, "")
            });

            var result = mockRepo.Object.GetAllSync();

            result.Should().ContainSingle(m => m.Action == "READ" && m.QueueName == "queue4");
        }
    }
}
