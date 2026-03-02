using Gotcha.Core.Services.ValidationServices;

namespace Gotcha.Core.Tests.Services
{
    public class ImageValidationServiceTests
    {
        #region IsAllowedImageUrl

        [Fact]
        // "https://example.com/image.png" → should return true
        public void IsAllowedImageUrl_ValidHttps_ReturnsTrue()
        {
            // Act
            bool result = ImageValidationService.IsAllowedImageUrl("https://example.com/image.png");

            // Assert
            Assert.True(result);
        }

        [Fact]
        // "http://example.com/image.png" → should return true
        public void IsAllowedImageUrl_ValidHttp_ReturnsTrue()
        {
            // Act
            bool result = ImageValidationService.IsAllowedImageUrl("http://example.com/image.png");

            // Assert
            Assert.True(result);
        }

        [Fact]
        // null → should return true (null is allowed — profile image is optional)
        public void IsAllowedImageUrl_Null_ReturnsTrue()
        {
            // Act
            bool result = ImageValidationService.IsAllowedImageUrl(null);

            // Assert
            Assert.True(result);
        }

        [Fact]
        // "" → should return true (empty is allowed)
        public void IsAllowedImageUrl_EmptyString_ReturnsTrue()
        {
            // Act
            bool result = ImageValidationService.IsAllowedImageUrl("");

            // Assert
            Assert.True(result);
        }

        [Fact]
        // "ftp://example.com/file.png" → should return false (only http/https allowed)
        public void IsAllowedImageUrl_FtpScheme_ReturnsFalse()
        {
            // Act
            bool result = ImageValidationService.IsAllowedImageUrl("ftp://example.com/file.png");

            // Assert
            Assert.False(result);
        }

        [Fact]
        // "/images/photo.png" → should return false (relative URL, not absolute)
        public void IsAllowedImageUrl_RelativeUrl_ReturnsFalse()
        {
            // Act
            bool result = ImageValidationService.IsAllowedImageUrl("/images/photo.png");

            // Assert
            Assert.False(result);
        }

        [Fact]
        // "not-a-url" → should return false (Uri.TryCreate fails)
        public void IsAllowedImageUrl_InvalidString_ReturnsFalse()
        {
            // Act
            bool result = ImageValidationService.IsAllowedImageUrl("not-a-url");

            // Assert
            Assert.False(result);
        }

        #endregion
    }
}
