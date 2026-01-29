using System.Text.RegularExpressions;
using System.Linq;
using System.Net.Http.Headers;


namespace MyCalculator_1_
{

    public partial class BackGround : Form
    {
        double result;
        string operation = " ";
        bool displayAnswer;
        public BackGround() //never delete this constructor
        {
            InitializeComponent();
        }

        private void button14_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void subtraction_Click(object sender, EventArgs e)
        {

        }

        private void division_Click(object sender, EventArgs e)
        {

        }

        private void multiply_Click(object sender, EventArgs e)
        {

        }

        private void ErrorCutie()
        {
            if (screen.Text == "Syntax Error" || screen.Text == "ERROR" || screen.Text == "Error")
            {
                screen.Clear();
                displayAnswer = false;
            }
        }

        private void Number_Click(object sender, EventArgs e)
        {
            ErrorCutie(); //helper for error guard
            Button button = (Button)sender;

            int cursorPosition = screen.SelectionStart;
            
            if (displayAnswer == true)
            {
                screen.Clear();
                displayAnswer = false;
            }

            if (displayAnswer == true || screen.Text == "Syntax Error" || screen.Text == "Error")
            {
                screen.Clear();
                displayAnswer = false;
                cursorPosition = 0;
            }

            if (cursorPosition > 0)
            {
                char currentChar = screen.Text[cursorPosition - 1];

                if ("+-/x".Contains(currentChar))
                {
                    screen.Text = screen.Text.Insert(cursorPosition, " ");
                    cursorPosition++;
                }
            }

            screen.Text = screen.Text.Insert(cursorPosition, button.Text);
            
            screen.SelectionStart = cursorPosition + 1;
            screen.Focus();
            screen.ScrollToCaret();
        }

        private void negativeSign(object sender, EventArgs e)
        {
            if (displayAnswer) displayAnswer = false;

            int cursorPosition = screen.SelectionStart;
            string text = screen.Text;


            int lastDelimiter = -1;
            char[] delimiters = { '+', 'x', '/', ' ' };

            for (int i = cursorPosition - 1; i >= 0; i--)
            {
                if (delimiters.Contains(text[i]))
                {
                    lastDelimiter = i;
                    break;
                }
            }

            int startOfNumber = lastDelimiter + 1;

            if (startOfNumber < text.Length && text[startOfNumber] == '-')
            {
                screen.Text = text.Remove(startOfNumber, 1);
                screen.SelectionStart = Math.Max(0, cursorPosition - 1);
            }
            else
            {
                screen.Text = text.Insert(startOfNumber, "-");
                screen.SelectionStart = cursorPosition + 1;
            }
            screen.Focus();
        }
        private void Operator_Click(object sender, EventArgs e)
        {
            ErrorCutie();
            Button button = (Button)sender;
            string newOperation = button.Text;
            int cursorPosition = screen.SelectionStart;

            if (displayAnswer) displayAnswer = false;

            if (string.IsNullOrEmpty(screen.Text))
            {
                if (newOperation == "-")
                {
                    screen.Text = "-";
                    screen.SelectionStart = 1;
                }
                return;
            }

            if (cursorPosition > 0)
            {
                char lastChar = screen.Text[cursorPosition - 1];

                if ("+-x/ ".Contains(lastChar))
                {
                    if (lastChar == ' ' && cursorPosition >= 2)
                    {
                        screen.Text = screen.Text.Remove(cursorPosition - 2, 3).Insert(cursorPosition - 2, " " + newOperation + " ");
                        screen.SelectionStart = cursorPosition + 1;
                    }
                    else
                    {
                        screen.Text = screen.Text.Remove(cursorPosition - 1, 1).Insert(cursorPosition - 1, newOperation);
                        screen.SelectionStart = cursorPosition;
                    }
                    return; 
                }
            }

         
            string toInsert = " " + newOperation + " ";
            screen.Text = screen.Text.Insert(cursorPosition, toInsert);
            screen.SelectionStart = cursorPosition + toInsert.Length;

            screen.Focus();
            screen.ScrollToCaret();
        }

        private void Result(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(screen.Text)) return;

            string pattern = @"-?\d+\.?\d*|[x/+-]"; 

