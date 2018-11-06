/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP2_0)
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WDToolbox;
using WDToolbox.Maths.Integer;
using WDToolbox.Maths.Integer;

namespace WDToolbox.Maths.Equation
{
    /// <summary>
    /// Parses text to create a lambda expression.
    /// Great for maths formulas.
    /// </summary>
    public class ExpressionParser
    {
        public Dictionary<string, string> TokenResolverTable { get; private set; }
        public Func<string, int> IndexOfParamater { get; private set; }
        protected static Dictionary<string, string> translateType { get; set; }
        protected static Dictionary<string, string> emulatedTypeCast { get; set; }
        protected string ExpressionHelperClass { get; set; }
        public string ErrorReport { get; protected set; }
        public string FormulaAsCode { get; protected set; }
        public string FormulaAsClass { get; protected set; }
        public LambdaExpression Expresion { get; protected set; }
        public bool OverflowChecks{ get; protected set; }

        
        static ExpressionParser()
        {
            //The code we compile from a string will need EmulatedInteger
            //but none of the code in this class uses it. So add this just to prevent
            //the using clause being cleaned up.
            EmulatedInteger hereToForceLinkerErrorIfNotIncluded = new EmulatedInteger();


           //to force compilation against unboxed types
            translateType = new Dictionary<string, string>();
            translateType.Add("int64", "long");
            translateType.Add("uint64", "ulong");
            translateType.Add("int32", "int");
            translateType.Add("uint32", "uint");
            translateType.Add("int16", "short");
            translateType.Add("uint16", "ushort");
            translateType.Add("single", "float");

            emulatedTypeCast = new Dictionary<string, string>();
            emulatedTypeCast.Add("short", "EmulatedInteger.SInt16({0})");
            emulatedTypeCast.Add("ushort", "EmulatedInteger.UInt16({0})");
            emulatedTypeCast.Add("byte", "EmulatedInteger.UInt8({0})");
            emulatedTypeCast.Add("sbyte", "EmulatedInteger.SInt8({0})");
        }

        public ExpressionParser(Func<string, int> indexOfParam)
        {
            IndexOfParamater = indexOfParam;

            TokenResolverTable = new Dictionary<string, string>();

            string[] MathFunctions = new string[] { "Abs", "Acos", "Asin", "Atan", "Atan2", "Ceiling", "Cos", "Cosh", "Exp", "Floor", "Log", "Log10", "Max", "Min", "Pow", "Round", "Sin", "Sinh", "Sqrt", "Tan", "Tanh", "Truncate" };

            //patch in the standard maths functions
            foreach (string s in MathFunctions)
            {
                TokenResolverTable.Add(NormaliseString(s), "Math." + s);
            }
            
            //a special case
            //StandardResolverTable.Add(NormaliseString("sign"), "({0})Math.Sign");
            TokenResolverTable.Add(NormaliseString("sign"), "Math.Sign");


            TokenResolverTable.Add(NormaliseString("pi"), "Math.PI");
            TokenResolverTable.Add(NormaliseString("e"), "Math.E");
            TokenResolverTable.Add(NormaliseString("null"), "0");

            TokenResolverTable.Add(NormaliseString("true"), "true");
            TokenResolverTable.Add(NormaliseString("false"), "false");

            ExpressionHelperClass =
              @"using System;
                using System.Linq.Expressions;
                using WDToolbox.Maths.Integer;

                class ExpressionHelper
                {
                    public static Expression<Func<TYPE[], TYPE>> exp()
                    {
                        return (VAR_LIST) => CODE;
                    }
                }";

            OverflowChecks = true;
        }

