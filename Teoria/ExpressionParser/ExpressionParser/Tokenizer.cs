using System.Globalization;

namespace ExpressionParser
{
    class TokenException : ApplicationException
    {
        public TokenException(string message) : base(message) { }
    }

    class Token
    {
        public enum TokenType
        {
            Value,       // un valore costante (es: 3.14   .123   1.234E+3)

            Op_Plus,     // l'operatore +
            Op_Minus,    // l'operatore -
            Op_Mult,     // l'operatore *
            Op_Div,      // l'operatore /

            Par_Left,    // la parentesi (
            Par_Right,   // la parentesi )
        }

        public TokenType Type { get; private set; }
        public double Value { get; private set; }  // solo quando Type == TokenType.Value

        private Token(TokenType type, double value)
        {
            Type = type;
            Value = value;
        }
        public Token(double value)
            : this(TokenType.Value, value)
        {
        }

        public Token(TokenType type)
            : this(type, 0.0)
        {
            if (type == TokenType.Value)
                throw new ArgumentException("Wrong type");
        }

        public override string ToString()
        {
            if (Type == TokenType.Value)
                return Value.ToString();

            return Type.ToString();
        }

        // Metodo statico di tokenizzazione
        public static IEnumerable<Token> TokenizeString(string expression)
        {
            // i_curr = indice in expression della posizione corrente
            // i_end = indice fino a cui si estrea un simbolo
            //
            // l'intervallo [i_curr ; i_end] è il range che produce il prossimo simbolo
            for (int i_curr = 0; i_curr < expression.Length;)
            {
                // Ingrora gli spazi
                while (i_curr < expression.Length  && char.IsWhiteSpace(expression[i_curr]))
                    ++i_curr;

                if (i_curr >= expression.Length)
                    break;

                char ch_curr = expression[i_curr];

                if (char.IsDigit(ch_curr) || ch_curr == '.')
                {
                    // Estrae un valore numerico

                    int i_end;
                    for (i_end = i_curr + 1; i_end < expression.Length; ++i_end)
                    {
                        ch_curr = expression[i_end];
                        if (!char.IsDigit(ch_curr) && ch_curr != '.')
                            break;
                    }

                    double value;
                    string value_str = expression.Substring(i_curr, i_end - i_curr);
                    if (double.TryParse(value_str, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                        yield return new Token(TokenType.Value, value);
                    else
                        throw new TokenException($"Unexpected token between indexes {i_curr} and {i_end}");

                    i_curr = i_end;
                }
                else
                {
                    // Può essere un operatore oppure ( )
                    switch (ch_curr)
                    {
                        case '+': yield return new Token(TokenType.Op_Plus);  break;
                        case '-': yield return new Token(TokenType.Op_Minus); break;
                        case '*': yield return new Token(TokenType.Op_Mult); break;
                        case '/': yield return new Token(TokenType.Op_Div); break;

                        case '(': yield return new Token(TokenType.Par_Left); break;
                        case ')': yield return new Token(TokenType.Par_Right); break;

                        default:
                            throw new TokenException($"Unexpected token at index {i_curr}");
                    }

                    ++i_curr;  // avanza di un carattere (ogni operatore occupa un carattere)
                }
            }
        }
    }
}
