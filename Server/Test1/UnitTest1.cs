using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 
using Moq;
using Spectre.Console;
using Xunit;
using YouAre.Domain;
using YouAre.Helpers;

namespace YouAre.Tests
{
    public class UserTests
    {
        [Fact]
        public void User_WithValidData_ShouldPassValidation()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Username = "jack.wellery",
                Email = "jack.wellery@gmail.com",
                Password = "password1",
                Description = "Professional photographer."
            };

            // Act & Assert
            var validationContext = new ValidationContext(user);
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var isValid = Validator.TryValidateObject(user, validationContext, validationResults, true);

            // Assert
            Xunit.Assert.True(isValid);
            Xunit.Assert.Empty(validationResults);
        }

        [Xunit.Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void User_WithInvalidUsername_ShouldFailValidation(string invalidUsername)
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Username = invalidUsername,
                Email = "jack.wellery@gmail.com",
                Password = "password1",
                Description = "Professional photographer."
            };

            // Act & Assert
            var validationContext = new ValidationContext(user);
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var isValid = Validator.TryValidateObject(user, validationContext, validationResults, true);

            // Assert
            Xunit.Assert.False(isValid);
            Xunit.Assert.Single(validationResults, v => v.MemberNames.Contains(nameof(User.Username)));
        }

        // Add more tests for other properties and validation scenarios...

        [Fact]
        public void Logger_Information_ShouldPrintCorrectMessage()
        {
            // Arrange
            var mockConsole = new Mock<IAnsiConsole>();
            AnsiConsole.Console = mockConsole.Object;

            // Act
            Logger.Information("Test information message");

            // Assert
            mockConsole.Verify(c => c.Markup(It.Is<string>(s => s.Contains("[lightcyan1]"))), Times.Once);
            mockConsole.Verify(c => c.Markup(It.Is<string>(s => s.Contains("Test information message"))), Times.Once);
        }
    }
}