        public Expression<Func<T[], T>> Parse<T>(string exp)
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            FormulaAsCode = "?";
            try
            {
                //setup types
                string dataType = typeof(T).Name.ToLower(); //double, not Double
                dataType = translateType.ContainsKey(dataType) ? translateType[dataType] : dataType;
                // returnType = (string)dataType.Clone();

                //parse
                FormulaAsCode = ResolveUserInput2Code(exp, dataType);
                FormulaAsCode = OverflowChecks ? string.Format("checked({0})", FormulaAsCode) : FormulaAsCode;

                FormulaAsClass = ExpressionHelperClass.Replace("TYPE", dataType).Replace("CODE", FormulaAsCode);
                CompilerResults results = Compile(FormulaAsClass);

                //check it compiled
                if (results.Errors.Count == 0)
                {
                    Expression<Func<T[], T>> expression = (Expression<Func<T[], T>>)results
                    .CompiledAssembly
                    .GetType("ExpressionHelper")
                    .GetMethod("exp").Invoke(null, null);

                    this.Expresion = expression;
                    ErrorReport = null;
                    return expression;
                }
                else
                {
                    ErrorReport = CreateErrorReport(results);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ErrorReport = ex.Message;
                return null;
            }
            
        }

        private static string NormaliseString(string s) { return s.Trim().ToLower(); }

        private string ResolveUserInput2Code(string input, string dataType)
        {
            //the task: replace certian phrases with others
            //Glorious hack!

            //The idea, if I replace all nontoken chars with " ".
            //given " " is not allowed in the tokens we resolve
            //" token " shold be a uniqe thing I can find 
            // (ie "tan" and "tanh" won't clash)
            //requires i pad a space before and after the whole thing.

            //Since the resolved token is not allowed a " "
            //there will be equal spaces in the new string afer 
            //all replacemnts are made.
            //So we go back and replace every nth space with every nth nontoken.

            //vola tricky problem with lots of edge cases in a few lines of code.
            //only works because of the way we write math eg: 10*pi not 10pi

            Predicate<char> isCharAllowedInToken = C => char.IsLetterOrDigit(C) || "_".Contains(C);
            Predicate<char> isCharAllowedInOtheMathsStuff = C => @" 01234567890./\+-*^&|!%()?:".Contains(C);
            
            StringBuilder sb = new StringBuilder();
            foreach (char c in input)
            {
                sb.Append(isCharAllowedInToken(c) ? c : ' ');
            }

            string searchableString = " " + sb.ToString() + " "; //padded

            //every unique " token " present
            string[] tokens = searchableString.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                                .ToList().Distinct().Select(S => " " + S + " ")
                                .ToArray();
            
            //now I can safely do simple replaces
            foreach (string token in tokens)
            {
                string tokenNoPadding = token.Substring(1, token.Length - 2);
                string resolvedToken = Resolve(tokenNoPadding, dataType);

                if (resolvedToken != null)
                {
                    searchableString = searchableString.Replace(token, " " + resolvedToken + " ");

                    //sanity
                    if (resolvedToken.Contains(" "))
                    {
                        throw new FormatException("Resolved Token can't have a space: \"" + resolvedToken + "\"");
                    }
                }
                else
                {
                    if (!tokenNoPadding.All(C => isCharAllowedInOtheMathsStuff(C)))
                    {
                        throw new FormatException("Unknown token: " + tokenNoPadding);
                    }
                }                
            }

            //remove padding
            searchableString = searchableString.Substring(1, searchableString.Length - 2);

            //sanity
            if (searchableString.Count(C => C == ' ') != input.Count(C => !isCharAllowedInToken(C)))
            {
                throw new FormatException("Problem with my briught idea.");
            }

            //fill spaces with original data
            sb.Clear();
            int startIndex=0; //all that stops spaces in the input becoming targets for replacement
            foreach (char c in input)
            {
                if (!isCharAllowedInToken(c))
                {
                    startIndex = searchableString.IndexOf(' ', startIndex);
                    searchableString = searchableString.Remove(startIndex, 1);
                    searchableString = searchableString.Insert(startIndex, ""+c);
                    startIndex++; //skip forward incase a space was replaced with a space
                }
            }

            //was there some emulation going on? If so cast back to our datatype
            if (emulationInUseFor(dataType))
            {
                searchableString = string.Format("({1})({0})", searchableString, dataType);
                //dataType = emulatedTypeCast.ContainsKey(dataType) ? emulatedTypeCast[dataType] : dataType;
            }

            return searchableString;
        }

