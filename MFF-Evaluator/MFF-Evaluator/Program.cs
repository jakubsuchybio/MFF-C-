using System;
using System.Collections.Generic;
using System.IO;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("MFF-Evaluator_Tests")]

namespace MFF_Evaluator {

    /// <summary>
    /// Parses input expression with registered operators.
    /// Default operators are +,-,*,/,~
    /// Can register more operators, but they have to have implemented operator classes.
    /// </summary>
    class ExpressionParser {
        /// <summary>
        /// Internal register of operators and theirs node types.
        /// </summary>
        private Dictionary<string, Type> registry = new Dictionary<string, Type>();

        /// <summary>
        /// Constructor which registers default operators.
        /// </summary>
        public ExpressionParser() {
            Register("~", typeof(NegateOperator));
            Register("+", typeof(AddOperator));
            Register("-", typeof(SubtractOperator));
            Register("*", typeof(MultiplyOperator));
            Register("/", typeof(DivideOperator));
        }

        /// <summary>
        /// Adds operator and its node class type into registry.
        /// </summary>
        /// <param name="s">Operator identificator. (e.g. '+')</param>
        /// <param name="t">Node class's type. (e.g. 'typeof(AddOperator)')</param>
        public void Register(string s, Type t) {
            if(registry.ContainsKey(s))
                throw new ArgumentException("Operator already registered.");

            registry.Add(s, t);
        }

        /// <summary>
        /// Removes operator from registry.
        /// </summary>
        /// <param name="s">Operator identificator. (e.g. '+')</param>
        public void UnRegister(string s) {
            if(!registry.ContainsKey(s))
                throw new ArgumentException("Operator have not been previously registered.");

            registry.Remove(s);
        }

        /// <summary>
        /// Tries to Parse input expression in prefix format.
        /// </summary>
        /// <param name="exprString">Input string expression.</param>
        /// <returns>Returns root node of expression tree or null if format error occured.</returns>
        public Node ParsePrefixExpression(string exprString) {
            string[] tokens = exprString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            Node result = null;
            Stack<object> unresolved = new Stack<object>();
            foreach(string token in tokens) {
                if(result != null)
                    return null;

                Type operatorType = null;
                if(registry.TryGetValue(token, out operatorType)) {
                    unresolved.Push(Activator.CreateInstance(operatorType));
                }
                else {
                    int value = 0;
                    if(!int.TryParse(token, out value) || value < 0)
                        return null;

                    Node expr = new ConstantNode(value);
                    while(unresolved.Count > 0) {
                        OperatorNode oper = unresolved.Peek() as OperatorNode;
                        if(oper.AddOperand(expr)) {
                            unresolved.Pop();
                            expr = oper;
                        }
                        else {
                            expr = null;
                            break;
                        }
                    }

                    if(expr != null)
                        result = expr;
                }
            }

            return result;
        }
    }

    #region Visitors

    /// <summary>
    /// Interface for Visitors, that must implements default operators.
    /// </summary>
    /// <typeparam name="T">Numeric generic parameter (e.g. int, double, short, bool, etc...)</typeparam>
    interface IVisitor<T> where T : struct {
        T Visit(ConstantNode node);
        T Visit(NegateOperator node);
        T Visit(AddOperator node);
        T Visit(SubtractOperator node);
        T Visit(MultiplyOperator node);
        T Visit(DivideOperator node);
    }

    /// <summary>
    /// Implementation of visitor for type int.
    /// Implements default Visits for all non-abstract node types.
    /// And remembers result of last evaluation for caching purposes.
    /// </summary>
    sealed class IntVisitor : IVisitor<int> {

        public int lastEvaluation = 0;

        public int Visit(ConstantNode node) {
            lastEvaluation = checked(node.Value);
            return lastEvaluation;
        }

        public int Visit(NegateOperator node) {
            lastEvaluation = checked(-node.Op.Accept(this));
            return lastEvaluation;
        }

        public int Visit(AddOperator node) {
            lastEvaluation = checked(node.Op0.Accept(this) + node.Op1.Accept(this));
            return lastEvaluation;
        }

        public int Visit(SubtractOperator node) {
            lastEvaluation = checked(node.Op0.Accept(this) - node.Op1.Accept(this));
            return lastEvaluation;
        }

        public int Visit(MultiplyOperator node) {
            lastEvaluation = checked(node.Op0.Accept(this) * node.Op1.Accept(this));
            return lastEvaluation;
        }

