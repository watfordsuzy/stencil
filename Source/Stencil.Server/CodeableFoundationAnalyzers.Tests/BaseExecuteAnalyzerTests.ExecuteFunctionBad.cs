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
        public async Task ExecuteFunction_Statement_Missing()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    int Test()
    {
        int i = 3;
        Console.WriteLine(i+2);
        return i;
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
                VerifyCS.Diagnostic(BaseExecuteAnalyzer.DiagnosticId).WithLocation(6, 5).WithArguments("Test", "ExecuteFunction"));
        }

        [Fact]
        public async Task ExecuteFunction_Statement_Missing_Nested()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : Program2
{
    int Test()
    {
        int i = 3;
        Console.WriteLine(i+2);
        return i;
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
                VerifyCS.Diagnostic(BaseExecuteAnalyzer.DiagnosticId).WithLocation(6, 5).WithArguments("Test", "ExecuteFunction"));
        }

        [Fact]
        public async Task ExecuteFunction_Expression_Missing()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    int Test()
        => 42;
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
                VerifyCS.Diagnostic(BaseExecuteAnalyzer.DiagnosticId).WithLocation(6, 5).WithArguments("Test", "ExecuteFunction"));
        }

        [Fact]
        public async Task ExecuteFunction_Expression_Missing_Nested()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : Program2
{
    int Test()
        => 42;
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
                VerifyCS.Diagnostic(BaseExecuteAnalyzer.DiagnosticId).WithLocation(6, 5).WithArguments("Test", "ExecuteFunction"));
        }

        [Fact]
        public async Task ExecuteFunction_With_Args_Statement_Missing()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    int Test(int i = 3)
    {
        Console.WriteLine(i+2);
        return i;
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
                VerifyCS.Diagnostic(BaseExecuteAnalyzer.DiagnosticId).WithLocation(6, 5).WithArguments("Test", "ExecuteFunction"));
        }

        [Fact]
        public async Task ExecuteFunction_With_Args_Statement_Missing_Nested()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : Program2
{
    int Test(int i = 3)
    {
        Console.WriteLine(i+2);
        return i;
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
                VerifyCS.Diagnostic(BaseExecuteAnalyzer.DiagnosticId).WithLocation(6, 5).WithArguments("Test", "ExecuteFunction"));
        }

        [Fact]
        public async Task ExecuteFunction_With_Args_Expression_Missing()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    int Test(int i = 3)
        => 42 + i;
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
                VerifyCS.Diagnostic(BaseExecuteAnalyzer.DiagnosticId).WithLocation(6, 5).WithArguments("Test", "ExecuteFunction"));
        }

        [Fact]
        public async Task ExecuteFunction_With_Args_Expression_Missing_Nested()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : Program2
{
    int Test(int i = 3)
        => 42 + i;
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
                VerifyCS.Diagnostic(BaseExecuteAnalyzer.DiagnosticId).WithLocation(6, 5).WithArguments("Test", "ExecuteFunction"));
        }

        [Fact]
        public async Task Async_ExecuteFunction_With_Args_Expression_Missing()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    async Task Test(int i = 3)
        => await Task.Delay(42 + i);
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
                VerifyCS.Diagnostic(BaseExecuteAnalyzer.DiagnosticId).WithLocation(6, 5).WithArguments("Test", "ExecuteFunction"));
        }

        [Fact]
        public async Task Async_ExecuteFunction_With_Args_Expression_Missing_Nested()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : Program2
{
    async Task Test(int i = 3)
        => await Task.Delay(42 + i);
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
                VerifyCS.Diagnostic(BaseExecuteAnalyzer.DiagnosticId).WithLocation(6, 5).WithArguments("Test", "ExecuteFunction"));
        }
    }
}
