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
            Ident,       // un identificatore (es: x, y, e, pi, ...)

            Op_Plus,     // l'operatore +
            Op_Minus,    // l'operatore -
            Op_Mult,     // l'operatore *
            Op_Div,      // l'operatore /
            Op_Pow,      // l'operatore ^

            Par_Left,    // la parentesi (
            Par_Right,   // la parentesi )

            Comma,       // la virgola ,
        }

        public TokenType Type { get; private set; }
        public double Value { get; private set; }  // solo quando Type == TokenType.Value
        public string Ident { get; private set; }  // solo quando Type == TokenType.Ident

        private Token(TokenType type, double value, string ident)
        {
            Type = type;
            Value = value;
            Ident = ident;
        }
        public Token(double value)
            : this(TokenType.Value, value, "")
        {
        }
        public Token(string ident)
            : this(TokenType.Ident, 0.0, ident)
        {
        }

        public Token(TokenType type)
            : this(type, 0.0, "")
        {
            if (type == TokenType.Value || type == TokenType.Ident)
                throw new ArgumentException("Wrong type");
        }

        public override string ToString()
        {
            if (Type == TokenType.Value)
                return String.Format(CultureInfo.InvariantCulture, "{0}", Value);

            if (Type == TokenType.Ident)
                return Ident;

            return Type.ToString();
        }

        // Metodo statico di tokenizzazione
        public static IEnumerable<Token> TokenizeString(string expression)
        {
            // i_curr = indice in expression della posizione corrente
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
                    for (i_end = i_curr + 1; i_end < expression.Length; ++i_end)  // Estrae cifre e '.'
                    {
                        ch_curr = expression[i_end];
                        if (!char.IsDigit(ch_curr) && ch_curr != '.')
                            break;
                    }

                    if (i_end < expression.Length && "eE".Contains(ch_curr))  // Verifica la presenza di un esponente
                    {
                        ++i_end;  // salta 'E' o 'e'

                        if (i_end < expression.Length && "+-".Contains(ch_curr = expression[i_end]))  // Verifica la presenza del segno per l'esponente
                            ++i_end;  // salta '+' o '-'

                        // Cifre dell'esponente
                        for (; i_end < expression.Length; ++i_end)
                        {
                            ch_curr = expression[i_end];
                            if (!char.IsDigit(ch_curr))
                                break;
                        }
                    }
                    // NOTA: ora l'indice i_end è il primo carattere non parte del valore costante,
                    //       cioè vale che l'intervallo [i_curr ; i_end[ è il range che produce il valore costante

                    double value;
                    string value_str = expression.Substring(i_curr, i_end - i_curr);
                    if (double.TryParse(value_str, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                        yield return new Token(value);
                    else
                        throw new TokenException($"Unexpected token between indexes {i_curr} and {i_end}");

                    i_curr = i_end;
                }
                else if (char.IsLetter(ch_curr) || ch_curr == '_')
                {
                    // Estrae un identificatore

                    int i_end;
                    for (i_end = i_curr + 1; i_end < expression.Length; ++i_end)  // Estrae lettere, cifre e '_'
                    {
                        ch_curr = expression[i_end];
                        if (!char.IsLetter(ch_curr) && !char.IsDigit(ch_curr) && ch_curr != '_')
                            break;
                    }

                    string ident = expression.Substring(i_curr, i_end - i_curr);
                    yield return new Token(ident);
                    i_curr = i_end;
                }
                else
                {
                    // Può essere un operatore oppure ( )
                    switch (ch_curr)
                    {
                        case '+': yield return new Token(TokenType.Op_Plus); break;
                        case '-': yield return new Token(TokenType.Op_Minus); break;
                        case '*': yield return new Token(TokenType.Op_Mult); break;
                        case '/': yield return new Token(TokenType.Op_Div); break;
                        case '^': yield return new Token(TokenType.Op_Pow); break;

                        case '(': yield return new Token(TokenType.Par_Left); break;
                        case ')': yield return new Token(TokenType.Par_Right); break;

                        case ',': yield return new Token(TokenType.Comma); break;

                        default:
                            throw new TokenException($"Unexpected token at index {i_curr}");
                    }

                    ++i_curr;  // avanza di un carattere (ogni operatore occupa un carattere)
                }
            }
        }
    }
}
