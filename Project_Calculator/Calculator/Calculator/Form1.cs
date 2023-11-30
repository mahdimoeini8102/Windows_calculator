using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MyClassLibrary;

namespace Calculator
{
    public partial class frm_main : Form
    {
        public frm_main()
        {
            InitializeComponent();
        }

        

        private void btn_num_click(object sender, MouseEventArgs e)
        {
            if (lbl_result.Text == "0")
            {
                lbl_result.Text = ((Button)sender).Text;
            }
            else
            {
                lbl_result.Text += ((Button)sender).Text;
            }
        }

        private void btn_bracket_open_Click(object sender, EventArgs e)
        {
            lbl_result.Text += '(';
        }

        private void btn_bracket_close_Click(object sender, EventArgs e)
        {
            lbl_result.Text += ')';
        }

        private void btn_plus_Click(object sender, EventArgs e)
        {
            lbl_result.Text += '+';
        }

        private void btn_minus_Click(object sender, EventArgs e)
        {
            lbl_result.Text += '-';
        }

        private void btn_multiplier_Click(object sender, EventArgs e)
        {
            lbl_result.Text += '×';
        }

        private void btn_divider_Click(object sender, EventArgs e)
        {
            lbl_result.Text += '÷';
        }

        private void frm_main_Load(object sender, EventArgs e)
        {
            lbl_result.Text = "";
            lbl_calculations.Text = "";
        }

        private void btn_equal_Click(object sender, EventArgs e)
        {
            if (lbl_calculations.Text == "" || (lbl_calculations.Text != "" && lbl_result.Text != ""))
            {
                MathParser total_expression = new MathParser(lbl_result.Text);
                lbl_calculations.Text = lbl_result.Text + "=";
                lbl_result.Text = Convert.ToString(total_expression.calculate_expression());
            }
            else
            {
                MathParser total_expression = new MathParser(lbl_calculations.Text.Remove(lbl_calculations.Text.Length - 1));
                lbl_result.Text = Convert.ToString(total_expression.calculate_expression());
            }
            
        }

        private void btn_clear_one_Click(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(lbl_result.Text.Length))
            {
                if( lbl_result.Text[lbl_result.Text.Length - 1] == '(')
                    if(lbl_result.Text.Length - 2 >= 0)
                        if(lbl_result.Text[lbl_result.Text.Length - 2] == '√')
                            lbl_result.Text = lbl_result.Text.Remove(lbl_result.Text.Length - 1, 1);
                
                lbl_result.Text = lbl_result.Text.Remove(lbl_result.Text.Length - 1);
            }
        }

        private void btn_clear_row_Click(object sender, EventArgs e)
        {
            if (lbl_calculations.Text.Length > 0)
                lbl_result.Text = lbl_calculations.Text.Remove(lbl_calculations.Text.Length - 1);
            else
                lbl_result.Text = "";
            lbl_calculations.Text = "";
        }

        private void btn_sqrt_Click(object sender, EventArgs e)
        {
            lbl_result.Text += "√(";
        }

        private void btn_power_Click(object sender, EventArgs e)
        {
            lbl_result.Text += "^";
        }

        private void btn_dot_Click(object sender, EventArgs e)
        {
            lbl_result.Text += ".";
        }

        private void btn_sign_change_Click(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(lbl_result.Text.Length))
            {
                if (lbl_result.Text[0] == '-')
                {
                    lbl_result.Text = lbl_result.Text.Remove(0, 1);
                }
                else
                {
                    lbl_result.Text = "-" + lbl_result.Text;
                }
            }
            else
            {
                lbl_result.Text = "-";
            }

        }

        private void btn_clear_all_Click(object sender, EventArgs e)
        {
            lbl_result.Text = "";
            lbl_calculations.Text = "";
        }



        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            MouseEventArgs e1 = null;
            switch (keyData)
            {
                case Keys.D0:
                case Keys.NumPad0:
                    btn_num_click(btn_num0, e1);
                    return true;
                case Keys.D1:
                case Keys.NumPad1:
                    btn_num_click(btn_num1, e1);
                    return true;
                case Keys.D2:
                case Keys.NumPad2:
                    btn_num_click(btn_num2, e1);
                    return true;
                case Keys.D3:
                case Keys.NumPad3:
                    btn_num_click(btn_num3, e1);
                    return true;
                case Keys.D4:
                case Keys.NumPad4:
                    btn_num_click(btn_num4, e1);
                    return true;
                case Keys.D5:
                case Keys.NumPad5:
                    btn_num_click(btn_num5, e1);
                    return true;
                case Keys.D6:
                case Keys.NumPad6:
                    btn_num_click(btn_num6, e1);
                    return true;
                case Keys.D7:
                case Keys.NumPad7:
                    btn_num_click(btn_num7, e1);
                    return true;
                case Keys.D8:
                case Keys.NumPad8:
                    btn_num_click(btn_num8, e1);
                    return true;
                case Keys.D9:
                case Keys.NumPad9:
                    btn_num_click(btn_num9, e1);
                    return true;
                case Keys.Multiply:
                    btn_multiplier_Click(btn_multiplier, e1);
                    return true;
                case Keys.Add:
                    btn_plus_Click(btn_plus, e1);
                    return true;
                case Keys.Subtract:
                    if (lbl_result.Text == "-")
                    {
                        btn_sign_change_Click(btn_sign_change, e1);
                    }
                    else
                    {
                        btn_minus_Click(btn_minus, e1);
                    }
                    return true;
                case Keys.Decimal:
                    btn_dot_Click(btn_dot, e1);
                    return true;
                case Keys.Divide:
                    btn_divider_Click(btn_divider, e1);
                    return true;
                case Keys.Shift | Keys.D6:
                    btn_power_Click(btn_power, e1);
                    return true;
                case Keys.Back:
                    btn_clear_one_Click(btn_clear_one, e1);
                    return true;
                case Keys.Delete:
                    btn_clear_row_Click(btn_clear_row, e1);
                    return true;
                case Keys.Escape:
                    btn_clear_all_Click(btn_clear_all, e1);
                    return true;
                case Keys.Enter:
                    btn_equal_Click(btn_equal, e1);
                    return true;
                case Keys.Shift | Keys.D9:
                    btn_bracket_open_Click(btn_bracket_open, e1);
                    return true;
                case Keys.Shift | Keys.D0:
                    btn_bracket_close_Click(btn_bracket_close, e1);
                    return true;
                case Keys.Shift | Keys.D2:
                    btn_sqrt_Click(btn_sqrt, e1);
                    return true;
                default:
                    return true;//return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        private void lbl_result_TextChanged(object sender, EventArgs e)
        {
            Font SmallFont = new Font("Consolas", 8);
            Font LargeFont = new Font("Consolas", 15);
            if (((Label)sender).Text.Length >= 20)
                ((Label)sender).Font = SmallFont;
            else
                ((Label)sender).Font = LargeFont;
            
        }

        
    }
}
