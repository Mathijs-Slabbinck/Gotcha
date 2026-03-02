using Gotcha.Core.Services.ValidationServices;

namespace Gotcha.Core.Tests.Services
{
    public class UserValidationServiceTests
    {
        #region IsValidEmail

        [Fact]
        // "user@example.com" → should return true
        public void IsValidEmail_ValidEmail_ReturnsTrue()
        {
            // Act
            bool result = UserValidationService.IsValidEmail("user@example.com");

            // Assert
            Assert.True(result);
        }

        [Fact]
        // "" → should return false
        public void IsValidEmail_EmptyString_ReturnsFalse()
        {
            // Act
            bool result = UserValidationService.IsValidEmail("");

            // Assert
            Assert.False(result);
        }

        [Fact]
        // null → should return false
        // Note: method signature is string (not string?), so test how it handles null
        public void IsValidEmail_Null_ReturnsFalse()
        {
            // Act
            bool result = UserValidationService.IsValidEmail(null!);

            // Assert
            Assert.False(result);
        }

        [Fact]
        // "   " (whitespace only) → should return false
        public void IsValidEmail_Whitespace_ReturnsFalse()
        {
            // Act
            bool result = UserValidationService.IsValidEmail("   ");

            // Assert
            Assert.False(result);
        }

        [Fact]
        // "userexample.com" (no @) → should return false
        public void IsValidEmail_MissingAtSign_ReturnsFalse()
        {
            // Act
            bool result = UserValidationService.IsValidEmail("userexample.com");

            // Assert
            Assert.False(result);
        }

        [Fact]
        // "user@" (no domain) → should return false
        public void IsValidEmail_MissingDomain_ReturnsFalse()
        {
            // Act
            bool result = UserValidationService.IsValidEmail("user@");

            // Assert
            Assert.False(result);
        }

        #endregion

        #region IsReservedUsername

        [Fact]
        // "admin" → should return true (exact match)
        public void IsReservedUsername_ReservedName_ReturnsTrue()
        {
            // Act
            bool result = UserValidationService.IsReservedUsername("admin");

            // Assert
            Assert.True(result);
        }

        [Fact]
        // "ADMIN" → should return true (case-insensitive via StringComparer.OrdinalIgnoreCase)
        public void IsReservedUsername_ReservedNameDifferentCase_ReturnsTrue()
        {
            // Act
            bool result = UserValidationService.IsReservedUsername("ADMIN");

            // Assert
            Assert.True(result);
        }

        [Fact]
        // "john" → should return false
        public void IsReservedUsername_NormalName_ReturnsFalse()
        {
            // Act
            bool result = UserValidationService.IsReservedUsername("john");

            // Assert
            Assert.False(result);
        }

        [Fact]
        // "" → should return false (empty string is not reserved)
        public void IsReservedUsername_EmptyString_ReturnsFalse()
        {
            // Act
            bool result = UserValidationService.IsReservedUsername("");

            // Assert
            Assert.False(result);
        }

        [Fact]
        // null → should return false
        public void IsReservedUsername_Null_ReturnsFalse()
        {
            // Act
            bool result = UserValidationService.IsReservedUsername(null!);

            // Assert
            Assert.False(result);
        }

        #endregion
    }
}
