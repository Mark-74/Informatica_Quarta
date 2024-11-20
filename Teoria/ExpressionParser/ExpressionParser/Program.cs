namespace ExpressionParser
{
    internal class Program
    {
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

        static void Main(string[] args)
        {
            TestTokenizer("3+7*2");
            TestTokenizer("   3   + 7*   2");
            TestTokenizer("(3+7)*2");

            TestTokenizer("3.14");
            TestTokenizer(".14");
            TestTokenizer("3.14E+3");  // TODO : ora non funziona, ma dovrà funzionare

            TestTokenizer("4 * / - a 123");
        }
    }
}
