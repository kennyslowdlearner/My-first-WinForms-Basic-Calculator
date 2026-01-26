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

        private void Number_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (displayAnswer == true)
            {
                screen.Clear();
                displayAnswer = false;
            }

            string[] parts = screen.Text.Split(' ');
            string currentNumber = parts[parts.Length - 1];

            if (currentNumber == "0" || currentNumber == "00")
                screen.Text = screen.Text.Remove(screen.Text.Length - 1) + button.Text;

            else
                screen.Text += button.Text;

            screen.Focus();
            screen.SelectionStart = screen.Text.Length;
            screen.ScrollToCaret();
        }

        private void Operator_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string newOperation = button.Text;

            if (string.IsNullOrEmpty(screen.Text))
            {
                if (newOperation == "-")
                {
                    screen.Text = "-";
                    screen.Focus();
                    screen.SelectionStart = screen.Text.Length;
                    screen.ScrollToCaret();
                }

                return;
            }

            if (screen.Text.EndsWith(" "))
            {
                if (newOperation == "-")
                {
                    screen.Text += "-";
                    screen.Focus();
                    screen.SelectionStart = screen.Text.Length;
                    screen.ScrollToCaret();
                }

                return;
            }

            string[] parts = screen.Text.Split(' ');
            string currentNumber = parts[parts.Length - 1];

            if (currentNumber == "-")
                return;

            if (screen.Text == "0")
                return;

            operation = newOperation;

            screen.Text += " " + operation.ToString() + " ";

            screen.Focus();
            screen.SelectionStart =  screen.Text.Length;
            screen.ScrollToCaret();
        }

        private void Result(object sender, EventArgs e) 
        {
            // TO DO
            // gonna study this one first
            // putting negative numbers and decimals aside for now -> okay na
            // multiline problems -> di ma scroll down

            if (string.IsNullOrEmpty(screen.Text) || screen.Text.EndsWith(" "))
                return;

            List<string> parts = screen.Text.Split(' ').ToList();

            try
            {
                for (int a = 0; a < parts.Count; a++)
                {
                    if (parts[a] == "x" || parts[a] == "/")
                    {
                        double integerOne = double.Parse(parts[a - 1]);
                        double integerTwo = double.Parse(parts[a + 1]);
                        double tempResult = 0;

                        if (parts[a] == "x")
                            tempResult = integerOne * integerTwo;

                        else if (integerTwo == 00)
                            throw new DivideByZeroException();

                        else if (parts[a] == "/")
                        {
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

                    if (parts[a] == "+")
                        currentResult += solveNext;

                    else if (parts[a] == "-")
                        currentResult -= solveNext;
                }


                screen.Text = currentResult.ToString();
                displayAnswer = true;
            }
            catch (DivideByZeroException)
            {
                screen.Text = "Error";
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
            if (screen.Text.Length > 0)
                screen.Text = screen.Text.Remove(screen.Text.Length - 1);

            if (screen.Text == " ")
                screen.Text = "";
        }

        private void Decimal_Click(object sender, EventArgs e)
        {
            string[] parts = screen.Text.Split(' ');
            string currentNumber = parts[parts.Length - 1];

            if (currentNumber.Contains("."))
                return;

            if (currentNumber == "-")
                screen.Text += "0.";
            else
                screen.Text += ".";
        }

        private void doubleZero_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            string[] parts = screen.Text.Split(' ');
            string currentNumber = parts[parts.Length - 1];

            if (currentNumber == "0")
                return;

            screen.Text += button.Text;

            screen.Focus();
            screen.SelectionStart = screen.Text.Length;
            screen.ScrollToCaret();
        }


    }
}
