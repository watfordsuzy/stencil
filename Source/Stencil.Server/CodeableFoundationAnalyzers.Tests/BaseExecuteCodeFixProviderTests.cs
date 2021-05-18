using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using VerifyCS = CodeableFoundationAnalyzers.Tests.CSharpCodeFixVerifier<
    Codeable.Foundation.Analyzers.BaseExecuteAnalyzer,
    Codeable.Foundation.Analyzers.BaseExecuteCodeFixProvider>;

namespace CodeableFoundationAnalyzers.Tests
{
    public partial class BaseExecuteCodeFixProviderTests
    {
        [Fact]
        public async Task Fixes_Statement_Missing_BaseExecuteMethod()
        {
            await VerifyCS.VerifyCodeFixAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    [|void Test()
    {
        int i = 3;
        Console.WriteLine(i + 2);
    }|]
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
", @"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    void Test()
    {
        base.ExecuteMethod(nameof(Test), delegate ()
        {
            int i = 3;
            Console.WriteLine(i + 2);
        });
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
        public async Task Fixes_Expression_Missing_BaseExecuteMethod()
        {
            await VerifyCS.VerifyCodeFixAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    [|void Test(int i = 3)
        => Console.WriteLine(i + 2);|]
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
", @"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    void Test(int i = 3) => base.ExecuteMethod(nameof(Test), delegate () { Console.WriteLine(i + 2); });
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
        public async Task Fixes_Statement_Missing_BaseExecuteFunction()
        {
            await VerifyCS.VerifyCodeFixAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    [|int Test(int i = 3)
    {
        Console.WriteLine(i + 2);
        return i + 2;
    }|]
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
", @"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    int Test(int i = 3)
    {
        return base.ExecuteFunction(nameof(Test), delegate ()
        {
            Console.WriteLine(i + 2);
            return i + 2;
        });
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
        public async Task Fixes_Expression_Missing_BaseExecuteFunction()
        {
            await VerifyCS.VerifyCodeFixAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    [|int Test(int i = 3)
        => i + 2;|]
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
", @"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    int Test(int i = 3) => base.ExecuteFunction(nameof(Test), delegate () { return i + 2; });
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
        public async Task Fixes_Task_Returning_Statement_Missing_BaseExecuteFunction()
        {
            await VerifyCS.VerifyCodeFixAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    [|Task Test(int i = 3)
    {
        Console.WriteLine(i + 2);
        return Task.CompletedTask;
    }|]
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
", @"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    Task Test(int i = 3)
    {
        return base.ExecuteFunction(nameof(Test), delegate ()
        {
            Console.WriteLine(i + 2);
            return Task.CompletedTask;
        });
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
        public async Task Fixes_Task_Returning_Expression_Missing_BaseExecuteFunction()
        {
            await VerifyCS.VerifyCodeFixAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    [|Task<int> Test(int i = 3)
        => Task.FromResult(i + 2);|]
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
", @"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    Task<int> Test(int i = 3) => base.ExecuteFunction(nameof(Test), delegate () { return Task.FromResult(i + 2); });
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
        public async Task Fixes_Async_Statement_Missing_BaseExecuteFunction()
        {
            await VerifyCS.VerifyCodeFixAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    [|async Task Test(int i = 3)
    {
        Console.WriteLine(i + 2);
        await Task.Yield();
    }|]
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
", @"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    async Task Test(int i = 3)
    {
        await base.ExecuteFunction(nameof(Test), async delegate ()
        {
            Console.WriteLine(i + 2);
            await Task.Yield();
        });
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
        public async Task Fixes_Async_Expression_Missing_BaseExecuteFunction()
        {
            await VerifyCS.VerifyCodeFixAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    [|async Task Test(int i = 3)
        => await Task.Delay(i);|]
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
", @"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    async Task Test(int i = 3) => await base.ExecuteFunction(nameof(Test), async delegate () { await Task.Delay(i); });
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
        public async Task Fixes_Async_Statement_With_Result_Missing_BaseExecuteFunction()
        {
            await VerifyCS.VerifyCodeFixAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    [|async Task<int> Test(int i = 3)
    {
        Console.WriteLine(i + 2);
        await Task.Yield();
        return i + 2;
    }|]
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
", @"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    async Task<int> Test(int i = 3)
    {
        return await base.ExecuteFunction(nameof(Test), async delegate ()
        {
            Console.WriteLine(i + 2);
            await Task.Yield();
            return i + 2;
        });
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
        public async Task Fixes_Async_Expression_With_Result_Missing_BaseExecuteFunction()
        {
            await VerifyCS.VerifyCodeFixAsync(@"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    [|async Task<int> Test(int i = 3)
        => await Task.Run(() => i + 2);|]
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
", @"
using System;
using System.Threading.Tasks;
class Program : ChokeableClass
{
    async Task<int> Test(int i = 3) => await base.ExecuteFunction(nameof(Test), async delegate () { return await Task.Run(() => i + 2); });
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
