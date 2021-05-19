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
        public async Task Empty_Method()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    void Test()
    {
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
        public async Task ExecuteMethod_Only_Statement()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    void Test()
    {
        ExecuteMethod(nameof(Test), delegate() { });
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
        public async Task ThisExecuteMethod_Only_Statement()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    void Test()
    {
        this.ExecuteMethod(nameof(Test), delegate() { });
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
        public async Task BaseExecuteMethod_Only_Statement()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    void Test()
    {
        base.ExecuteMethod(nameof(Test), delegate() { });
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
        public async Task ExecuteMethod_Only_Expression()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    void Test()
        => ExecuteMethod(nameof(Test), delegate() { });
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
        public async Task ThisExecuteMethod_Only_Expression()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    void Test()
        => this.ExecuteMethod(nameof(Test), delegate() { });
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
        public async Task BaseExecuteMethod_Only_Expression()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    void Test()
        => base.ExecuteMethod(nameof(Test), delegate() { });
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
        public async Task ExecuteMethod_Only_Statement_With_Local_Function()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    void Test()
    {
        ExecuteMethod(nameof(Test), delegate() { Inner(); });

        void Inner()
        {
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

        [Fact]
        public async Task ExecuteMethod_Only_Statement_With_Local_Function_First()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    void Test()
    {
        void Inner()
        {
        }

        ExecuteMethod(nameof(Test), Inner);
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
