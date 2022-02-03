using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using SharpDump.Logic;
using FluentAssertions;

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

            // act
            Dump.Minidump(logger, processId);

            //assert
            logger.Logs.Should().NotBeEmpty();
            logger.Logs.Should().Contain("any log");
        }
    }
}