        private string ResolveName(string token)
        {
            return TokenResolverTable.ContainsKey(token) ? TokenResolverTable[token] : null;
        }

        protected string Resolve(string name, string dataType)
        {
            Predicate<string> notAnOperator = S => S.Any(C => (char.IsNumber(C) || char.IsLetter(C)));

            int index = IndexOfParamater(name);
            if (index >= 0)
            {
                return cast("VAR_LIST[" + index + "]", dataType);
                //return "VAR_LIST[" + index + "]";
            }
            else
            {
                string token = NormaliseString(name);
                string r = ResolveName(token);

                if (r != null)
                {
                    //return string.Format(r, dataType);
                    if (notAnOperator(r))
                    {
                        //sin, tan, pi etc
                        return cast(r, dataType);
                        //return r;
                    }
                    else
                    {
                        return r;
                    }
                }
                else if(notAnOperator(name))
                {
                    //number  7 2.45
                    return cast(name, dataType);
                    //return name;
                }
            }

            return null;
        }


        private bool emulationInUseFor(string dataType) { return OverflowChecks && emulatedTypeCast.ContainsKey(dataType); }

        private string cast(string what, string dataType, bool encloseWhatInBrackets=false)
        {
            //dont actuly use shorts or bytes... they don't oferflow because some architect thought that was a good idea.
            if (emulationInUseFor(dataType))
            {
                string s =string.Format(emulatedTypeCast[dataType], what);
                return encloseWhatInBrackets ? "(" + s + ")" : s;
            }

            return encloseWhatInBrackets ? 
                string.Format("({1})({0})", what, dataType) :
                string.Format("({1}){0}",   what, dataType);
        }

        private static CompilerResults Compile(string ToCompile)
        {
            Dictionary<String, String> providerOptions = new Dictionary<String, String> { { "CompilerVersion", "v3.5" } };
            
            Microsoft.CSharp.CSharpCodeProvider codeProvider = new Microsoft.CSharp.CSharpCodeProvider(providerOptions);
            System.CodeDom.Compiler.ICodeCompiler icc = codeProvider.CreateCompiler();
            string Output = "";// @"mypath";
            System.CodeDom.Compiler.CompilerParameters parameters = new System.CodeDom.Compiler.CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.OutputAssembly = Output;
            parameters.ReferencedAssemblies.Add("System.Core.dll");
            parameters.ReferencedAssemblies.Add("System.Data.dll");
            parameters.ReferencedAssemblies.Add("System.XML.Linq.dll");
            parameters.ReferencedAssemblies.Add("WD toolbox.dll");
            CompilerResults results = icc.CompileAssemblyFromSource(parameters, ToCompile);

            if (results.Errors.Count == 0)
            {
                //System.Diagnostics.Process.Start(Output);
            }

            return results;
        }

        private static string CreateErrorReport(CompilerResults results)
        {

            string errors = "";

            if (results.Errors.Count > 0)
            {
                foreach (CompilerError CompErr in results.Errors)
                {
                    errors += "Line number " + CompErr.Line +
                                ", Error Number: " + CompErr.ErrorNumber +
                                ", '" + CompErr.ErrorText + ";" +
                                Environment.NewLine + Environment.NewLine;
                }
            }

            return errors;
        }

        public string GenerateDebugReport()
        {
            StringBuilder sb = new StringBuilder();
            if (ErrorReport.IsSomething())
            {
                sb.AppendLine("Errors:");
                sb.AppendLine(ErrorReport.IndentLines());
            }
            if (FormulaAsCode.IsSomething())
            {
                sb.AppendLine("Code: " + FormulaAsCode);
            }
            if (FormulaAsCode.IsSomething())
            {
                sb.AppendLine("Class: ");
                sb.AppendLine(FormulaAsClass.IndentLines());
            }
            //sb.AppendLine(
            return sb.ToString();
        }

        public static Expression<Func<ushort[], ushort>> exp()
        {
            return (VAR_LIST) => (ushort)(((ushort)VAR_LIST[0]) * ((ushort)2) * ((ushort)31415) / ((ushort)10000));
        }
    }
}

#endif