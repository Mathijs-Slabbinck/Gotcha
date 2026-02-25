using Gotcha.Core.Services.ValidationServices;

namespace Gotcha.Core.Tests.Services
{
    public class ValidationServiceTests
    {
        #region IsValidEmail

        [Fact]
        // "user@example.com" → should return true
        public void IsValidEmail_ValidEmail_ReturnsTrue()
        {
            // Act
            bool result = LastLineValidationService.IsValidEmail("user@example.com");

            // Assert
            Assert.True(result);
        }

        [Fact]
        // "" → should return false
        public void IsValidEmail_EmptyString_ReturnsFalse()
        {
            // Act
            bool result = LastLineValidationService.IsValidEmail("");

            // Assert
            Assert.False(result);
        }

        [Fact]
        // null → should return false
        // Note: method signature is string (not string?), so test how it handles null
        public void IsValidEmail_Null_ReturnsFalse()
        {
            // Act
            bool result = LastLineValidationService.IsValidEmail(null!);

            // Assert
            Assert.False(result);
        }

        [Fact]
        // "   " (whitespace only) → should return false
        public void IsValidEmail_Whitespace_ReturnsFalse()
        {
            // Act
            bool result = LastLineValidationService.IsValidEmail("   ");

            // Assert
            Assert.False(result);
        }

        [Fact]
        // "userexample.com" (no @) → should return false
        public void IsValidEmail_MissingAtSign_ReturnsFalse()
        {
            // Act
            bool result = LastLineValidationService.IsValidEmail("userexample.com");

            // Assert
            Assert.False(result);
        }

        [Fact]
        // "user@" (no domain) → should return false
        public void IsValidEmail_MissingDomain_ReturnsFalse()
        {
            // Act
            bool result = LastLineValidationService.IsValidEmail("user@");

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
            bool result = LastLineValidationService.IsReservedUsername("admin");

            // Assert
            Assert.True(result);
        }

        [Fact]
        // "ADMIN" → should return true (case-insensitive via StringComparer.OrdinalIgnoreCase)
        public void IsReservedUsername_ReservedNameDifferentCase_ReturnsTrue()
        {
            // Act
            bool result = LastLineValidationService.IsReservedUsername("ADMIN");

            // Assert
            Assert.True(result);
        }

        [Fact]
        // "john" → should return false
        public void IsReservedUsername_NormalName_ReturnsFalse()
        {
            // Act
            bool result = LastLineValidationService.IsReservedUsername("john");

            // Assert
            Assert.False(result);
        }

        [Fact]
        // "" → should return false (empty string is not reserved)
        public void IsReservedUsername_EmptyString_ReturnsFalse()
        {
            // Act
            bool result = LastLineValidationService.IsReservedUsername("");

            // Assert
            Assert.False(result);
        }

