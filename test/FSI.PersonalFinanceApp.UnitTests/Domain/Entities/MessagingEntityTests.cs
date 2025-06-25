using FSI.PersonalFinanceApp.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace FSI.PersonalFinanceApp.UnitTests.Domain.Entities
{
    public class MessagingEntityTests
    {
        [Fact]
        public void Should_Set_And_Get_Default_Properties_Correctly()
        {
            var entity = new MessagingEntity();

            entity.Action.Should().BeEmpty();
            entity.QueueName.Should().BeEmpty();
            entity.MessageRequest.Should().BeEmpty();
            entity.MessageResponse.Should().BeEmpty();
            entity.IsProcessed.Should().BeFalse();
            entity.ErrorMessage.Should().BeEmpty();
        }

        [Fact]
        public void Should_Initialize_Using_First_Constructor()
        {
            var entity = new MessagingEntity("CreateExpense", "expense-queue", "{json}", true, "");

            entity.Action.Should().Be("CreateExpense");
            entity.QueueName.Should().Be("expense-queue");
            entity.MessageRequest.Should().Be("{json}");
            entity.MessageResponse.Should().BeEmpty();
            entity.IsProcessed.Should().BeTrue();
            entity.ErrorMessage.Should().BeEmpty();
        }

        [Fact]
        public void Should_Initialize_Using_Second_Constructor()
        {
            var entity = new MessagingEntity("CreateIncome", "income-queue", "{request}", "{response}", true, "none");

            entity.Action.Should().Be("CreateIncome");
            entity.QueueName.Should().Be("income-queue");
            entity.MessageRequest.Should().Be("{request}");
            entity.MessageResponse.Should().Be("{response}");
            entity.IsProcessed.Should().BeTrue();
            entity.ErrorMessage.Should().Be("none");
        }
    }
}
