using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;

namespace Calculator.Tests
{
    public class ExpressionTest
    {
        [Fact]
        public void BasicOperations()
        {
            ExpressionCalculator expression = new ExpressionCalculator();
            expression.GetResult("2+3").Should().Be(2d + 3d);
            expression.GetResult("2-3").Should().Be(2d - 3d);
            expression.GetResult("2*3").Should().Be(2d * 3d);
            expression.GetResult("5/2").Should().Be(5d / 2d);
            expression.GetResult("2^3").Should().Be(Math.Pow(2,3));
            Math.Round(expression.GetResult("2r3"),5).Should().Be(Math.Round(Math.Exp(Math.Log(2, Math.E) /3),5)); //round because double precision limit
            Math.Round(expression.GetResult("2r3"), 6).Should().Be(Math.Round(Math.Pow(2d, 1d/3d),6)); //round because double precision limit
            Math.Round(expression.GetResult("2l3"),6).Should().Be(Math.Round(Math.Log(2d,3d),6)); //round because double precision limit
        }

    }
}
