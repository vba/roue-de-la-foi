using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FluentAssertions;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using SupportWheelOfFate.Core.Dto;
using SupportWheelOfFate.Core.Generator;
using SupportWheelOfFate.Core.Queries;
using SupportWheelOfFate.Core.Repository;
using Xunit;

namespace SupportWheelOfFate.Core.Tests.Repository
{
    public class WheelUsherRepositorySpec
    {
        private const int ParallelTestCount = 100;

        [Theory(DisplayName = "Turn wheel should fail when engineers count is not enough"), AutoMoqData]
        public void TurnWheel_ShouldFail_WhenEngineersCountIsNotEnough(WheelUsherRepository sut, [Frozen] Mock<IEngineersGenerator> engineersGenerator, Generator<int> generator)
        {
            // Arrange
            var count = generator.First(x => x < 4);
            Action turn = () => sut.TurnWheel(new TurnWheelQuery(count));
            
            // Act & Assert
            turn.ShouldThrow<ArgumentOutOfRangeException>().And.Message.Should()
                .StartWith($"Engineers count cannot be less than {EngineersBoundary.Min.Value}");
        }
        
        [Theory(DisplayName = "Turn wheel should produce correct support time table when even engineers count is received"), AutoMoqData]
        public void TurnWheel_ShouldGetCorrectTimeTable_WhenEvenEngineersCountIsReceived(Mock<IEngineersGenerator> engineersGenerator, Generator<int> intGenerator, Generator<(string, string)> tupleGenerator)
        {
            Enumerable.Range(0, ParallelTestCount).AsParallel().ForAll(_ =>
            {                
                // Arrange
                var sut = new WheelUsherRepository(engineersGenerator.Object);
                var count = intGenerator.First(x => x > 4 && x % 2 == 0);
                var tuples = tupleGenerator.Take(count).ToImmutableList();
                IImmutableList<ServiceDayDto> actualResult;
    
                engineersGenerator.Setup(x => x.Generate(count)).Returns(() => tuples);
    
                // Act
                actualResult = sut.TurnWheel(new TurnWheelQuery(count));
                
                // Assert
                actualResult.Should().HaveCount(count);
                ValidateAtMostOneHalfDayShift(actualResult);
                ValidateNoConsecutiveHalfDayShifts(actualResult);
            });
        }
        
        [Theory(DisplayName = "Turn wheel should produce correct support time table when odd engineers count is received"), AutoMoqData]
        public void TurnWheel_ShouldGetCorrectTimeTable_WhenOddEngineersCountIsReceived(Mock<IEngineersGenerator> engineersGenerator, Generator<int> intGenerator, Generator<(string, string)> tupleGenerator)
        {
            Enumerable.Range(0, ParallelTestCount).AsParallel().ForAll(_ =>
            {                
                // Arrange
                var sut = new WheelUsherRepository(engineersGenerator.Object);
                var count = intGenerator.First(x => x > 4 && x % 2 != 0);
                var tuples = tupleGenerator.Take(count).ToImmutableList();
                IImmutableList<ServiceDayDto> actualResult;
    
                engineersGenerator.Setup(x => x.Generate(count)).Returns(() => tuples);
    
                // Act
                actualResult = sut.TurnWheel(new TurnWheelQuery(count));
                
                // Assert
                actualResult.Should().HaveCount(count);
                ValidateAtMostOneHalfDayShift(actualResult);
                ValidateNoConsecutiveHalfDayShifts(actualResult);
            });
        }
        
        private static void ValidateNoConsecutiveHalfDayShifts(IImmutableList<ServiceDayDto> actualResult)
        {
            var fractions = actualResult.GroupByFractionOf(2);
            
            if (fractions.Last().Item2.Count == 1)
            {
                fractions.Last().Item2.Add(fractions.First().Item2.First());
            }
            
            fractions.ForEach(x =>
            {
                var first = x.Item2[0];
                var last = x.Item2[1];
                ValidateSequentialityRule(first, last);
            });
            ValidateSequentialityRule(actualResult.Last(), actualResult.First());
        }

        private static void ValidateSequentialityRule(ServiceDayDto first, ServiceDayDto last)
        {
            const string rule = "An engineer cannot have half day shifts on consecutive days";
            
            (!Equals(first.ForenoonEngineer, last.ForenoonEngineer)).Should().BeTrue(rule);
            (!Equals(first.ForenoonEngineer, last.AfternoonEngineer)).Should().BeTrue(rule);
            (!Equals(first.AfternoonEngineer, last.ForenoonEngineer)).Should().BeTrue(rule);
            (!Equals(first.AfternoonEngineer, last.AfternoonEngineer)).Should().BeTrue(rule);
        }

        private static void ValidateAtMostOneHalfDayShift(IImmutableList<ServiceDayDto> actualResult)
        {
            const string rule = "An engineer can do at most one half day shift in a day";
            actualResult.Any(x => Equals(x.ForenoonEngineer, x.AfternoonEngineer))
                .Should().BeFalse(rule);
        }
    }

    public static class EnumerableExtensions
    {
        public static List<(int, List<TSource>)> GroupByFractionOf<TSource>(this IEnumerable<TSource> source, int itemsPerFraction)
        {
            var enumerable = source as IList<TSource> ?? source.ToList();
            return enumerable.Zip(Enumerable.Range(0, enumerable.Count), (x, y) => new { Fraction = y / itemsPerFraction, Item = x })
                .GroupBy(i => i.Fraction, g => g.Item)
                .Select(x => (x.Key, x.ToList()))
                .ToList();
        }
    }
}