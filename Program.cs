using System.Text;

namespace ReversePolishNotation
{
    class Program
    {
        /// Evaluate expression in Reverse Polish Notation
        public static double EvaluateRPN(string expression)
        {
            Stack<double> stack = new Stack<double>();
            string[] tokens = expression.Split(' ');

            foreach (string token in tokens)
            {
                if (double.TryParse(token, out double value))
                {
                    stack.Push(value);
                }
                else
                {
                    double right = stack.Pop();
                    double left = stack.Pop();

                    switch (token)
                    {
                        case "+":
                            stack.Push(left + right);
                            break;
                        case "-":
                            stack.Push(left - right);
                            break;
                        case "*":
                            stack.Push(left * right);
                            break;
                        case "/":
                            stack.Push(left / right);
                            break;
                        case "^":
                            stack.Push(Math.Pow(left, right));
                            break;
                    }
                }
            }
            double result = stack.Pop();
            return result;
        }

        // Using Shunting Yard algorithm by Edsger Dijkstra in 1961
        static string ConvertToRPN(string expression)
        {
            string[] tokens = expression.Split(' ');

            Stack<string> operators = new Stack<string>();
            Queue<string> output = new Queue<string>();

            foreach (string token in tokens)
            {
                double number;
                if (double.TryParse(token, out number))
                {
                    output.Enqueue(token);
                }
                else if (token == "(")
                {
                    operators.Push(token);
                }
                else if (token == ")")
                {
                    while (operators.Peek() != "(")
                    {
                        output.Enqueue(operators.Pop());
                    }
                    operators.Pop();
                }
                else
                {
                    while (operators.Count > 0 && GetPrecedence(token) <= GetPrecedence(operators.Peek()))
                    {
                        output.Enqueue(operators.Pop());
                    }
                    operators.Push(token);
                }
            }
            while (operators.Count > 0)
            {
                output.Enqueue(operators.Pop());
            }
            return string.Join(" ", output);
        }


        private static int GetPrecedence(string op)
        {
            switch (op)
            {
                case "+":
                case "-":
                    return 1;
                case "*":
                case "/":
                    return 2;
                case "^":
                    return 3;
                default:
                    return 0;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Evaluating RPN with Dijkstra Algorithm");
            const string eval = "3 + 4 * 2 / ( 1 - 5 ) ^ 2 ^ 3";
            Console.WriteLine(eval);
            Console.WriteLine(ConvertToRPN(eval));
            Console.WriteLine(EvaluateRPN(ConvertToRPN(eval)));
        }
    }
}
