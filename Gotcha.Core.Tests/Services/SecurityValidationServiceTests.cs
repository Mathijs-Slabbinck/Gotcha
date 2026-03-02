using Gotcha.Core.Services.ValidationServices;

namespace Gotcha.Core.Tests.Services
{
    public class SecurityValidationServiceTests
    {
        #region IsValidIP

        [Fact]
        // "192.168.1.1" → should return true
        public void IsValidIP_ValidIP_ReturnsTrue()
        {
            // Act
            bool result = SecurityValidationService.IsValidIP("192.168.1.1");

            // Assert
            Assert.True(result);
        }

        [Fact]
        // "" → should return false
        public void IsValidIP_EmptyString_ReturnsFalse()
        {
            // Act
            bool result = SecurityValidationService.IsValidIP("");

            // Assert
            Assert.False(result);
        }

        [Fact]
        // null → should return false
        public void IsValidIP_Null_ReturnsFalse()
        {
            // Act
            bool result = SecurityValidationService.IsValidIP(null!);

            // Assert
            Assert.False(result);
        }

        [Fact]
        // "192.168.1" → should return false (only 3 octets)
        public void IsValidIP_TooFewOctets_ReturnsFalse()
        {
            // Act
            bool result = SecurityValidationService.IsValidIP("192.168.1");

            // Assert
            Assert.False(result);
        }

        [Fact]
        // "192.168.1.1.1" → should return false (5 octets)
        public void IsValidIP_TooManyOctets_ReturnsFalse()
        {
            // Act
            bool result = SecurityValidationService.IsValidIP("192.168.1.1.1");

            // Assert
            Assert.False(result);
        }

        [Fact]
        // "256.1.1.1" → should return false (octet > 255)
        public void IsValidIP_OctetOver255_ReturnsFalse()
        {
            // Act
            bool result = SecurityValidationService.IsValidIP("256.1.1.1");

            // Assert
            Assert.False(result);
        }

        [Fact]
        // "-1.1.1.1" → should return false (negative number)
        public void IsValidIP_NegativeOctet_ReturnsFalse()
        {
            // Act
            bool result = SecurityValidationService.IsValidIP("-1.1.1.1");

            // Assert
            Assert.False(result);
        }

        [Fact]
        // "01.1.1.1" → should return false (leading zero rejected unless exactly "0")
        public void IsValidIP_LeadingZeros_ReturnsFalse()
        {
            // Act
            bool result = SecurityValidationService.IsValidIP("01.1.1.1");

            // Assert
            Assert.False(result);
        }

        [Fact]
        // "0.0.0.0" → should return true (minimum boundary)
        public void IsValidIP_AllZeros_ReturnsTrue()
        {
            // Act
            bool result = SecurityValidationService.IsValidIP("0.0.0.0");

            // Assert
            Assert.True(result);
        }

        [Fact]
        // "255.255.255.255" → should return true (maximum boundary)
        public void IsValidIP_AllMax_ReturnsTrue()
        {
            // Act
            bool result = SecurityValidationService.IsValidIP("255.255.255.255");

            // Assert
            Assert.True(result);
        }

        #endregion
    }
}
