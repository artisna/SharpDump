using FluentAssertions;
using Moq;
using SharpDump.Logic;
using Xunit;

namespace SharpDump.Test
{
    public class MinidumpTest
    {
        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        public void Minidump_AnyResult(int processId)
        {
            // arrange
            var logger = new StatusLogger();
            var mockedProcessProvider = new Mock<IProcessProvider>();

            // act
            Dump.Minidump(logger, mockedProcessProvider.Object, processId);

            //assert
            logger.Logs.Should().NotBeEmpty();
            logger.Logs.Should().Contain("any log");
        }
    }
}
