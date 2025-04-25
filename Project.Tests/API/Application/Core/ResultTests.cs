using Application.Core;
using Xunit;

namespace Project.Tests.Application.Core
{
    public class ResultTests
    {
        [Fact]
        public void Success_Should_Return_Successful_Result_With_Value()
        {
            // Arrange
            var value = "Hello";

            // Act
            var result = Result<string>.Success(value);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(value, result.Value);
            Assert.Null(result.Error);
        }

        [Fact]
        public void Failure_Should_Return_Failure_Result_With_Error()
        {
            // Arrange
            var errorMessage = "Something went wrong";

            // Act
            var result = Result<string>.Failure(errorMessage);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(errorMessage, result.Error);
            Assert.Null(result.Value);
        }
    }
}
