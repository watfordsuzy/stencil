using Codeable.Foundation.Analyzers;
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
        public async Task ExecuteMethod_Statement_Missing()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    void Test()
    {
        int i = 3;
        Console.WriteLine(i+2);
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
",
                VerifyCS.Diagnostic(BaseExecuteAnalyzer.DiagnosticId).WithLocation(6, 5).WithArguments("Test", "ExecuteMethod"));
        }

        [Fact]
        public async Task ExecuteMethod_Statement_Missing_Nested()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : Program2
{
    void Test()
    {
        int i = 3;
        Console.WriteLine(i+2);
    }
}

class Program2 : Program3
{
}

class Program3 : ChokeableClass
{
}

class ChokeableClass
{
protected void ExecuteMethod(string methodName, Action action, params object[] parameters)
{
    action();
}
protected TResult ExecuteFunction<TResult>(string methodName, Func<TResult> func, params object[] parameters)
{
    return func();
}
}
",
                VerifyCS.Diagnostic(BaseExecuteAnalyzer.DiagnosticId).WithLocation(6, 5).WithArguments("Test", "ExecuteMethod"));
        }

        [Fact]
        public async Task ExecuteMethod_Expression_Missing()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    void Test()
        => Console.WriteLine(2);
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
",
                VerifyCS.Diagnostic(BaseExecuteAnalyzer.DiagnosticId).WithLocation(6, 5).WithArguments("Test", "ExecuteMethod"));
        }

        [Fact]
        public async Task ExecuteMethod_Expression_Missing_Nested()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : Program2
{
    void Test()
        => Console.WriteLine(2);
}

class Program2 : Program3
{
}

class Program3 : ChokeableClass
{
}

class ChokeableClass
{
protected void ExecuteMethod(string methodName, Action action, params object[] parameters)
{
    action();
}
protected TResult ExecuteFunction<TResult>(string methodName, Func<TResult> func, params object[] parameters)
{
    return func();
}
}
",
                VerifyCS.Diagnostic(BaseExecuteAnalyzer.DiagnosticId).WithLocation(6, 5).WithArguments("Test", "ExecuteMethod"));
        }

        [Fact]
        public async Task ExecuteMethod_With_Args_Statement_Missing()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    void Test(int i = 3)
    {
        Console.WriteLine(i+2);
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
",
                VerifyCS.Diagnostic(BaseExecuteAnalyzer.DiagnosticId).WithLocation(6, 5).WithArguments("Test", "ExecuteMethod"));
        }

        [Fact]
        public async Task ExecuteMethod_With_Args_Statement_Missing_Nested()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : Program2
{
    void Test(int i = 3)
    {
        Console.WriteLine(i+2);
    }
}

class Program2 : Program3
{
}

class Program3 : ChokeableClass
{
}

class ChokeableClass
{
protected void ExecuteMethod(string methodName, Action action, params object[] parameters)
{
    action();
}
protected TResult ExecuteFunction<TResult>(string methodName, Func<TResult> func, params object[] parameters)
{
    return func();
}
}
",
                VerifyCS.Diagnostic(BaseExecuteAnalyzer.DiagnosticId).WithLocation(6, 5).WithArguments("Test", "ExecuteMethod"));
        }

        [Fact]
        public async Task ExecuteMethod_With_Args_Expression_Missing()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    void Test(int i)
        => Console.WriteLine(i + 2);
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
",
                VerifyCS.Diagnostic(BaseExecuteAnalyzer.DiagnosticId).WithLocation(6, 5).WithArguments("Test", "ExecuteMethod"));
        }

        [Fact]
        public async Task ExecuteMethod_With_Args_Expression_Missing_Nested()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : Program2
{
    void Test(int i)
        => Console.WriteLine(i + 2);
}

class Program2 : Program3
{
}

class Program3 : ChokeableClass
{
}

class ChokeableClass
{
protected void ExecuteMethod(string methodName, Action action, params object[] parameters)
{
    action();
}
protected TResult ExecuteFunction<TResult>(string methodName, Func<TResult> func, params object[] parameters)
{
    return func();
}
}
",
                VerifyCS.Diagnostic(BaseExecuteAnalyzer.DiagnosticId).WithLocation(6, 5).WithArguments("Test", "ExecuteMethod"));
        }

        [Fact]
        public async Task ExecuteMethod_Partial_Missing()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
partial class Program
{
    partial void Test(int i)
        => Console.WriteLine(i + 2);
}

partial class Program : ChokeableClass
{
    partial void Test(int i);
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
",
                VerifyCS.Diagnostic(BaseExecuteAnalyzer.DiagnosticId).WithLocation(6, 5).WithArguments("Test", "ExecuteMethod"));
        }
    }
}
