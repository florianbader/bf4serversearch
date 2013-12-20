using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace rio.Battlefield4.ServerSearch.Wpf
{
    public class ParameterException : Exception
    {
        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="ParameterException"/> class from being created.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="stackTrace">The stack trace.</param>
        private ParameterException(string message, string stackTrace)
        {
            _message = message;
            _stackTrace = stackTrace;
        }

        #endregion Constructors

        #region Fields

        private string _message;
        private string _stackTrace;

        #endregion Fields

        #region Properties

        public override string Message
        {
            get { return _message; }
        }

        ///   </PermissionSet>
        public override string StackTrace
        {
            get { return _stackTrace; }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        public static void CheckCustom<TValue>(Expression<Func<TValue>> getParameterMethod, Func<TValue, bool> customTest)
        {
            ParameterException.CheckDefault(() => customTest);

            var value = GetValue(getParameterMethod);

            if (customTest(value))
                Throw<TValue>(getParameterMethod, "The value of parameter [PARAM] of method [METHOD] pass not the custom test '{0}'. Please specify a correct value.", customTest);
        }

        public static void CheckDefault<TValue>(Expression<Func<TValue>> getParameterMethod)
        {
            var value = GetValue(getParameterMethod);

            if (EqualityComparer<TValue>.Default.Equals(value, default(TValue)))
                Throw<TValue>(getParameterMethod, "The value of parameter [PARAM] of method [METHOD] was set to default value. Please specify a correct value.");
        }

        public static void CheckFileExists(Expression<Func<string>> getParameterMethod)
        {
            var value = GetValue(getParameterMethod);

            if (!File.Exists(value))
                Throw<string>(getParameterMethod, "The file [PARAM] does not exist.");
        }

        public static void CheckIsInRange(Expression<Func<int>> getParameterMethod, int min, int max)
        {
            var value = GetValue(getParameterMethod);

            if (min > value || value > max)
                Throw<int>(getParameterMethod, "The value of parameter [PARAM] of method [METHOD] was not in range between {0} and {1}. Please specify a correct value.", min, max);
        }

        public static void CheckIsInRange(Expression<Func<long>> getParameterMethod, long min, long max)
        {
            var value = GetValue(getParameterMethod);

            if (min > value || value > max)
                Throw<long>(getParameterMethod, "The value of parameter [PARAM] of method [METHOD] was not in range between {0} and {1}. Please specify a correct value.", min, max);
        }

        public static void CheckNullOrEmpty(Expression<Func<string>> getParameterMethod)
        {
            var value = GetValue(getParameterMethod);

            if (string.IsNullOrEmpty(value))
                Throw<string>(getParameterMethod, "The value of parameter [PARAM] of method [METHOD] was null or empty string. Please specify a correct value.");
        }

        public static void CheckNullOrWhiteSpace(Expression<Func<string>> getParameterMethod)
        {
            var value = GetValue(getParameterMethod);

            if (string.IsNullOrWhiteSpace(value))
                Throw<string>(getParameterMethod, "The value of parameter [PARAM] of method [METHOD] was null, empty or contains only whitespaces. Please specify a correct value.");
        }

        #endregion Public Methods

        #region Private Methods

        private static TValue GetValue<TValue>(Expression<Func<TValue>> getParameterMethod)
        {
            if (getParameterMethod == null)
                throw new ArgumentNullException("getParameterMethod");

            if (!(getParameterMethod.Body is MemberExpression))
                throw new ArgumentException("Only member expressions are allowed", "getParameterMethod");

            var value = getParameterMethod.Compile()();

            return value;
        }

        private static void Throw<TValue>(Expression<Func<TValue>> getParameterMethod, string messageFormat, params object[] args)
        {
            StringBuilder stackTraceBuilder = new StringBuilder();
            MethodBase currentMethod = null;
            string methodName = null;
            var framesToSkip = 2;

            do
            {
                var stackFrame = new StackFrame(framesToSkip);
                currentMethod = stackFrame.GetMethod();

                if (currentMethod == null)
                    break;

                if (methodName == null)
                    methodName = currentMethod.Name;

                stackTraceBuilder.Append(stackFrame.ToString());
                framesToSkip++;
            } while (currentMethod != null);

            var parameterName = ((MemberExpression)getParameterMethod.Body).Member.Name;
            var message = string.Format(messageFormat.Replace("[METHOD]", methodName).Replace("[PARAM]", parameterName), args);

            throw new ParameterException(message, stackTraceBuilder.ToString());
        }

        #endregion Private Methods

        #endregion Methods
    }
}