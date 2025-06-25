using FSI.PersonalFinanceApp.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace FSI.PersonalFinanceApp.UnitTests.Domain.Entities
{
    public class UserEntityTests
    {
        [Fact]
        public void Should_Set_And_Get_Email_And_PasswordHash()
        {
            var user = new UserEntity
            {
                Email = "user@example.com",
                PasswordHash = "hashed_password_123"
            };

            user.Email.Should().Be("user@example.com");
            user.PasswordHash.Should().Be("hashed_password_123");
        }

        [Fact]
        public void Email_And_PasswordHash_Should_Not_Be_Null_By_Default()
        {
            var user = new UserEntity();

            user.Email.Should().NotBeNull();
            user.PasswordHash.Should().NotBeNull();
            user.Email.Should().BeEmpty();
            user.PasswordHash.Should().BeEmpty();
        }
    }
}
