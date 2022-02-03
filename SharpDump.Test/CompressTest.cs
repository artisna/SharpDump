using FluentAssertions;
using SharpDump.Logic;
using System.IO;
using System.Linq;
using Xunit;

namespace SharpDump.Test
{
    public class CompressTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("input.in")]
        public void Compress_WithInvalidInput_ThrowException(string inputFile)
        {
            // arrange
            var logger = new StatusLogger();
            var outputFile = "output.out";
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
            File.Exists(existedOutputFilePath).Should().BeFalse();
        }

        [Fact]
        public void Compress_WithExistedInputFile_CreateOutput()
        {
            // arrange
            var logger = new StatusLogger();
            var currentDirectory = Directory.GetCurrentDirectory();
            var existedInputFilePath = Path.Combine(currentDirectory, "Data", "existedEmptyInput.txt");
            var outputFilePath = Path.Combine(currentDirectory, "Data", "output.txt");

            // act
            Dump.Compress(logger, existedInputFilePath, outputFilePath);

            //assert
            File.Exists(outputFilePath).Should().BeTrue();
        }

        [Theory]
        [InlineData("inputWithContent.txt", "output.zip")]
        [InlineData("alreadyZippedInput.zip", "output.zip")]
        [InlineData("inputWithContent.txt", "output.txt")]
        [InlineData("inputWithContent.txt", "output.out")]
        public void Compress_WithExistedInputFileWithContent_ZipItToOutput(string inputFileName, string outputFileName)
        {
            // arrange
            var logger = new StatusLogger();
            var currentDirectory = Directory.GetCurrentDirectory();
            var existedInputFilePath = Path.Combine(currentDirectory, "Data", inputFileName);
            var outputFilePath = Path.Combine(currentDirectory, "Data", outputFileName);
            var inputFileLength = (new FileInfo(existedInputFilePath)).Length;

            // act
            Dump.Compress(logger, existedInputFilePath, outputFilePath);

            //assert
            File.Exists(outputFilePath).Should().BeTrue();
            
            var outputFileLength = (new FileInfo(outputFilePath)).Length;
            //inputFileLength.Should().BeGreaterThan(outputFileLength);
            Assert.NotEqual(inputFileLength, outputFileLength);
        }
    }
}