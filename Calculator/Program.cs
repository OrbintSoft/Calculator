using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Calculator.Tests")]
namespace Calculator
{
    /// <summary>
    /// This program compute some calculations in console mode
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Entry point of the application
        /// </summary>
        /// <param name="args">Provide arguments to control the appucation, "-h" to get help</param>
        /// <returns>
        /// 0: The program has been executed with success
        /// -1: an Error has occurred
        /// </returns>
        internal static int Main(string[] args)
        {
            try
            {
                LocalizationManager.Init();
                ExecuteCommand(args);
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// Execute the right command in base of provided arguments
        /// </summary>
        /// <param name="args">Commands to be executed</param>
        internal static void ExecuteCommand(string[] args)
        {
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "--help":
                    case "-h":
                    case "-?":
                        PrintDocumentation();
                        break;
                    //I break back compability, because the command 'interacive' can generate conflict in case the user wants
                    //load a file with same name. That should not be a problem since in interactive mode the user is free to input commands
                    case "--interactive":
                    case "-i":
                        InteractiveMode();
                        break;
                    case "-file":
                    case "-f":
                        if (args.Length > 1)
                        {
                            string path = args[1];
                            OperationsEnum op = OperationsEnum.Addition; //Default for backward compability
                            if (args.Length > 2)
                            {
                                if (int.TryParse(args[2], out int opcode) && Enum.IsDefined(typeof(OperationsEnum), opcode))
                                {
                                    op = (OperationsEnum)opcode;
                                    FileInput(args[1], op);
                                }
                                else
                                {
                                    ArgumentError();
                                }
                            }
                            break;
                        }
                        else
                        {
                            ArgumentError();
                            break;
                        }
                    default:
                        //For backward compability if a valid file name is passed without arguments I load from file
                        if (File.Exists(args[0])) 
                        {
                            FileInput(args[0], OperationsEnum.Addition);
                            break; ;
                        }
                        else
                        {
                            ArgumentError();
                            break;
                        }
                }
            }
            else
            {
                throw new ArgumentException(LocalizationManager.GetString("No argument provided, type --help to get list of available commands"));
            }
        }

        /// <summary>
        /// Generate an error because wrong arguments or format are passed
        /// </summary>
        /// <returns></returns>
        private static void ArgumentError()
        {
            throw new ArgumentException(LocalizationManager.GetString("Wrong arguments provided, type --help to get list of available commands"));
        }

        /// <summary>
        /// Starts console interactive mode and allow the user to chose the desidered operation
        /// </summary>
        private static void InteractiveMode()
        {
            string input = "";
            do
            {
                Console.WriteLine();
                Console.WriteLine(LocalizationManager.GetString("Chose an operation"));
                Console.WriteLine();
                Console.WriteLine(string.Format(LocalizationManager.GetString("{0}: Addition"),       (int)OperationsEnum.Addition),       OperationsEnum.Addition.ToString());
                Console.WriteLine(string.Format(LocalizationManager.GetString("{0}: Substraction"),   (int)OperationsEnum.Substraction),   OperationsEnum.Substraction.ToString());
                Console.WriteLine(string.Format(LocalizationManager.GetString("{0}: Multiplication"), (int)OperationsEnum.Multiplication), OperationsEnum.Multiplication.ToString());
                Console.WriteLine(string.Format(LocalizationManager.GetString("{0}: Division"),       (int)OperationsEnum.Division),       OperationsEnum.Division.ToString());
                Console.WriteLine(string.Format(LocalizationManager.GetString("{0}: Power"),          (int)OperationsEnum.Power),          OperationsEnum.Power.ToString());
                Console.WriteLine(string.Format(LocalizationManager.GetString("{0}: Root"),           (int)OperationsEnum.Root),           OperationsEnum.Root.ToString());
                Console.WriteLine(string.Format(LocalizationManager.GetString("{0}: Logarithm"),      (int)OperationsEnum.Logarithm),      OperationsEnum.Logarithm.ToString());
                Console.WriteLine(string.Format(LocalizationManager.GetString("{0}: Expression"),     (int)OperationsEnum.Expression),     OperationsEnum.Expression.ToString());
                Console.WriteLine("---------------------");
                Console.WriteLine(LocalizationManager.GetString("type 'q' to quit the program"));
                input = Console.ReadLine();
            } while (input != "q" && !ChooseOperation(input));           
        }

