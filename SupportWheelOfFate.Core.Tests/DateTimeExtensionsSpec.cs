using System;
using System.Collections.Immutable;
using FluentAssertions;
using Xunit;

namespace SupportWheelOfFate.Core.Tests
{
    public class DateTimeExtensionsSpec
    {
        [Fact]
        public void GetFollowingWorkingDays_ShouldProvideWorkingDays_WhenSuchAmountIsProvided()
        {
            // Arrange
            var workingDaysAmount = 5;
            var sut = DateTime.Now;
            IImmutableList<DateTime> actualWorkingDays;
            
            // Act
            actualWorkingDays = sut.GetFollowingWorkingDays(workingDaysAmount);
            
            // Assert
            actualWorkingDays.Should().HaveCount(workingDaysAmount);
        }
    }
}