        [Fact]
        // null → should return false
        public void IsReservedUsername_Null_ReturnsFalse()
        {
            // Act
            bool result = LastLineValidationService.IsReservedUsername(null!);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region IsAllowedImageUrl

        [Fact]
        // "https://example.com/image.png" → should return true
        public void IsAllowedImageUrl_ValidHttps_ReturnsTrue()
        {
            // Act
            bool result = LastLineValidationService.IsAllowedImageUrl("https://example.com/image.png");

            // Assert
            Assert.True(result);
        }

        [Fact]
        // "http://example.com/image.png" → should return true
        public void IsAllowedImageUrl_ValidHttp_ReturnsTrue()
        {
            // Act
            bool result = LastLineValidationService.IsAllowedImageUrl("http://example.com/image.png");

            // Assert
            Assert.True(result);
        }

        [Fact]
        // null → should return true (null is allowed — profile image is optional)
        public void IsAllowedImageUrl_Null_ReturnsTrue()
        {
            // Act
            bool result = LastLineValidationService.IsAllowedImageUrl(null);

            // Assert
            Assert.True(result);
        }

        [Fact]
        // "" → should return true (empty is allowed)
        public void IsAllowedImageUrl_EmptyString_ReturnsTrue()
        {
            // Act
            bool result = LastLineValidationService.IsAllowedImageUrl("");

            // Assert
            Assert.True(result);
        }

        [Fact]
        // "ftp://example.com/file.png" → should return false (only http/https allowed)
        public void IsAllowedImageUrl_FtpScheme_ReturnsFalse()
        {
            // Act
            bool result = LastLineValidationService.IsAllowedImageUrl("ftp://example.com/file.png");

            // Assert
            Assert.False(result);
        }

        [Fact]
        // "/images/photo.png" → should return false (relative URL, not absolute)
        public void IsAllowedImageUrl_RelativeUrl_ReturnsFalse()
        {
            // Act
            bool result = LastLineValidationService.IsAllowedImageUrl("/images/photo.png");

            // Assert
            Assert.False(result);
        }

        [Fact]
        // "not-a-url" → should return false (Uri.TryCreate fails)
        public void IsAllowedImageUrl_InvalidString_ReturnsFalse()
        {
            // Act
            bool result = LastLineValidationService.IsAllowedImageUrl("not-a-url");

            // Assert
            Assert.False(result);
        }

        #endregion

        #region IsValidIP

        [Fact]
        // "192.168.1.1" → should return true
        public void IsValidIP_ValidIP_ReturnsTrue()
        {
            // Act
            bool result = LastLineValidationService.IsValidIP("192.168.1.1");

            // Assert
            Assert.True(result);
        }

        [Fact]
        // "" → should return false
        public void IsValidIP_EmptyString_ReturnsFalse()
        {
            // Act
            bool result = LastLineValidationService.IsValidIP("");

            // Assert
            Assert.False(result);
        }

        [Fact]
        // null → should return false
        public void IsValidIP_Null_ReturnsFalse()
        {
            // Act
            bool result = LastLineValidationService.IsValidIP(null!);

            // Assert
            Assert.False(result);
        }

        [Fact]
        // "192.168.1" → should return false (only 3 octets)
        public void IsValidIP_TooFewOctets_ReturnsFalse()
        {
            // Act
            bool result = LastLineValidationService.IsValidIP("192.168.1");

            // Assert
            Assert.False(result);
        }

        [Fact]
        // "192.168.1.1.1" → should return false (5 octets)
        public void IsValidIP_TooManyOctets_ReturnsFalse()
        {
            // Act
            bool result = LastLineValidationService.IsValidIP("192.168.1.1.1");

            // Assert
            Assert.False(result);
        }

        [Fact]
        // "256.1.1.1" → should return false (octet > 255)
        public void IsValidIP_OctetOver255_ReturnsFalse()
        {
            // Act
            bool result = LastLineValidationService.IsValidIP("256.1.1.1");

            // Assert
            Assert.False(result);
        }

        [Fact]
        // "-1.1.1.1" → should return false (negative number)
        public void IsValidIP_NegativeOctet_ReturnsFalse()
        {
            // Act
            bool result = LastLineValidationService.IsValidIP("-1.1.1.1");

            // Assert
            Assert.False(result);
        }

        [Fact]
        // "01.1.1.1" → should return false (leading zero rejected unless exactly "0")
        public void IsValidIP_LeadingZeros_ReturnsFalse()
        {
            // Act
            bool result = LastLineValidationService.IsValidIP("01.1.1.1");

            // Assert
            Assert.False(result);
        }

        [Fact]
        // "0.0.0.0" → should return true (minimum boundary)
        public void IsValidIP_AllZeros_ReturnsTrue()
        {
            // Act
            bool result = LastLineValidationService.IsValidIP("0.0.0.0");

            // Assert
            Assert.True(result);
        }

        [Fact]
        // "255.255.255.255" → should return true (maximum boundary)
        public void IsValidIP_AllMax_ReturnsTrue()
        {
            // Act
            bool result = LastLineValidationService.IsValidIP("255.255.255.255");

            // Assert
            Assert.True(result);
        }

        #endregion
    }
}
