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
        public async Task Disposers_Are_Exempted()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass, IDisposable
{
    public void Dispose()
    {
        Console.WriteLine(1);
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
        public async Task Dispose_Pattern_Is_Exempted()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass, IDisposable
{
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Console.WriteLine(1);
        }
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
