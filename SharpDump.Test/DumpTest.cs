using Xunit;
using SharpDump.Logic;
using Moq;
using FluentAssertions;
using System;
using System.Linq;
using System.IO;

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

        [Fact]
        public void Compress_WithExistedOutputFile_RemoveIt()
        {
            // arrange
            var logger = new StatusLogger();
            var inputFile = "";
            var currentDirectory = Directory.GetCurrentDirectory();
            var existedOutputFilePath = Path.Combine(currentDirectory, "Data", "existedOutput.txt");
            var expectedLog = $"[X] Output file '{existedOutputFilePath}' already exists, removing";

            // act
            Dump.Compress(logger, inputFile, existedOutputFilePath);

            //assert
            logger.Logs.Should().Contain(expectedLog);
        }
    }
}