        /// <summary>
        /// Determinates the operation to b executed based on input
        /// </summary>
        /// <param name="input">Operation code</param>
        /// <returns>True if input is valid</returns>
        private static bool ChooseOperation(string input)
        {
            if (int.TryParse(input, out int opcode) && Enum.IsDefined(typeof(OperationsEnum), opcode))
            {
                if ((OperationsEnum)opcode == OperationsEnum.Expression)
                {
                    InteractiveExpression();
                }
                else
                {
                    InteractiveOperation((OperationsEnum)opcode);                                  
                }
                return true;
            }
            else
            {
                Console.WriteLine(LocalizationManager.GetString("Operation not supported"));
                return false;
            }
        }

        /// <summary>
        /// Gets the required input operands and execute the operation
        /// </summary>
        /// <param name="operation">Operation to be executed</param>
        private static void InteractiveOperation(OperationsEnum operation)
        {
            double input1 = ReadOperandInteractive(1);
            double input2 = ReadOperandInteractive(2);
            double result = ExecuteBasicOperation(operation, input1, input2);
            PrintResult(result);
        }

        /// <summary>
        /// Execute an operation between two operands
        /// </summary>
        /// <param name="operation">Chosed Operation</param>
        /// <param name="operand1">Operand 1</param>
        /// <param name="operand2">Operand 2</param>
        /// <returns>The result of the operation</returns>
        private static double ExecuteBasicOperation(OperationsEnum operation, double operand1, double operand2)
        {
            switch (operation)
            {
                case OperationsEnum.Addition:
                    return operand1 + operand2;
                case OperationsEnum.Substraction:
                    return operand1 - operand2;
                case OperationsEnum.Multiplication:
                    return operand1 * operand2;
                case OperationsEnum.Division:
                    return operand1 / operand2;
                case OperationsEnum.Power:
                    return Math.Pow(operand1, operand2);
                case OperationsEnum.Root:
                    return Math.Exp(Math.Log(Convert.ToDouble(operand1), Math.E) / Convert.ToDouble(operand2));
                case OperationsEnum.Logarithm:
                    return Math.Log(operand1, operand2);
                default:
                    throw new NotImplementedException(LocalizationManager.GetString("Operation not supported"));
            }
        }

        /// <summary>
        /// Request tjhe user to inpu the operand
        /// </summary>
        /// <param name="opNumber"></param>
        /// <returns>
        /// The operand read by console
        /// </returns>
        private static double ReadOperandInteractive(int opNumber)
        {
            do
            {
                Console.WriteLine(string.Format(LocalizationManager.GetString("Insert the operand {0}:"), opNumber));
                string op = Console.ReadLine();
                if (double.TryParse(op, NumberStyles.AllowDecimalPoint ,CultureInfo.InvariantCulture, out double number))
                {
                    return number;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine(string.Format(LocalizationManager.GetString("{0} is not a valid number"), op));
                }
            } while (true);
        }

        /// <summary>
        /// Get input expression from console, checke the input and print the result
        /// </summary>
        private static void InteractiveExpression()
        {
            Console.WriteLine();
            Console.WriteLine(LocalizationManager.GetString("Insert the desidered expression:"));
            string input = Console.ReadLine();
            double result = CalculateExpression(input);
            PrintResult(result);                   
        }

        /// <summary>
        /// Parse the input and calculated the result based on expression
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static double CalculateExpression(string input)
        {
            ExpressionCalculator exc = new ExpressionCalculator();
            return exc.GetResult(input);            
        }

        /// <summary>
        /// Read all operands found in the file and compute the chosen expression, then prints the result
        /// </summary>
        /// <param name="path">Path of file</param>
        /// <param name="operation">Chosed operation</param>
        private static void FileInput(string path, OperationsEnum operation)
        {
            double result = 0;
            if (File.Exists(path))
            {
                if (operation == OperationsEnum.Expression)
                {
                    result = CalculateExpression(File.ReadAllText(path));
                }
                else
                {
                    string[] operators = File.ReadAllLines(path);
                    double[] inputs = operators.Select(o => double.Parse(o, CultureInfo.InvariantCulture)).ToArray();
                    if (inputs.Count() > 1)
                    {
                        result = inputs[0];
                        for (int i = 1; i < inputs.Count(); i++)
                        {
                            result = ExecuteBasicOperation(operation, result, inputs[i]);
                        }
                    }
                    else
                    {
                        throw new ArgumentException(LocalizationManager.GetString("Wrong number of arguments"));
                    }                    
                }
                PrintResult(result);
            }
            else
            {
                throw new FileNotFoundException(LocalizationManager.GetString("Input File not found"));
            }
        }

