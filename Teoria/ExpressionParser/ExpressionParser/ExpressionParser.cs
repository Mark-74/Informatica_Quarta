using System.Data;

namespace ExpressionParser
{
    class ExpressionParser
    {
        public abstract class Term
        {
            public abstract double Evaluate();
        }
        public class ValueTerm : Term
        {
            public double Value { get; private set; }
            public override double Evaluate() { return Value; }
        }
        public abstract class BinaryTerm : Term  // BinaryTerm = un Term con due operandi
        {
            protected Term OperandLeft { get; private set; }
            protected Term OperandRight { get; private set; }
        }
        public class SumTerm : BinaryTerm { public override double Evaluate() { return OperandLeft.Evaluate() + OperandRight.Evaluate(); } }
        public class DiffTerm : BinaryTerm { public override double Evaluate() { return OperandLeft.Evaluate() - OperandRight.Evaluate(); } }
        public class MultTerm : BinaryTerm { public override double Evaluate() { return OperandLeft.Evaluate() * OperandRight.Evaluate(); } }
        public class DivTerm : BinaryTerm { public override double Evaluate() { return OperandLeft.Evaluate() / OperandRight.Evaluate(); } }
    }
}