            List<string> parts = Regex.Matches(screen.Text, pattern)
                                      .Cast<Match>()
                                      .Select(m => m.Value.Trim())
                                      .Where(v => !string.IsNullOrWhiteSpace(v))
                                      .ToList();

            if (parts.Count < 1) 
                return;

            if (parts.Count > 0 && "+-x/".Contains(parts.Last()))
            {
                screen.Text = "Syntax Error";
                displayAnswer = true;
                return;
            }

            try
            {
                for (int y = 0; y < parts.Count; y++)
                {
                    if (parts[y] == "x" || parts[y] == "/")
                    {
                        double left = double.Parse(parts[y - 1]);
                        double right = double.Parse(parts[y + 1]);
                        double res = 0;

                        if (parts[y] == "x") res = left * right;
                        else
                        {
                            if (right == 0) 
                                throw new DivideByZeroException();

                            res = left / right;
                        }

                        parts[y - 1] = res.ToString();
                        parts.RemoveAt(y); 
                        parts.RemoveAt(y); 
                        y--; 
                    }
                }

           
                double finalResult = double.Parse(parts[0]);
                for (int x = 1; x < parts.Count; x += 2)
                {
                    string op = parts[x];
                    double nextVal = double.Parse(parts[x + 1]);

                    if (op == "+") 
                        finalResult += nextVal;

                    else if (op == "-") 
                        finalResult -= nextVal;
                }

                screen.Text = Math.Round(finalResult, 7).ToString();
                displayAnswer = true;


            }

            catch (Exception)
            {
                screen.Text = "Syntax Error";
                displayAnswer = true;
            }
        }

        private void ACbutton(object sender, EventArgs e)
        {
            screen.Text = "";

            result = 0;
            operation = "";
        }

        private void Delbutton(object sender, EventArgs e)
        {
            int cursorPosition = screen.SelectionStart;

            if (screen.Text.Length > 0 && cursorPosition > 0)
            {
                screen.Text = screen.Text.Remove(cursorPosition - 1, 1);
                screen.SelectionStart = cursorPosition - 1;
            }

            screen.Focus();
        }

        private void Decimal_Click(object sender, EventArgs e)
        { 
            // fix multiple decimals in a number
            int cursorPosition = screen.SelectionStart;
            string textBeforeCursor = screen.Text.Substring(0, cursorPosition);

            string[] chunks = textBeforeCursor.Split(new char[] { ' ', '+', '-', 'x', '/'}, StringSplitOptions.None);
            string currentNumber = chunks.Last();

            if (currentNumber.Contains("."))
                return;

            string toInsert = (string.IsNullOrEmpty(currentNumber) || currentNumber == "-") ? "0." : ".";
            
            screen.Text = screen.Text.Insert(cursorPosition, toInsert);
            screen.SelectionStart = cursorPosition + toInsert.Length;
            screen.Focus();
            screen.ScrollToCaret();

        }

        private void doubleZero_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (displayAnswer == true || screen.Text == "Syntax Error" || screen.Text == "Error")
            {
                screen.Clear();
                displayAnswer = false;
            }

            int cursorPosition = screen.SelectionStart;

            if (cursorPosition == 0 || (cursorPosition > 0 && "x/+- ".Contains(screen.Text[cursorPosition - 1])))
            {
                return;
            }


            screen.Text = screen.Text.Insert(cursorPosition, "00");
            screen.SelectionStart = cursorPosition + 2;
            screen.Focus();
            screen.ScrollToCaret();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BackGround_Load(object sender, EventArgs e)
        {
            //kahoykahoy
        }

        private void right_Click(object sender, EventArgs e)
        {
            ErrorCutie(); //helper for error guard
            if (screen.SelectionStart < screen.Text.Length)
            {
                screen.SelectionStart += 1;
                screen.ScrollToCaret();
            }
            screen.Focus();
        }

        private void left_Click(object sender, EventArgs e)
        {
            ErrorCutie();
            if (screen.SelectionStart > 0)
            {
                screen.SelectionStart -= 1;
                screen.ScrollToCaret();     
            }

            screen.Focus();
        }

       
    }
}

// doesnt solve when the numbers and operators lose space