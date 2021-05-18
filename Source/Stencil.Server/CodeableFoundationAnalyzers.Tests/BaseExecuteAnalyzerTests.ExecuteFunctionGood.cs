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
        public async Task ExecuteFunction_Only_Statement()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    int Test()
    {
        return ExecuteFunction(nameof(Test), delegate() { return 0; });
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
        public async Task ThisExecuteFunction_Only_Statement()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    int Test()
    {
        return this.ExecuteFunction(nameof(Test), delegate() { return 0; });
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
        public async Task BaseExecuteFunction_Only_Statement()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    int Test()
    {
        return base.ExecuteFunction(nameof(Test), delegate() { return 0; });
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
        public async Task AsyncExecuteFunction_Only_Statement()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    Task<int> Test()
    {
        return ExecuteFunction(nameof(Test), async delegate() { await Task.Yield(); return 0; });
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
        public async Task ThisAsyncExecuteFunction_Only_Statement()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    Task<int> Test()
    {
        return this.ExecuteFunction(nameof(Test), async delegate() { await Task.Yield(); return 0; });
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
        public async Task BaseAsyncExecuteFunction_Only_Statement()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    Task<int> Test()
    {
        return base.ExecuteFunction(nameof(Test), async delegate() { await Task.Yield(); return 0; });
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
        public async Task AsyncAwaitExecuteFunction_Only_Statement()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    async Task<int> Test()
    {
        return await ExecuteFunction(nameof(Test), async delegate() { await Task.Yield(); return 0; });
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
        public async Task AsyncAwaitThisExecuteFunction_Only_Statement()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    async Task<int> Test()
    {
        return await this.ExecuteFunction(nameof(Test), async delegate() { await Task.Yield(); return 0; });
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
        public async Task AsyncAwaitBaseExecuteFunction_Only_Statement()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    async Task<int> Test()
    {
        return await base.ExecuteFunction(nameof(Test), async delegate() { await Task.Yield(); return 0; });
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
        public async Task ExecuteFunction_Only_Expression()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    int Test()
        => ExecuteFunction(nameof(Test), delegate() { return 0; });
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
        public async Task ThisExecuteFunction_Only_Expression()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    int Test()
        => this.ExecuteFunction(nameof(Test), delegate() { return 0; });
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
        public async Task BaseExecuteFunction_Only_Expression()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    int Test()
        => base.ExecuteFunction(nameof(Test), delegate() { return 0; });
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
        public async Task AsyncExecuteFunction_Only_Expression()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    Task<int> Test()
        => ExecuteFunction(nameof(Test), async delegate() { await Task.Yield(); return 0; });
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
        public async Task AsyncThisExecuteFunction_Only_Expression()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    Task<int> Test()
        => this.ExecuteFunction(nameof(Test), async delegate() { await Task.Yield(); return 0; });
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
        public async Task AsyncBaseExecuteFunction_Only_Expression()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    Task<int> Test()
        => base.ExecuteFunction(nameof(Test), async delegate() { await Task.Yield(); return 0; });
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
        public async Task AsyncAwaitExecuteFunction_Only_Expression()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    async Task<int> Test()
        => await ExecuteFunction(nameof(Test), async delegate() { await Task.Yield(); return 0; });
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
        public async Task AsyncAwaitThisExecuteFunction_Only_Expression()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    async Task<int> Test()
        => await this.ExecuteFunction(nameof(Test), async delegate() { await Task.Yield(); return 0; });
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
        public async Task AsyncAwaitBaseExecuteFunction_Only_Expression()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    async Task<int> Test()
        => await base.ExecuteFunction(nameof(Test), async delegate() { await Task.Yield(); return 0; });
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
        public async Task AsyncAwaitExecuteFunction_No_Result_Only_Statement()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    async Task Test()
    {
        await ExecuteFunction(nameof(Test), async delegate() { await Task.Yield(); });
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
        public async Task AsyncAwaitThisExecuteFunction_No_Result_Only_Statement()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    async Task Test()
    {
        await this.ExecuteFunction(nameof(Test), async delegate() { await Task.Yield(); });
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
        public async Task AsyncAwaitBaseExecuteFunction_No_Result_Only_Statement()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    async Task Test()
    {
        await base.ExecuteFunction(nameof(Test), async delegate() { await Task.Yield(); });
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
        public async Task AsyncAwaitBaseExecuteFunction_No_Result_Only_Expression()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    async Task Test()
        => await base.ExecuteFunction(nameof(Test), async delegate() { await Task.Yield(); });
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
        public async Task AsyncVoidAwaitExecuteFunction_No_Result_Only_Statement()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    async void Test()
    {
        await ExecuteFunction(nameof(Test), async delegate() { await Task.Yield(); });
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
        public async Task AsyncVoidAwaitThisExecuteFunction_No_Result_Only_Statement()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    async void Test()
    {
        await this.ExecuteFunction(nameof(Test), async delegate() { await Task.Yield(); });
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
        public async Task AsyncVoidAwaitBaseExecuteFunction_No_Result_Only_Statement()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    async void Test()
    {
        await base.ExecuteFunction(nameof(Test), async delegate() { await Task.Yield(); });
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
