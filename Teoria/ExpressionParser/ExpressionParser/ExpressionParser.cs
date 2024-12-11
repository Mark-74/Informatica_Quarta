using System.Collections.Generic;

namespace ExpressionParser
{
    delegate double UnaryFunction(double parameter);
    class EvalContext
    {
        public Dictionary<string, double> Constants { get; set; }
        public Dictionary<string, UnaryFunction> Functions { get; set; }
        public EvalContext()
        {
            Constants = new Dictionary<string, double>();
            Functions = new Dictionary<string, UnaryFunction>();
        }
    }

    class ExpressionException : ApplicationException
    {
        public ExpressionException(string message) : base(message) { }
    }
    class ExpressionParser
    {
        public abstract class Term
        {
            public abstract double Evaluate(EvalContext ctx);
        }
        public class ValueTerm : Term
        {
            public double Value { get; private set; }
            public ValueTerm(double value) { Value = value; }
            public override double Evaluate(EvalContext ctx) { return Value; }
        }
        public class IdentTerm : Term
        {
            public string Ident { get; private set; }
            public IdentTerm(string ident) { Ident = ident; }
            public override double Evaluate(EvalContext ctx) { return ctx.Constants[Ident]; }
        }
        public class FunctionTerm : Term
        {
            public string FunctionName { get; private set; }
            public List<Term> Parameters { get; private set; }
            public FunctionTerm(string functionName, List<Term> parameters) { FunctionName = functionName; Parameters = parameters; }
            public override double Evaluate(EvalContext ctx) { return ctx.Functions[FunctionName](Parameters[0].Evaluate(ctx)); }
        }
        public abstract class BinaryTerm : Term  // BinaryTerm = un Term con due operandi
        {
            protected Term OperandLeft { get; private set; }
            protected Term OperandRight { get; private set; }

            public BinaryTerm(Term left, Term right)
            {
                OperandLeft = left;
                OperandRight = right;
            }
        }
        public class SumTerm : BinaryTerm { public SumTerm(Term left, Term right) : base(left, right) { } public override double Evaluate(EvalContext ctx) { return OperandLeft.Evaluate(ctx) + OperandRight.Evaluate(ctx); } }
        public class DiffTerm : BinaryTerm { public DiffTerm(Term left, Term right) : base(left, right) { } public override double Evaluate(EvalContext ctx) { return OperandLeft.Evaluate(ctx) - OperandRight.Evaluate(ctx); } }
        public class MultTerm : BinaryTerm { public MultTerm(Term left, Term right) : base(left, right) { } public override double Evaluate(EvalContext ctx) { return OperandLeft.Evaluate(ctx) * OperandRight.Evaluate(ctx); } }
        public class DivTerm : BinaryTerm { public DivTerm(Term left, Term right) : base(left, right) { } public override double Evaluate(EvalContext ctx) { return OperandLeft.Evaluate(ctx) / OperandRight.Evaluate(ctx); } }
        public class PowTerm : BinaryTerm { public PowTerm(Term _base, Term exp) : base(_base, exp) { } public override double Evaluate(EvalContext ctx) { return Math.Pow(OperandLeft.Evaluate(ctx), OperandRight.Evaluate(ctx)); } }

        /*
            Grammatica:

                expr:
                    expr-sum

                expr-sum:
                    expr-mult
                    expr-sum + expr-mult
                    expr-sum - expr-mult

                expr-mult:
                    expr-power
                    expr-mult * expr-power
                    expr-mult / expr-power

                expr-power:
                    expr-function
                    expr-power ^ expr-function

                expr-function:
                    expr-primary
                    expr-primary (expr, ...)     <<-- la lista parametri è opzionale, potrebbe essere ()

                expr-primary:
                    const-value      <<-- 3.14  0.1E+7  ecc...
                    identifier       <<-- x, y oppure e, pi
                    (expr)
         */

        private Term Expr()
        {
            return ExprSum();
        }

        private Term ExprSum()
        {
            Term op_left = ExprMult();

            if (CurrToken == null)  // l'espressione finisce qui!
                return op_left;

            // Qui vale (CurrToken != null)
            if (CurrToken.Type == Token.TokenType.Op_Plus)
            {
                AdvanceToken();  // estrare il '+'
                Term op_right = ExprSum();
                return new SumTerm(op_left, op_right);
            }
            else if (CurrToken.Type == Token.TokenType.Op_Minus)
            {
                AdvanceToken();  // estrare il '-'
                Term op_right = ExprSum();
                return new DiffTerm(op_left, op_right);
            }

            return op_left;  // lasciamo al livello superiore la gestione del token 
        }

