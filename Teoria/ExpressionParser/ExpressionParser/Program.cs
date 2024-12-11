using ExpressionParser;

namespace ExpressionParser
{
    internal class Program
    {
        static EvalContext BuildContext()
        {
            EvalContext ctx = new EvalContext();

            ctx.Constants.Add("e",  Math.E);
            ctx.Constants.Add("pi", Math.PI);

            ctx.Functions.Add("sin", Math.Sin);
            ctx.Functions.Add("cos", Math.Cos);
            ctx.Functions.Add("tan", Math.Tan);
            ctx.Functions.Add("ln",  Math.Log);
            ctx.Functions.Add("log", Math.Log10);
            return ctx;
        }
        static void TestTokenizer(string expr)
        {
            Console.Write($"{expr} -->> ");                                  

            try
            {
                foreach (Token tok in Token.TokenizeString(expr))
                {
                    Console.Write($"{tok.ToString()} ");
                }
            }
            catch (Exception e)
            {
                Console.Write($"Exception '{e.Message}'");
            }

            Console.WriteLine();
        }

        static void TestExpressionParser(string expr)
        {
            EvalContext ctx = BuildContext();

            Console.Write($"{expr} -->> ");

            try
            {
                ExpressionParser parser = new ExpressionParser(expr);
                ExpressionParser.Term term = parser.EvaluateExpression(expr);
                Console.Write(term.Evaluate(ctx));
            }
            catch (Exception e)
            {
                Console.Write($"Exception '{e.Message}'");
            }

            Console.WriteLine();
        }

        static void PlotExpression(string expr, double range_min, double range_max, double step)
        {
            EvalContext ctx = BuildContext();
            ctx.Constants.Add("x", 0.0);

            Console.WriteLine($"----- {expr} in range [{range_min} ; {range_max}] -----");
            try
            {
                ExpressionParser parser = new ExpressionParser(expr);
                ExpressionParser.Term term = parser.EvaluateExpression(expr);

                for (double x = range_min; x <= range_max; x += step)
                {
                    ctx.Constants["x"] = x;
                    Console.WriteLine($"{x} {term.Evaluate(ctx)}");
                }
            }
            catch (Exception e)
            {
                Console.Write($"Exception '{e.Message}'");
            }
            Console.WriteLine($"-----------------------------");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("------ Test del Tokenizzatore ------");
            TestTokenizer("3+7*2");
            TestTokenizer("   3   + 7*   2");
            TestTokenizer("(3+7)*2");

            TestTokenizer("3.14");
            TestTokenizer(".14");
            TestTokenizer("3.14E+3");

            TestTokenizer("4 * / - a 123");

            TestTokenizer("4 * x");

            Console.WriteLine("------ Test del Parser ------");
            TestExpressionParser("1 + 2 * 4");
            TestExpressionParser("(1 + 2) * 4");
            TestExpressionParser("1 + 2 + 4");
            TestExpressionParser("1 + 2^4");
            TestExpressionParser("(1 + 2)^4");
            TestExpressionParser("3 * 2^4");
            TestExpressionParser("(3 * 2)^4");
            TestExpressionParser("3 ^ 2^4");
            TestExpressionParser("2 ^ .5");

            TestExpressionParser("1 + 2 + 4)");  // << error
            TestExpressionParser(")1 + 2 + 4");  // << error
            TestExpressionParser("(1 + 2 + 4");  // << error
            TestExpressionParser("2 ^");  // << error

            TestExpressionParser("4 * pi");
            TestExpressionParser("e * .5");
            TestExpressionParser("sin(pi/2)");
            TestExpressionParser("cos(pi/2)");

            TestExpressionParser("");  // << error

            PlotExpression("sin(x)", -3.1415, +3.1415, 0.1);
            PlotExpression("x * (x+1) * (x-1)", -2, +2, 0.1);
        }
    }
}
