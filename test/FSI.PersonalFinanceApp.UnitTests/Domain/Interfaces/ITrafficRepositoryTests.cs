using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSI.PersonalFinanceApp.UnitTests.Domain.Interfaces
{
    public class ITrafficRepositoryTests
    {
        [Fact]
        public async Task Should_Add_TrafficEntity()
        {
            var mockRepo = new Mock<ITrafficRepository>();
            var traffic = new TrafficEntity { Method = "POST", Action = "/api/expense" };

            mockRepo.Setup(r => r.AddAsync(traffic)).ReturnsAsync(1L);

            var result = await mockRepo.Object.AddAsync(traffic);

            result.Should().Be(1L);
            mockRepo.Verify(r => r.AddAsync(It.Is<TrafficEntity>(t => t.Method == "POST")), Times.Once);
        }

        [Fact]
        public async Task Should_Get_All_TrafficAsync()
        {
            var mockRepo = new Mock<ITrafficRepository>();
            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<TrafficEntity>
            {
                new TrafficEntity { Method = "GET", Action = "/api/income" },
                new TrafficEntity { Method = "DELETE", Action = "/api/account" }
            });

            var result = await mockRepo.Object.GetAllAsync();

            result.Should().HaveCount(2);
            result.Should().Contain(t => t.Action == "/api/account");
        }

        [Fact]
        public void Should_Get_All_TrafficSync()
        {
            var mockRepo = new Mock<ITrafficRepository>();
            mockRepo.Setup(r => r.GetAllSync()).Returns(new List<TrafficEntity>
            {
                new TrafficEntity { Method = "PUT", Action = "/api/goal" }
            });

            var result = mockRepo.Object.GetAllSync();

            result.Should().ContainSingle(t => t.Method == "PUT");
        }
    }
}