        private Term ExprMult()
        {
            Term op_left = ExprPower();

            if (CurrToken == null)  // l'espressione finisce qui!
                return op_left;

            // Qui vale (CurrToken != null)
            if (CurrToken.Type == Token.TokenType.Op_Mult)
            {
                AdvanceToken();  // estrare il '*'
                Term op_right = ExprMult();
                return new MultTerm(op_left, op_right);
            }
            else if (CurrToken.Type == Token.TokenType.Op_Div)
            {
                AdvanceToken();  // estrare il '/'
                Term op_right = ExprMult();
                return new DivTerm(op_left, op_right);
            }

            return op_left;  // lasciamo al livello superiore la gestione del token 
        }
        private Term ExprPower()
        {
            Term op_left = ExprFunction();

            if (CurrToken == null)  // l'espressione finisce qui!
                return op_left;

            // Qui vale (CurrToken != null)
            if (CurrToken.Type == Token.TokenType.Op_Pow)
            {
                AdvanceToken();  // estrare il '^'
                Term op_right = ExprPower();
                return new PowTerm(op_left, op_right);
            }

            return op_left;  // lasciamo al livello superiore la gestione del token 
        }
        private Term ExprFunction()
        {
            Term op_left = ExprPrimary();

            if (CurrToken == null)  // l'espressione finisce qui!
                return op_left;

            // Qui vale (CurrToken != null)
            if (op_left is IdentTerm  &&  CurrToken.Type == Token.TokenType.Par_Left)
            {
                IdentTerm function = op_left as IdentTerm;

                AdvanceToken();  // estrare il '('

                // estrae la lista parametri
                List<Term> parameters = new List<Term>();
                while (CurrToken != null)
                {
                    if (CurrToken.Type == Token.TokenType.Par_Right)
                        break;

                    if (parameters.Count > 0)  // per i parametri successivi al primo è richiesta la ,
                    {
                        if (CurrToken.Type != Token.TokenType.Comma)
                            throw new ExpressionException("Missing ,");
                        AdvanceToken();
                    }

                    parameters.Add(Expr());
                }

                // Verifica la ')'
                Token tok = CurrToken;
                AdvanceToken();

                if (tok == null)
                    throw new ExpressionException("Missing )");
                if (tok.Type != Token.TokenType.Par_Right)
                    throw new ExpressionException($"Unexpected {tok} token");

                return new FunctionTerm(function.Ident, parameters);
            }

            return op_left;  // lasciamo al livello superiore la gestione del token 
        }
        private Term ExprPrimary()
        {
            Token tok = CurrToken;  // un token ci deve essere per forza
            AdvanceToken();

            if (tok == null)
                throw new ExpressionException("Fine inattesa dell'espressione");

            if (tok.Type == Token.TokenType.Value)
                return new ValueTerm(tok.Value);

            if (tok.Type == Token.TokenType.Ident)
                return new IdentTerm(tok.Ident);

            if (tok.Type == Token.TokenType.Par_Left)
            {
                Term t = Expr();  // estrazione dell'espressione all'interno delle parentesi

                // Verifica la ')'
                tok = CurrToken;
                AdvanceToken();

                if (tok == null)
                    throw new ExpressionException("Missing )");
                if (tok.Type != Token.TokenType.Par_Right)
                    throw new ExpressionException($"Unexpected {tok} token");

                return t;
            }

            throw new ExpressionException($"Unexpected {tok} token");
        }




        private IEnumerator<Token> tokens;      // enumeratore dei tokens per l'espressione passata nel costruttore
        private Token CurrToken { get; set; }    // token corrente
        private void AdvanceToken()             // passa al token successivo
        {
            CurrToken = (tokens.MoveNext() ? tokens.Current : null);
        }
        public ExpressionParser(string expr)
        {
            this.tokens = Token.TokenizeString(expr).GetEnumerator();  // crea l'enumeratore per l'espressione expr
            AdvanceToken();  // carica il primo token
        }
        public Term EvaluateExpression(string expr)
        {
            return Expr();
        }
    }
}