using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Codeable.Foundation.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class BaseExecuteAnalyzer : DiagnosticAnalyzer
    {
        private readonly SymbolDisplayFormat _symbolDisplayFormat = new SymbolDisplayFormat(
            typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces, genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters);

        public const string DiagnosticId = "CallBaseExecute";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Usage";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.MethodDeclaration);
        }

        private void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            var methodDeclaration = (MethodDeclarationSyntax)context.Node;
            if (methodDeclaration.Identifier.ValueText.StartsWith("ExecuteMethod")
                || methodDeclaration.Identifier.ValueText.StartsWith("ExecuteFunction")
                || methodDeclaration.Identifier.ValueText == "Dispose")
            {
                return;
            }

            // Exempt Abstract Methods
            if (methodDeclaration.Modifiers.Any(SyntaxKind.AbstractKeyword))
            {
                return;
            }

            // Exempt Static Methods
            // TODO: determine correct pattern for static methods
            if (methodDeclaration.Modifiers.Any(SyntaxKind.StaticKeyword))
            {
                return;
            }

            // Partial methods without bodies
            if (methodDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword)
                && methodDeclaration.Body == null
                && methodDeclaration.ExpressionBody == null)
            {
                return;
            }

            if (!HasExecuteMethodOrFunction(context, methodDeclaration))
            {
                return;
            }

            var returnType = context.SemanticModel.GetTypeInfo(methodDeclaration.ReturnType);
            bool isAwait = IsAwait(methodDeclaration);
            bool isVoidOrAsyncTask = returnType.Type.SpecialType == SpecialType.System_Void
                                  || (isAwait && returnType.Type.ToDisplayString(_symbolDisplayFormat) == "System.Threading.Tasks.Task");
            if (!isVoidOrAsyncTask)
            {
                AnalyzeNodeForExecuteFunction(context, methodDeclaration);
            }
            else
            {
                AnalyzeNodeForExecuteMethod(context, methodDeclaration, isAwait ? "ExecuteFunction" : "ExecuteMethod");
            }
        }

        private void AnalyzeNodeForExecuteMethod(SyntaxNodeAnalysisContext context, MethodDeclarationSyntax methodDeclaration, string methodName)
        {
            if (methodDeclaration.Body != null && methodDeclaration.Body.Statements.Any())
            {
                var firstStatement = methodDeclaration.Body.Statements.First();
                if (firstStatement is ExpressionStatementSyntax statementSyntax)
                {
                    if (statementSyntax.Expression is InvocationExpressionSyntax invocation)
                    {
                        if (IsBaseExecuteInvocation(invocation, methodName))
                        {
                            return;
                        }
                    }
                    else if (statementSyntax.Expression is AwaitExpressionSyntax awaitExpression)
                    {
                        if (awaitExpression.Expression is InvocationExpressionSyntax awaitedInvocation)
                        {
                            if (IsBaseExecuteInvocation(awaitedInvocation, methodName))
                            {
                                return;
                            }
                        }
                    }
                }
            }
            else if (methodDeclaration.ExpressionBody != null)
            {
                if (methodDeclaration.ExpressionBody.Expression is InvocationExpressionSyntax invocation)
                {
                    if (IsBaseExecuteInvocation(invocation, methodName))
                    {
                        return;
                    }
                }
                else if (methodDeclaration.ExpressionBody.Expression is AwaitExpressionSyntax awaitExpression)
                {
                    if (awaitExpression.Expression is InvocationExpressionSyntax awaitedInvocation)
                    {
                        if (IsBaseExecuteInvocation(awaitedInvocation, methodName))
                        {
                            return;
                        }
                    }
                }
            }

            context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation(), methodDeclaration.Identifier.ValueText, methodName));
        }

        private void AnalyzeNodeForExecuteFunction(SyntaxNodeAnalysisContext context, MethodDeclarationSyntax methodDeclaration)
        {
            const string methodName = "ExecuteFunction";
            if (methodDeclaration.Body != null)
            {
                var firstStatement = methodDeclaration.Body.Statements.First();
                if (firstStatement is ReturnStatementSyntax returnStatement)
                {
                    if (returnStatement.Expression is InvocationExpressionSyntax invocation)
                    {
                        if (IsBaseExecuteInvocation(invocation, methodName))
                        {
                            return;
                        }
                    }
                    else if (returnStatement.Expression is AwaitExpressionSyntax awaitExpression
                        && awaitExpression.Expression is InvocationExpressionSyntax awaitedInvocation)
                    {
                        if (IsBaseExecuteInvocation(awaitedInvocation, methodName))
                        {
                            return;
                        }
                    }
                }
            }
            else if (methodDeclaration.ExpressionBody != null)
            {
                if (methodDeclaration.ExpressionBody.Expression is InvocationExpressionSyntax invocation)
                {
                    if (IsBaseExecuteInvocation(invocation, methodName))
                    {
                        return;
                    }
                }
                else if (methodDeclaration.ExpressionBody.Expression is AwaitExpressionSyntax awaitExpression)
                {
                    if (awaitExpression.Expression is InvocationExpressionSyntax awaitedInvocation)
                    {
                        if (IsBaseExecuteInvocation(awaitedInvocation, methodName))
                        {
                            return;
                        }
                    }
                }
            }

            context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation(), methodDeclaration.Identifier.ValueText, methodName));
        }

        private bool IsBaseExecuteInvocation(InvocationExpressionSyntax invocation, string methodName)
        {
            if (invocation.Expression is IdentifierNameSyntax identifier
                && identifier.Identifier.ValueText.StartsWith(methodName))
            {
                return true;
            }
            else if (invocation.Expression is MemberAccessExpressionSyntax memberAccessSyntax
                  && memberAccessSyntax.Name.Identifier.ValueText.StartsWith(methodName)
                  && (memberAccessSyntax.Expression is BaseExpressionSyntax
                      || memberAccessSyntax.Expression is ThisExpressionSyntax))
            {
                return true;
            }

            return false;
        }

        private bool HasExecuteMethodOrFunction(SyntaxNodeAnalysisContext context, MethodDeclarationSyntax methodDeclaration)
        {
            if (methodDeclaration.Parent is ClassDeclarationSyntax clazz)
            {
                if (HasExecuteMethodOrExecuteFunction(clazz))
                {
                    return true;
                }

                var symbolInfo = context.SemanticModel.GetDeclaredSymbol(methodDeclaration, context.CancellationToken);
                if (HasExecuteMethodOrExecuteFunction(symbolInfo.ContainingType))
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasExecuteMethodOrExecuteFunction(ClassDeclarationSyntax clazz)
        {
            foreach (var member in clazz.Members)
            {
                if (member is MethodDeclarationSyntax memberMethod
                    && (memberMethod.Identifier.ValueText == "ExecuteMethod"
                        || memberMethod.Identifier.ValueText == "ExecuteFunction"))
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasExecuteMethodOrExecuteFunction(ITypeSymbol typeSymbol)
        {
            if (typeSymbol == null)
            {
                return false;
            }

            foreach (var member in typeSymbol.GetMembers())
            {
                if (member.Kind == SymbolKind.Method
                    && (member.Name == "ExecuteMethod"
                        || member.Name == "ExecuteFunction"))
                {
                    return true;
                }
            }


            return HasExecuteMethodOrExecuteFunction(typeSymbol.BaseType);
        }

        private bool IsAwait(MethodDeclarationSyntax methodDeclaration) 
                => methodDeclaration.Modifiers.Any(SyntaxKind.AsyncKeyword);
    }
}
