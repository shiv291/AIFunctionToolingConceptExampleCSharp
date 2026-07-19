using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SemanticKernelToolingExample
{
    public class CalculatorPlugin
    {
        [KernelFunction]
        [Description("Adds two numbers.")]
        public double Add(
            [Description("First number")] double a,
            [Description("Second number")] double b)
        {
            return a + b;
        }

        [KernelFunction]
        [Description("Subtracts two numbers.")]
        public double Subtract(double a, double b)
        {
            return a - b;
        }

        [KernelFunction]
        [Description("Multiplies two numbers.")]
        public double Multiply(double a, double b)
        {
            return a * b;
        }

        [KernelFunction]
        [Description("Divides two numbers.")]
        public double Divide(double a, double b)
        {
            if (b == 0)
                throw new ArgumentException("Cannot divide by zero.");

            return a / b;
        }

        [KernelFunction]
        [Description("Calculates the square root.")]
        public double SquareRoot(double number)
        {
            if (number < 0)
                throw new ArgumentException("Negative number.");

            return Math.Sqrt(number);
        }

        [KernelFunction]
        [Description("Raises a number to a power.")]
        public double Power(double number, double exponent)
        {
            return Math.Pow(number, exponent);
        }
    }
}
