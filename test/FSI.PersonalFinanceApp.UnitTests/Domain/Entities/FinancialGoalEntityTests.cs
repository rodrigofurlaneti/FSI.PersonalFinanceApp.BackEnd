using FSI.PersonalFinanceApp.Domain.Entities;
using FluentAssertions;
using Xunit;
using System;
using System.Threading;

namespace FSI.PersonalFinanceApp.UnitTests.Domain.Entities
{
    public class FinancialGoalEntityTests
    {
        [Fact]
        public void Should_Set_And_Get_Properties_Correctly()
        {
            var goal = new FinancialGoalEntity
            {
                TargetAmount = 1000,
                CurrentAmount = 100,
                DueDate = new DateTime(2025, 12, 31),
                Description = "Viagem"
            };

            goal.TargetAmount.Should().Be(1000);
            goal.CurrentAmount.Should().Be(100);
            goal.DueDate.Should().Be(new DateTime(2025, 12, 31));
            goal.Description.Should().Be("Viagem");
            goal.IsCompleted.Should().BeFalse();
        }

        [Fact]
        public void Should_Update_CurrentAmount_When_Progress_Is_Updated()
        {
            var goal = new FinancialGoalEntity { TargetAmount = 1000 };
            goal.UpdateProgress(250);
            goal.CurrentAmount.Should().Be(250);
            goal.IsCompleted.Should().BeFalse();
        }

        [Fact]
        public void Should_Mark_As_Completed_When_Target_Reached()
        {
            var goal = new FinancialGoalEntity { TargetAmount = 1000 };
            goal.UpdateProgress(1000);
            goal.CurrentAmount.Should().Be(1000);
            goal.IsCompleted.Should().BeTrue();
        }

        [Fact]
        public void MarkAsCompleted_Should_Set_IsCompleted_And_UpdateTimestamp()
        {
            var goal = new FinancialGoalEntity();
            var before = goal.UpdatedAt;
            Thread.Sleep(10);
            goal.MarkAsCompleted();
            goal.IsCompleted.Should().BeTrue();
            goal.UpdatedAt.Should().BeAfter(before);
        }

        [Fact]
        public void UpdateProgress_Should_Update_Timestamp()
        {
            var goal = new FinancialGoalEntity { TargetAmount = 500 };
            var before = goal.UpdatedAt;
            Thread.Sleep(10);
            goal.UpdateProgress(100);
            goal.UpdatedAt.Should().BeAfter(before);
        }
    }
}
