using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FluentAssertions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using SupportWheelOfFate.Core.Generator;
using Xunit;

namespace SupportWheelOfFate.Core.Tests.Generator
{
    public class EngineersGeneratorSpec
    {
        [Theory(DisplayName = "Generate should fail when count is out of range of 1..FirstNames.Count"), AutoData]
        public void Generate_ShouldFail_WhenCountOutRangesFirstNames(Generator<int> generator)
        {
            // Arrange
            var count = generator.First(x => x > 97 && x < 150);
            var sut = new EngineersGenerator();
            Action generate = () => sut.Generate(count);

            // Act & Assert
            generate.ShouldThrow<ArgumentOutOfRangeException>().And.Message.Should()
                .StartWith($"Count {count} is out of first names range of 1..");
        }
        
        
        [Theory(DisplayName = "Generate should fail when count is out of range of 1..LastNames.Count"), AutoData]
        public void Generate_ShouldFail_WhenCountOutRangesLastNames(Generator<int> generator)
        {
            // Arrange
            var count = generator.First(x => x > 1000);
            var sut = new EngineersGenerator();
            Action generate = () => sut.Generate(count);

            // Act & Assert
            generate.ShouldThrow<ArgumentOutOfRangeException>().And.Message.Should()
                .StartWith($"Count {count} is out of last names range of 1..");
        }
        
        [Theory(DisplayName = "Generate should get a list of tuples when count lays is in the ranges of names"), AutoData]
        public void Generate_ShouldGetList_WhenCountInRangesLastNames(Generator<int> generator)
        {
            // Arrange
            var count = generator.First(x => x > 9 && x < 97);
            var sut = new EngineersGenerator();
            IImmutableList<(string, string)> actual;
            
            // Act
            actual = sut.Generate(count);
            
            // Assert
            actual.Should().HaveCount(count);
            actual.Where(x => x.Item1 == null || x.Item2 == null).Should().HaveCount(0);
        }
    }
}