using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyClassLibrary
{
    public class MathParser
    {
        string expression;

        public MathParser() { }
        public MathParser(string text)
        {

            //multiply_detection(); //this function detect hide multipliers in expressions like 2(1+3) or 3√4
            text = priority_detection(text, '^');
            text = priority_detection(text, '×', '÷');
            text = priority_detection(text, '+', '-');
            text = add_a_before_b(text, '0', '.'); // multi point in one number not supported yet! (like 2.5.3.3.5)
            text = add_needed_parantez(text);
            this.expression = text;
        }

        public float calculate_expression()
        {
            return run_calculate(expression, false);
        }


        private float run_calculate(string expression, Boolean operand1_is_negative)
        {
            Operation main_operation;
            float result = 0;
            int start_index = 0;
            if (expression.Length == 0)
                return 0;
            else if (operand1_is_negative)
            {
                start_index++;
                main_operation = new Operation(-get_number(expression, ref start_index));
                if (start_index != expression.Length)
                {
                    if (is_operator(expression[start_index], false))
                    {
                        main_operation.set_operator(expression[start_index]);
                        detect_expression(expression.Remove(0, start_index + 1), 0, true, 0);
                        main_operation.set_operand2(run_calculate(expression.Remove(0, start_index + 1), false));
                        return main_operation.calculate_result();
                    }
                    else ;
                    //Expression error!
                }
                result = main_operation.calculate_result();
            }
            else if (is_number(expression[start_index]))
            {
                main_operation = new Operation(get_number(expression, ref start_index));
                if (start_index != expression.Length)
                {
                    if (is_operator(expression[start_index], false))
                    {
                        main_operation.set_operator(expression[start_index]);
                        main_operation.set_operand2(run_calculate(expression.Remove(0, start_index + 1), false));
                        return main_operation.calculate_result();
                    }
                    else ;
                    //Expression error!
                }
                result = main_operation.calculate_result();
            }
            else if (expression[start_index] == '(')
            {
                int end_expression_index = detect_expression(expression, start_index - 1, true, 0);

                string local_expression = expression;
                local_expression = local_expression.Remove(end_expression_index);
                local_expression = local_expression.Remove(0, start_index + 1);

                string local_result = Convert.ToString(run_calculate(local_expression, false));

                expression = expression.Remove(start_index, -start_index + end_expression_index + 1);
                expression = expression.Insert(start_index, local_result);
                return run_calculate(expression, false);
            }
            else if (expression[start_index] == '√')
            {
                int end_expression_index = detect_expression(expression, start_index, true, 0);

                string local_expression = expression;
                local_expression = local_expression.Remove(end_expression_index);
                local_expression = local_expression.Remove(0, start_index + 2);


                expression = expression.Remove(start_index, -start_index + end_expression_index + 1);

                main_operation = new Operation(run_calculate(local_expression, false));
                main_operation.set_operator('√');

                expression = expression.Insert(start_index, Convert.ToString(main_operation.calculate_result()));
                return run_calculate(expression, false);

            }
            else if (expression[start_index] == '∞')
            {
                // Divided by zero exception
            }
            else if (expression == "NaN")
            {
                // Negative number under sqrt exception
            }
            else if (expression[start_index] == '-' || expression[start_index] == '+')
            {
                char sign = expression[start_index];
                int negative_number;
                if (sign == '+')
                    negative_number = 0;
                else
                    negative_number = 1;
                if (expression.Length == 1)
                    return 0;
                while (expression[start_index] == '-' || expression[start_index] == '+')
                {
                    expression = expression.Remove(0, 1);
                    if (expression[start_index] == '-')
                        negative_number++;
                }
                if (negative_number % 2 == 0)
                {
                    sign = '+';
                    expression = expression.Insert(0, "+");
                }
                else
                {
                    sign = '-';
                    expression = expression.Insert(0, "-");
                }
                start_index++;
                if (is_number(expression[start_index]))
                {
                    main_operation = new Operation(0, sign, get_number(expression, ref start_index));
                    if (start_index == expression.Length)
                    {
                        result = main_operation.calculate_result();
                    }
                    else
                    {
                        if (sign == '-')
                            result = run_calculate(expression, true);
                        else
                            result = run_calculate(expression, false);
                    }
                }
                else if (expression[start_index] == '(')
                {
                    int end_expression_index = detect_expression(expression, start_index - 1, true, 0);

                    string local_expression = expression;
                    local_expression = local_expression.Remove(end_expression_index);
                    local_expression = local_expression.Remove(0, start_index + 1);

                    string local_result = Convert.ToString(run_calculate(local_expression, false));

                    expression = expression.Remove(start_index, end_expression_index - start_index + 1);
                    expression = expression.Insert(start_index, local_result);
                    result = run_calculate(expression, false);
                }
                else if (expression[start_index] == '√')
                {
                    int end_expression_index = detect_expression(expression, start_index, true, 0);

                    string local_expression = expression;
                    local_expression = local_expression.Remove(end_expression_index);
                    local_expression = local_expression.Remove(0, start_index + 2);


                    expression = expression.Remove(start_index, -start_index + end_expression_index + 1);

                    main_operation = new Operation(run_calculate(local_expression, false));
                    main_operation.set_operator('√');

                    expression = expression.Insert(start_index, Convert.ToString(main_operation.calculate_result()));
                    return run_calculate(expression, false);
                }
            }
            return result;
        }
        /// <summary>
        /// This function put parantezes in your expression to calculate answer easier.
        /// </summary>
        /// <param name="expression">It's a mathematic expression like '1+6-4'</param>
        /// <param name="sensetive_list">Choose signs that you want to set priority base on them. (like '+' or '-')</param>
        /// <returns></returns>
        private string priority_detection(string expression, params char[] sensetive_list)
        {
            List<char> temp_priority_detected_expression = new List<char>();
            temp_priority_detected_expression.AddRange(expression);

            int parantez_open_index;
            List<int> parantez_close_index = new List<int> { };

            int i = 0;
            while (i <= temp_priority_detected_expression.Count)
            {
                if (parantez_close_index.Count > 0)
                {
                    while (i == parantez_close_index[0])
                    {
                        temp_priority_detected_expression.Insert(parantez_close_index[0], ')');
                        parantez_close_index.RemoveAt(0);
                        if (parantez_close_index.Count == 0) break;
                    }
                }
                if (i == temp_priority_detected_expression.Count) break;
                if (sensetive_list.Contains(temp_priority_detected_expression[i]))
                {
                    parantez_open_index = detect_expression(string.Concat(temp_priority_detected_expression), i, false, 0);
                    temp_priority_detected_expression.Insert(parantez_open_index, '(');
                    i++;
                    for (int j = 0; j < parantez_close_index.Count; j++)
                        parantez_close_index[j] += 2;
                    parantez_close_index.Add(detect_expression(string.Concat(temp_priority_detected_expression), i, true, 0));
                    parantez_close_index.Sort();
                }
                i++;
            }
            return string.Concat(temp_priority_detected_expression);
        }
        private string add_a_before_b(string expression, char a, char b)
        {
            List<char> temp_result = new List<char>();
            temp_result.AddRange(expression);
            int i = 0;
            while (i < temp_result.Count)
            {
                if (temp_result[i] == '.' && i == 0)
                    temp_result.Insert(i, '0');
                else if (temp_result[i] == '.' && !is_number(temp_result[i - 1]))
                    temp_result.Insert(i, '0');
                i++;
            }
            return string.Concat(temp_result);
        }
        private string add_needed_parantez(string expression)
        {
            int count = expression.Split('(').Length - expression.Split(')').Length;
            int i = 0;
            if(count > 0)
                for (i = count; i > 0; i--)
                    expression = expression.Insert(expression.Length, ")");
            else if(count < 0)
                for (i = count; i < 0; i++)
                    if((expression[expression.Length - 1]) == ')')
                        expression = expression.Remove(expression.Length - 1);
            if (i != 0)
            {
                //parantez needed exception!
            }
            return expression;
        }
        /// <summary>
        /// This function get the string and the index of start and go to_forward or to_back (you should set it) to
        /// find where the expression finish and return index of it. for example you send 1-(1+3+4)+5 with index=9
        /// and to_forward = flase. the function return 1. 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="index">Start index of text</param>
        /// <param name="to_forward">Search text to forward (true) or to back (false)</param>
        /// <param name="level_parantez">You must put '0' here. However, this function use it because it's recursively call itself.</param>
        /// <returns></returns>
        private int detect_expression(string text, int index, Boolean to_forward, int level_parantez)
        {
            if (level_parantez == 0)
            {
                if (!to_forward) //to back
                {
                    while (index > 0)
                    {
                        index--;
                        if (is_number(text[index]) || text[index] == '.')
                        {
                            //__NOP
                        }
                        else if (is_operator(text[index], !to_forward))
                        {
                            return index + 1;
                        }
                        else if (text[index] == ')')
                        {
                            return detect_expression(text, index - 1, false, level_parantez + 1);
                        }
                    }
                    return index;
                }
                else //to forward
                {
                    while (index < text.Length - 1)
                    {
                        index++;
                        if (is_number(text[index]) || text[index] == '.')
                        {
                            //__NOP
                        }
                        else if (is_operator(text[index], !to_forward))
                        {
                            return index;
                        }
                        else if (text[index] == '(')
                        {
                            return detect_expression(text, index + 1, true, level_parantez + 1);
                        }
                    }
                    return index + 1;
                }
            }
            else
            {
                if (!to_forward) //to_back
                {
                    while (index >= 0)
                    {
                        if (text[index] == ')')
                            level_parantez++;
                        else if (text[index] == '(')
                        {
                            if (index - 1 >= 0 && text.Length >= 1)
                                if (text[index - 1] == '√')
                                    index--;
                            level_parantez--;
                        }
                        if (level_parantez == 0)
                            return index;
                        index--;
                    }
                    return index;
                }
                else
                {
                    while (index < text.Length)
                    {
                        if (text[index] == '(')
                            level_parantez++;
                        else if (text[index] == ')')
                            level_parantez--;
                        if (level_parantez == 0)
                            return index;
                        index++;
                    }
                    return index;
                }
            }
        }



        private Boolean is_number(char input)
        {
            if (Convert.ToBoolean(input >= 48) && Convert.ToBoolean(input <= 57))
                return true;
            else
                return false;
        }
        private Boolean is_operator(char input, Boolean to_back)
        {
            if (input == '+' || input == '-' || input == '×' || input == '^' ||
                input == '÷' || (input == '√' && to_back))
                return true;
            else
                return false;
        }

        private float get_number(string expression, ref int start_index)
        {
            float int_part_result = 0, frac_part_result = 0;
            Boolean fractional_part_enable = false;
            int frac_digits = 0;
            while (start_index < expression.Length)
            {
                if (is_number(expression[start_index]))
                {
                    if (!fractional_part_enable)
                    {
                        int_part_result *= 10;
                        int_part_result += int.Parse(expression[start_index].ToString());
                    }
                    else
                    {
                        frac_digits++;
                        frac_part_result *= 10;
                        frac_part_result += int.Parse(expression[start_index].ToString());
                    }
                }
                else if (expression[start_index] == '.')
                {
                    if (!fractional_part_enable)
                    {
                        fractional_part_enable = true;
                    }
                    else
                    {
                        //error: two points in one number
                    }
                }
                else
                {
                    break;
                }
                start_index++;
            }
            frac_part_result /= Convert.ToInt64(Math.Pow(10, frac_digits));
            return int_part_result + frac_part_result;
        }
    }
}
