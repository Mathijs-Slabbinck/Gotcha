using Gotcha.Core.Entities.Logging.LogEntities;
using Gotcha.Core.Enums;
using Gotcha.Core.Services.ResultModel;

namespace Gotcha.Core.Tests.Services
{
    public class ResultModelTests
    {
        [Fact]
        // New ResultModel<string> with no errors added → HasErrors should be false
        public void HasErrors_NoErrors_ReturnsFalse()
        {
            // Arrange
            ResultModel<string> result = new ResultModel<string>();

            // Assert
            Assert.False(result.HasErrors);
        }

        [Fact]
        // Add an Error to Errors list → HasErrors should be true
        // Use: new Error(LogSubTypes.Error_Other, "test error")
        public void HasErrors_WithErrors_ReturnsTrue()
        {
            // Arrange
            ResultModel<string> result = new ResultModel<string>();
            Error error = new Error(LogSubTypes.Error_Other, "test error");

            // Act
            result.Errors.Add(error);

            // Assert
            Assert.True(result.HasErrors);
        }

        [Fact]
        // No errors → Success should be true (Success = !HasErrors)
        public void Success_NoErrors_ReturnsTrue()
        {
            // Arrange
            ResultModel<string> result = new ResultModel<string>();

            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        // With errors → Success should be false
        public void Success_WithErrors_ReturnsFalse()
        {
            // Arrange
            ResultModel<string> result = new ResultModel<string>();
            Error error = new Error(LogSubTypes.Error_Other, "test error");

            // Act
            result.Errors.Add(error);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        // No errors AND no warnings → FullSuccess should be true
        public void FullSuccess_NoErrorsNoWarnings_ReturnsTrue()
        {
            // Arrange
            ResultModel<string> result = new ResultModel<string>();

            // Assert
            Assert.True(result.FullSuccess);
        }

        [Fact]
        // No errors but has warnings → FullSuccess should be false
        // (Success would be true, but FullSuccess requires no warnings either)
        public void FullSuccess_WithWarningsOnly_ReturnsFalse()
        {
            // Arrange
            ResultModel<string> result = new ResultModel<string>();
            Warning warning = new Warning(LogSubTypes.Warning_Other, "test warning");

            // Act
            result.Warnings.Add(warning);

            // Assert
            Assert.True(result.Success);
            Assert.False(result.FullSuccess);
        }

        [Fact]
        // Setting Data to an empty List<string> should auto-add a Warning
        // (CheckIfEmptyList triggers when value is ICollection with Count == 0)
        // Check: result.HasWarnings == true, result.Warnings.Count == 1
        public void Data_EmptyCollection_TriggersWarning()
        {
            // Arrange
            ResultModel<List<string>> result = new ResultModel<List<string>>();

            // Act
            result.Data = new List<string>();

            // Assert
            Assert.True(result.HasWarnings);
            Assert.Single(result.Warnings);
        }

        [Fact]
        // Setting Data to null should NOT trigger a warning
        // (CheckIfEmptyList returns early on null)
        public void Data_Null_DoesNotTriggerWarning()
        {
            // Arrange
            ResultModel<List<string>> result = new ResultModel<List<string>>();

            // Act
            result.Data = null;

            // Assert
            Assert.False(result.HasWarnings);
        }

        [Fact]
        // Setting Data to a non-empty List<string> { "item" } should NOT trigger a warning
        public void Data_NonEmptyCollection_DoesNotTriggerWarning()
        {
            // Arrange
            ResultModel<List<string>> result = new ResultModel<List<string>>();

            // Act
            result.Data = new List<string> { "item" };

            // Assert
            Assert.False(result.HasWarnings);
        }

        [Fact]
        // Setting Data to an empty string "" should NOT trigger a warning
        // (CheckIfEmptyList returns early when value is string)
        public void Data_EmptyString_DoesNotTriggerWarning()
        {
            // Arrange
            ResultModel<string> result = new ResultModel<string>();

            // Act
            result.Data = "";

            // Assert
            Assert.False(result.HasWarnings);
        }
    }
}
