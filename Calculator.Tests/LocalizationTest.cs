using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;
using System.Globalization;

namespace Calculator.Tests
{
    public class LocalizationTest
    {
        [Fact]        
        public void TestNoInit()
        {
            Action action = () => LocalizationManager.GetString("Test");
            action.Should().Throw<Exception>();
        }

        [Fact]
        public void TestNoFoundLanguage()
        {
            LocalizationManager.Init();
            CultureInfo.CurrentCulture = new CultureInfo("gn"); //Guarani
            LocalizationManager.GetString("Test").Should().Be("Test");            
        }

        [Fact]
        public void TestKeyNotFound()
        {
            LocalizationManager.Init();
            CultureInfo.CurrentCulture = new CultureInfo("en"); 
            LocalizationManager.GetString("NO_KEY").Should().Be("NO_KEY");
        }

        [Fact]
        public void TestTraslations()
        {
            LocalizationManager.Init();
            CultureInfo.CurrentCulture = new CultureInfo("en"); 
            LocalizationManager.GetString("Division").Should().Be("Division");
            CultureInfo.CurrentCulture = new CultureInfo("it");
            LocalizationManager.GetString("Division").Should().Be("Divisione");
        }
    }
}