        /// <summary>
        /// Print the result
        /// </summary>
        /// <param name="result"></param>
        private static void PrintResult(double result)
        {
            Console.WriteLine(string.Format(CultureInfo.InvariantCulture, LocalizationManager.GetString("Result: {0}"), result));
        }

        /// <summary>
        /// Prints on console the available documentation
        /// </summary>
        private static void PrintDocumentation()
        {
            Console.WriteLine(LocalizationManager.GetString("-h (--help) / -? Prints list of commands"));
            Console.WriteLine();
            Console.WriteLine(LocalizationManager.GetString("-i (--interactive) The programs works in console interactive mode, follow display istructions"));
            Console.WriteLine();
            Console.WriteLine(LocalizationManager.GetString("-f (--file) [File path of input file] [opcode, default: 0 (Addition)]"));
            Console.WriteLine("         ex: -f C:\\input.txt 3");
            Console.WriteLine(string.Format("         opcode:{0}  {1}", (int)OperationsEnum.Addition, OperationsEnum.Addition.ToString()));
            Console.WriteLine(string.Format("         opcode:{0}  {1}", (int)OperationsEnum.Substraction, OperationsEnum.Substraction.ToString()));
            Console.WriteLine(string.Format("         opcode:{0}  {1}", (int)OperationsEnum.Multiplication, OperationsEnum.Multiplication.ToString()));
            Console.WriteLine(string.Format("         opcode:{0}  {1}", (int)OperationsEnum.Division, OperationsEnum.Division.ToString()));
            Console.WriteLine(string.Format("         opcode:{0}  {1}", (int)OperationsEnum.Power, OperationsEnum.Power.ToString()));
            Console.WriteLine(string.Format("         opcode:{0}  {1}", (int)OperationsEnum.Root, OperationsEnum.Root.ToString()));
            Console.WriteLine(string.Format("         opcode:{0}  {1}", (int)OperationsEnum.Logarithm, OperationsEnum.Logarithm.ToString()));
            Console.WriteLine(string.Format("         opcode:{0}  {1}", (int)OperationsEnum.Expression, OperationsEnum.Expression.ToString()));
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine(LocalizationManager.GetString("Expression input format:"));
            Console.WriteLine(LocalizationManager.GetString("[operand 1] [operator] [operand 2]"));
            Console.WriteLine(LocalizationManager.GetString(LocalizationManager.GetString("ex: 2 + 4")));
            Console.WriteLine(LocalizationManager.GetString("Allowed operations in expression:"));
            Console.WriteLine(LocalizationManager.GetString("[+] Addition,       ex: 2 + 3 = 5"));
            Console.WriteLine(LocalizationManager.GetString("[-] Substraction,   ex: 5 - 3 = 2"));
            Console.WriteLine(LocalizationManager.GetString("[*] Multiplication, ex: 2 * 3 = 6"));
            Console.WriteLine(LocalizationManager.GetString("[/] Division,       ex: 6 / 3 = 2"));
            Console.WriteLine(LocalizationManager.GetString("[^] Power,          ex: 2 ^ 3 = 8"));
            Console.WriteLine(LocalizationManager.GetString("[r] Root,           ex: 8 r 3 = 2"));
            Console.WriteLine(LocalizationManager.GetString("[l] Logarithm,      ex: 8 l 2 = 3"));
            Console.WriteLine(LocalizationManager.GetString("Expression syntax:"));
            Console.WriteLine(LocalizationManager.GetString("[(] Open an expressione, [)] close an expression"));
            Console.WriteLine(LocalizationManager.GetString("([operand 1] [operator] [operand 2]) [operator] [operand 3]"));
            Console.WriteLine(LocalizationManager.GetString("Brackets are not mandatory be operator order is respected"));
            Console.WriteLine(LocalizationManager.GetString("Operator order:"));
            Console.WriteLine("r,l,^ > *,/ > +,-");
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine(LocalizationManager.GetString("[.] is the decimal separator, ex: 23.4577"));
        }
    }
}

