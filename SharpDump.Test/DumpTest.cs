using Xunit;
using SharpDump.Logic;
using Moq;
using FluentAssertions;
using System;

namespace SharpDump.Test
{
    public class DumpTest
    {
        [Fact]
        public void IsHighIntegrity() 
        {
            // arrange
            // act
            var anyResult = Dump.IsHighIntegrity();

            //assert
        }

        [Theory]
        [InlineData(null,null)]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData("input.in", "output.out")]
        public void Compress_WithInvalidInputOutputFile_ThrowException(string inputFile, string outputFile)
        {
            // arrange

            // act
            Action compression = () => Dump.Compress(inputFile, outputFile);

            //assert
            compression.Should().Throw<Exception>();
        }
    }
}