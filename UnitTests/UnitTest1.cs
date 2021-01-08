using calculator;
using NUnit.Framework;
using FluentAssertions;
using System;

namespace UnitTests
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(8, 4, '*', 32)]
        [TestCase(80, 90, '-', -10)]
        [TestCase(10, 5, '÷', 2)]
        [TestCase(10, 5, '+', 15)]
        public void Compute_ShouldReturnFResult_WhenArgsEqualSVal1OperationVal2(double value1, double value2, char operation, double result)
        {
            Computer.Compute($"{value1}{operation}{value2}").Should().Be(result.ToString());
        }

        [Test]
        public void Compute_ShouldReturn47And25_WhenArgsAre7Multiply3Divide4Multiply8()
        {
            Computer.Compute("7*3÷4*8").Should().Be("42");
        }
       

        [Test]
        public void Compute_CheckPriorities_ShouldReturn6()
        {
            Computer.Compute("2+2*2").Should().Be("6");
        }

        [Test]
        public void Compute_CheckBrackets_ShouldReturn8()
        {
            Computer.Compute("(2+2)*2").Should().Be("8");
        }
        
        [Test]
        public void Parse_CheckBracketsEachInOther_ShouldReturn80()
        {
            Computer.Compute("2*((2*(7-2)+2)*3+4)").Should().Be("80");
        }

        [Test]
        public void Compute_CheckUncorrectCharacter_ShouldReturnNull()
        {
            Computer.Compute("(2+2f)*2").Should().Be(null);
        }

        [Test]
        public void Parse_CheckNotEndedBracket_ShouldThrowException()
        {
            bool hasThrowed = false;
            try
            {
                Computer.Compute("2*2+2(");
            }
            catch (Exception)
            {
                hasThrowed = true;
            }
            hasThrowed.Should().BeTrue();
        }
    }
}