        public int Visit(DivideOperator node) {
            lastEvaluation = checked(node.Op0.Accept(this) / node.Op1.Accept(this));
            return lastEvaluation;
        }
    }

    /// <summary>
    /// Implementation of visitor for type double.
    /// Implements default Visits for all non-abstract node types.
    /// And remembers result of last evaluation for caching purposes.
    /// </summary>
    sealed class DoubleVisitor : IVisitor<double> {
        public double lastEvaluation = 0;

        public double Visit(ConstantNode node) {
            lastEvaluation = checked(node.Value);
            return lastEvaluation;
        }

        public double Visit(NegateOperator node) {
            lastEvaluation = checked(-node.Op.Accept(this));
            return lastEvaluation;
        }

        public double Visit(AddOperator node) {
            lastEvaluation = checked(node.Op0.Accept(this) + node.Op1.Accept(this));
            return lastEvaluation;
        }

        public double Visit(SubtractOperator node) {
            lastEvaluation = checked(node.Op0.Accept(this) - node.Op1.Accept(this));
            return lastEvaluation;
        }

        public double Visit(MultiplyOperator node) {
            lastEvaluation = checked(node.Op0.Accept(this) * node.Op1.Accept(this));
            return lastEvaluation;
        }

        public double Visit(DivideOperator node) {
            lastEvaluation = checked(node.Op0.Accept(this) / node.Op1.Accept(this));
            return lastEvaluation;
        }
    }

    #endregion

    /// <summary>
    /// Base Node type which have to implement Accept method for visitors.
    /// </summary>
    abstract class Node {
        public abstract T Accept<T>(IVisitor<T> visitor) where T : struct;
    }

    #region Value Nodes

    abstract class ValueNode : Node {
        public abstract int Value { get; }
    }

    /// <summary>
    /// Basic node for Constants, which can be only signed ints.
    /// </summary>
    sealed class ConstantNode : ValueNode {
        private int value;

        public ConstantNode(int value) {
            this.value = value;
        }
    
        public override int Value {
	        get { return this.value; }
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.Visit(this);
        }
    }
 
    #endregion

    abstract class OperatorNode : Node {
        public abstract bool AddOperand(Node op);
    }

    #region Unary Operations

    /// <summary>
    /// Abstract structure for representation of Unary operators
    /// </summary>
    abstract class UnaryOperator : OperatorNode {
        protected Node op;

		public Node Op {
			get { return op; }
			set { op = value; }
		}

		public sealed override bool AddOperand(Node op) {
			if (this.op == null) {
				this.op = op;
			}
			return true;
		}
    }

    sealed class NegateOperator : UnaryOperator {
        public sealed override T Accept<T>(IVisitor<T> visitor) {
            return visitor.Visit(this);
        }
    }

    #endregion

    #region Binary Operations

    /// <summary>
    /// Abstract structure for representation of Binary operators
    /// </summary>
    abstract class BinaryNode : OperatorNode {
        protected Node op0, op1;

        public Node Op0 {
            get { return op0; }
            set { op0 = value; }
        }

        public Node Op1 {
            get { return op1; }
            set { op1 = value; }
        }

        public sealed override bool AddOperand(Node op) {
            if(op0 == null) {
                op0 = op;
                return false;
            }
            else if(op1 == null) {
                op1 = op;
            }
            return true;
        }
    }

    sealed class AddOperator : BinaryNode {
        public sealed override T Accept<T>(IVisitor<T> visitor) {
            return visitor.Visit(this);
        }
    }

    sealed class SubtractOperator : BinaryNode {
        public sealed override T Accept<T>(IVisitor<T> visitor) {
            return visitor.Visit(this);
        }
    }

    sealed class DivideOperator : BinaryNode {
        public sealed override T Accept<T>(IVisitor<T> visitor) {
            return visitor.Visit(this);
        }
    }

    sealed class MultiplyOperator : BinaryNode {
        public sealed override T Accept<T>(IVisitor<T> visitor) {
            return visitor.Visit(this);
        }
    }

    #endregion

    class Program {
        internal static string SLNPath = @"c:\Users\Jakub\Documents\Visual Studio 2013\Projects\MFF-Evaluator\";

