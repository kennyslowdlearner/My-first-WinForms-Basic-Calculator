using System.Text.RegularExpressions;
using System.Linq;


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

                if ("x/+-".Contains(currentChar))
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
            if (displayAnswer == true)
            {
                screen.Clear();
                displayAnswer = false;
            }

            int cursorPosition = screen.SelectionStart;

            if (cursorPosition == 0 || "x/+".Contains(screen.Text[cursorPosition - 1]))
            {
                screen.Text = screen.Text.Insert(cursorPosition, "-");
                screen.SelectionStart = cursorPosition + 1;
            }
            else
                return;

            screen.Focus();
            screen.ScrollToCaret();
        }
        private void Operator_Click(object sender, EventArgs e)
        {
            // implement line for 9 + 7 - 8 / -> okay na bahahaha
            ErrorCutie(); //helper for error guard

            Button button = (Button)sender;
            string newOperation = button.Text;
            int cursorPosition = screen.SelectionStart;

            string[] parts = screen.Text.Split(' ');
            string currentNumber = parts[parts.Length - 1];

            if (displayAnswer == true)
            {
               screen.Clear();
               displayAnswer = false;
            }

            //if (displayAnswer)
            //    displayAnswer = false;

            if (screen.Text == "Syntax Error" || screen.Text == "Error")
            {
                screen.Clear();
                if (button.Text != "-")
                    return;
            }

            if (string.IsNullOrEmpty(screen.Text))
            {
                if (newOperation == "-")
                {
                    screen.Text = "-";
                    screen.SelectionStart = screen.Text.Length;
                    return;
                }

                return;
            }

            

            if (newOperation == "-")
            {
                if (string.IsNullOrEmpty(screen.Text) || (cursorPosition > 0 && screen.Text[cursorPosition - 1] == ' '))
                {
                    screen.Text  = screen.Text.Insert(cursorPosition, "-");
                    screen.SelectionStart = cursorPosition + 1;
                    return;
                }
            }

            if (cursorPosition > 0 && screen.Text[cursorPosition - 1] == ' ')
            {
                if (cursorPosition >= 2)
                {
                    screen.Text = screen.Text.Remove(cursorPosition - 2, 1).Insert(cursorPosition - 2, newOperation);
                    screen.SelectionStart = cursorPosition;
                }
                
                screen.Focus();
                screen.ScrollToCaret();
                return;
            }

            string operationSpace = " " + newOperation + " ";
            screen.Text = screen.Text.Insert(cursorPosition, operationSpace);

            screen.SelectionStart = cursorPosition + operationSpace.Length;

            screen.Focus();
            screen.ScrollToCaret();
            
        }

        private void Result(object sender, EventArgs e)
        {
            // TO DO
            // gonna study this one first
            // putting negative numbers and decimals aside for now -> okay na
            // multiline problems -> di ma scroll down


            if (string.IsNullOrEmpty(screen.Text))
                return;

            string pattern = @"(?<=\b|^| )-\d+\.?\d*|\d+\.?\d*|[x/+-]";

            List<string> parts = Regex.Matches(screen.Text, pattern)
                                        .Cast<Match>()
                                        .Select(m => m.Value)
                                        .ToList();

            if (parts.Count == 0)
                return;
            if ("x/+".Contains(parts.Last()))
            {
                screen.Text = "Syntax Error";
                displayAnswer = true;
                return;
            }


            if (parts[0] == "x" || parts[0] == "/" || parts[0] == "+")
            {
                screen.Text = "Syntax Error";
                displayAnswer = true;
                return;
            }

            if (parts.Count < 3 && !(parts.Count == 1 && double.TryParse(parts[0], out _)))
            {
                screen.Text = "Syntax Error";
                displayAnswer = true;
                return;
            }


            try
            {
                for (int z = 0; z < parts.Count; z++)
                {
                    if (parts[z] == "x" || parts[z] == "/" || parts[z] == "+" || parts[z] == "-")
                        continue;

                    if (!double.TryParse(parts[z], out _))
                        throw new FormatException();
                }


                for (int a = 0; a < parts.Count; a++)
                {
                    if (parts[a] == "x" || parts[a] == "/")
                    {
                        double integerOne = double.Parse(parts[a - 1]);
                        double integerTwo = double.Parse(parts[a + 1]);
                        double tempResult = 0;

                        if (parts[a] == "x")
                            tempResult = integerOne * integerTwo;

                        else if (parts[a] == "/")
                        {
                            if (integerTwo == 0)
                                throw new DivideByZeroException();
                            tempResult = integerOne / integerTwo;
                        }

                        parts[a - 1] = tempResult.ToString();
                        parts.RemoveAt(a);
                        parts.RemoveAt(a);
                        a--;
                    }
                }


                double currentResult = double.Parse(parts[0]);

                for (int a = 1; a < parts.Count; a += 2)
                {
                    double solveNext = double.Parse(parts[a + 1]);
                    string operation = parts[a];

                    if (operation == "+")
                        currentResult += solveNext;

                    else if (operation == "-")
                        currentResult -= solveNext;
                }

                double roundedResult = Math.Round(currentResult, 7);
                screen.Text = roundedResult.ToString();
                displayAnswer = true;
            }
            catch (DivideByZeroException)
            {
                screen.Text = "Math ERROR";
                displayAnswer = true;
            }
            catch (Exception)
            {
                screen.Text = "Syntax ERROR";
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

            string[] chunks = screen.Text.Split(' ', '+', '-', 'x', '/');
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
