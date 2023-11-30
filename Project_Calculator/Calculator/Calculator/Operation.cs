using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyClassLibrary
{
    enum Operator
    {
        Plus,
        Minus,
        Multiplier,
        Divider,
        Sqrt,
        Power
    }

    class Operation
    {
        float operand1;
        float operand2;
        Operator function;
        float result;

        public Operation() { }
        public Operation(float operand1)
        {
            this.operand1 = operand1;
            this.operand2 = 0;
            this.function = Operator.Plus;
        }
        public Operation(float operand1, Operator function)
        {
            this.operand1 = operand1;
            this.operand2 = 0;
            this.function = function;
        }
        public Operation(float operand1, char function, float operand2)
        {
            this.operand1 = operand1;
            this.operand2 = operand2;
            set_operator(function);
        }

        public void set_operator(char function)
        {
            switch (function)
            {
                case '+':
                    this.function = Operator.Plus;
                    break;
                case '-':
                    this.function = Operator.Minus;
                    break;
                case '×':
                    this.function = Operator.Multiplier;
                    break;
                case '÷':
                    this.function = Operator.Divider;
                    break;
                case '^':
                    this.function = Operator.Power;
                    break;
                case '√':
                    this.function = Operator.Sqrt;
                    break;

                default:
                    this.function = Operator.Plus;
                    break;
            }
        }

        public void set_operand1(float operand1)
        {
            this.operand1 = operand1;
        }

        public void set_operand2(float operand2)
        {
            this.operand2 = operand2;
        }

        public float calculate_result()
        {
            switch (function)
            {
                case Operator.Plus:
                    result = operand1 + operand2;
                    break;
                case Operator.Minus:
                    result = operand1 - operand2;
                    break;
                case Operator.Multiplier:
                    result = operand1 * operand2;
                    break;
                case Operator.Divider:
                    result = operand1 / operand2; // Exeption of divided by zero!!!
                    break;
                case Operator.Power:
                    result = (float)(Math.Pow(Convert.ToDouble(operand1), Convert.ToDouble(operand2)));
                    break;
                case Operator.Sqrt:
                    result = (float)(Math.Sqrt(Convert.ToDouble(operand1)));
                    break;
                default:
                    result = 0;
                    break;
            }
            return result;
        }


    }
}
