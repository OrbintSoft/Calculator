using FluentAssertions;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Calculator.Tests
{
    public class ConsoleTest
    {
        private readonly ITestOutputHelper output;

        public ConsoleTest(ITestOutputHelper output)
        {
            LocalizationManager.Init();
            this.output = output;
        }


        [Fact]
        public void TestMissingArguments()
        {            
            var args = new string[0];
            Program.Main(args).Should().Be(-1);
            Action action = () => Program.ExecuteCommand(args);
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void TestWrongArguments()
        {
            var args = new string[2] { "-fd", "dfdf" };
            Program.Main(args).Should().Be(-1);
            Action action = () => Program.ExecuteCommand(args);
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void TestFile()
        {
            var args = new string[2] { "-f", "input.txt" };
            Program.Main(args).Should().Be(0);
        }
    }
}