        /// <summary>
        /// Implementation for codex "Vyhodnocovani vyrazu"
        /// Has parametrized input and output streams for testing purposes of evaluating in int precision.
        /// </summary>
        /// <param name="input">Input stream. (e.g. Console.In)</param>
        /// <param name="output">Output stream. (e.g. Console.Out)</param>
        public static void RunInt(TextReader input, TextWriter output) {
            string expression = input.ReadLine();
            try {
                ExpressionParser parser = new ExpressionParser();
                IntVisitor visitor = new IntVisitor();
                Node root = parser.ParsePrefixExpression(expression);
                if(root == null)
                    throw new FormatException();
                output.WriteLine(root.Accept(visitor));
            }
            catch(OverflowException) {
                output.WriteLine("Overflow Error");
            }
            catch(DivideByZeroException) {
                output.WriteLine("Divide Error");
            }
            catch(FormatException) {
                output.WriteLine("Format Error");
            }
        }

        /// <summary>
        /// Part of implementation for codex "Vyhodnocovani vyrazu II"
        /// Has parametrized input and output streams for testing purposes of evaluating in double precision.
        /// </summary>
        /// <param name="input">Input stream. (e.g. Console.In)</param>
        /// <param name="output">Output stream. (e.g. Console.Out)</param>
        public static void RunDouble(TextReader input, TextWriter output) {
            string expression = input.ReadLine();
            try {
                ExpressionParser parser = new ExpressionParser();
                DoubleVisitor visitor = new DoubleVisitor();
                Node root = parser.ParsePrefixExpression(expression);
                if(root == null)
                    throw new FormatException();
                output.WriteLine(root.Accept(visitor));
            }
            catch(OverflowException) {
                output.WriteLine("Overflow Error");
            }
            catch(DivideByZeroException) {
                output.WriteLine("Divide Error");
            }
            catch(FormatException) {
                output.WriteLine("Format Error");
            }
        }
        
        /// <summary>
        /// Implementation for codex "Vyhodnocovani vyrazu II"
        /// Has parametrized input and output streams for testing purposes.
        /// Evaluates line by line for lines like:
        /// "= 'valid expression'" which will parse expression for further evaluation
        /// "i" which evaluates parsed expression in int precission
        /// "d" which evaluates parsed expression in double precission
        /// </summary>
        /// <param name="input">Input stream. (e.g. Console.In)</param>
        /// <param name="output">Output stream. (e.g. Console.Out)</param>
        public static void Run(TextReader input, TextWriter output) {
            var parser = new ExpressionParser();
            var doubleVisitor = new DoubleVisitor();
            var intVisitor = new IntVisitor();
            
            Node lastExpressionTree = null;
            bool intEvaluated = false;
            bool doubleEvaluated = false;

            string expression = input.ReadLine();
            while(expression != null && expression != "end") {
                if(expression.Length == 0) {
                    expression = input.ReadLine();
                    continue;
                }

                try {
                    switch(expression[0]) {
                        case '=':
                            lastExpressionTree = null;
                            intEvaluated = false;
                            doubleEvaluated = false;

                            if(expression.Length <= 2 || expression[1] != ' ')
                                throw new FormatException();
                            
                            lastExpressionTree = parser.ParsePrefixExpression(expression.Substring(1));
                            if(lastExpressionTree == null)
                                throw new FormatException();

                            break;
                        case 'i':
                            if(expression.Length > 1)
                                throw new FormatException();

                            if(lastExpressionTree == null)
                                throw new NullReferenceException();

                            if(intEvaluated)
                                output.WriteLine(intVisitor.lastEvaluation);
                            else
                                output.WriteLine(lastExpressionTree.Accept(intVisitor));

                            intEvaluated = true;
                            break;
                        case 'd':
                            if(expression.Length > 1)
                                throw new FormatException();

                            if(lastExpressionTree == null)
                                throw new NullReferenceException();

                            if(doubleEvaluated)
                                output.WriteLine("{0:F5}", doubleVisitor.lastEvaluation);
                            else
                                output.WriteLine("{0:F5}", lastExpressionTree.Accept(doubleVisitor));

                            doubleEvaluated = true;
                            break;
                        default:
                            throw new FormatException();
                    }
                }
                catch(NullReferenceException) {
                    output.WriteLine("Expression Missing");
                }
                catch(OverflowException) {
                    output.WriteLine("Overflow Error");
                }
                catch(DivideByZeroException) {
                    output.WriteLine("Divide Error");
                }
                catch(FormatException) {
                    output.WriteLine("Format Error");
                }

                expression = input.ReadLine();
            }
        }

        static void Main(string[] args) {
            Run(Console.In, Console.Out);
        }
    }
}
