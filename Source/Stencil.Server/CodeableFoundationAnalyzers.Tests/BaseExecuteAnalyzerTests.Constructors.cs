using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using VerifyCS = CodeableFoundationAnalyzers.Tests.CSharpCodeFixVerifier<
    Codeable.Foundation.Analyzers.BaseExecuteAnalyzer,
    Microsoft.CodeAnalysis.Testing.EmptyCodeFixProvider>;

namespace CodeableFoundationAnalyzers.Tests
{
    public partial class BaseExecuteAnalyzerTests
    {
        [Fact]
        public async Task Constructors_Are_Exempted()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    int _x;
    public Program()
    {
        _x = 1;
    }
}

class ChokeableClass
{
public void ExecuteMethod(string methodName, Action action, params object[] parameters)
{
    action();
}
public TResult ExecuteFunction<TResult>(string methodName, Func<TResult> func, params object[] parameters)
{
    return func();
}
}
");
        }

        [Fact]
        public async Task Constructors_With_Args_Are_Exempted()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    int _x;
    public Program(int x)
    {
        _x = x;
    }
}

class ChokeableClass
{
public void ExecuteMethod(string methodName, Action action, params object[] parameters)
{
    action();
}
public TResult ExecuteFunction<TResult>(string methodName, Func<TResult> func, params object[] parameters)
{
    return func();
}
}
");
        }
    }
}
