using Xunit;
using SharpDump.Logic;
using Moq;
using FluentAssertions;
using System;
using System.Linq;

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
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData("input.in", "output.out")]
        public void Compress_WithInvalidInputOutputFile_ThrowException(string inputFile, string outputFile)
        {
            // arrange
            var logger = new StatusLogger();
            var expectedLog = "[X] Exception while compressing file: ";

            // act
            Dump.Compress(logger, inputFile, outputFile);

            //assert
            logger.Logs.Last().Should().Contain(expectedLog);
        }
    }
}