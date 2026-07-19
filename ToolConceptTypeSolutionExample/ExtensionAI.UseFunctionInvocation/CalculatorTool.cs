using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ExtensionAI.UseFunctionInvocation
{
    public class CalculatorTool
    {
        [Description("Adds two numbers.")]
        public double Add(double x, double y) => x + y;
        [Description("Subtracts second number from first.")]
        public double Subtract(double a, double b) => a - b;

        [Description("Multiplies two numbers.")]
        public double Multiply(double a, double b) => a * b;

        [Description("Divides first number by second.")]
        public double Divide(double a, double b)
        {
            if (b == 0) throw new ArgumentException("Division by zero.");
            return a / b;
        }

        [Description("Raises a number to a power.")]
        public double Power(double number, double exponent)
            => Math.Pow(number, exponent);

        [Description("Calculates the square root.")]
        public double SquareRoot(double number)
        {
            if (number < 0) throw new ArgumentException("Negative input.");
            return Math.Sqrt(number);
        }
    }
}
