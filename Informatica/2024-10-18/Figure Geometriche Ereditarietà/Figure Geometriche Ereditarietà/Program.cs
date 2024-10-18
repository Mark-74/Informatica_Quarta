/*
 * Marco Balducci 4H 2024-10-18
 * Esercizio su ereditarietà
*/

using System.Security.Principal;

namespace Figure_Geometriche_Ereditarietà
{
    abstract class Figure
    {
        protected string _name;

        public Figure(string name)
        {
            _name = name;
        }

        public abstract double Area();
        public abstract double Perimeter();

        public string Name() => _name;
    }

    class Triangolo : Figure
    {
        private double _base, _side1, _side2, _height;

        public Triangolo(string name, double @base, double side1, double side2, double height)
            : base(name)
        {
            _base = @base;
            _side1 = side1;
            _side2 = side2;
            _height = height;
        }

        public override double Area() => _base * _height / 2;
        public override double Perimeter() => _base + _side1 + _side2;
    }

    class Rettangolo : Figure
    {
        private double _base, _height;

        public Rettangolo(string name, double @base, double height)
            : base(name)
        {
            _base = @base;
            _height = height;
        }

        public override double Area() => _base * _height;
        public override double Perimeter() => _base * 2 + _height * 2;
    }

    class Quadrato : Rettangolo
    {
        public Quadrato(string name, double side)
            : base(name, side, side) { }

    }

    class Cerchio : Figure
    {
        private double _radius;

        public Cerchio(string name, double radius)
            : base(name)
        {
            _radius = radius;
        }

        public override double Area() => 2*Math.PI * (_radius*_radius);
        public override double Perimeter() => 2*Math.PI * _radius;
    }

    class Rombo : Figure
    {
        private double majorDiagonal, minorDiagonal;

        public Rombo(string name, double majorDiagonal, double minorDiagonal)
            : base(name)
        {
            this.majorDiagonal = majorDiagonal;
            this.minorDiagonal = minorDiagonal;
        }

        public override double Area() => majorDiagonal*minorDiagonal / 2;
        public override double Perimeter() => Math.Sqrt(Math.Pow(majorDiagonal / 2, 2) + Math.Pow(minorDiagonal/2, 2));
    }

    class Trapezio : Figure
    {
        private double _majorBase, _minorBase, _side1, _side2, _height;

        public Trapezio(string name, double majorBase, double minorBase, double side1, double side2, double height)
            : base(name)
        {
            _majorBase = majorBase;
            _minorBase = minorBase;
            _side1 = side1;
            _side2 = side2;
            _height = height;
        }

        public override double Area() => _majorBase * _minorBase / 2;
        public override double Perimeter() => _majorBase + _minorBase + _side1 + _side2;
    }

    internal class Program
    {

    }
}
