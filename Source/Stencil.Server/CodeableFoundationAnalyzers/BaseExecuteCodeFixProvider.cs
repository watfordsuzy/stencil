using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Simplification;

namespace Codeable.Foundation.Analyzers
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(BaseExecuteCodeFixProvider)), Shared]
    public class BaseExecuteCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(BaseExecuteAnalyzer.DiagnosticId); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            foreach (var diagnostic in context.Diagnostics)
            {
                var diagnosticSpan = diagnostic.Location.SourceSpan;

                // Find the type declaration identified by the diagnostic.
                var declaration = root.FindToken(diagnosticSpan.Start)
                                      .Parent
                                      .AncestorsAndSelf()
                                      .OfType<MethodDeclarationSyntax>()
                                      .First();

                // Register a code action that will invoke the fix.
                context.RegisterCodeFix(
                    CodeAction.Create(
                        title: CodeFixResources.CodeFixTitle,
                        createChangedDocument: token => AddBaseExecuteAsync(context.Document, declaration, token),
                        equivalenceKey: nameof(CodeFixResources.CodeFixTitle)),
                    diagnostic);
            }
        }

        enum CodeFixType
        {
            AddBaseExecuteMethod,
            AddBaseExecuteFunction,
            AddAsyncBaseExecuteFunction,
            AddAsyncBaseExecuteFunctionWithResult,
        }

        enum DelegateType
        {
            None,
            Async,
        }

        private async Task<Document> AddBaseExecuteAsync(Document document, MethodDeclarationSyntax declaration, CancellationToken token)
        {
            CodeFixType codeFixType = DetermineCodeFixType(declaration);

            switch(codeFixType)
            {
                case CodeFixType.AddBaseExecuteMethod:
                    return await AddBaseExecuteMethodAsync(document, declaration, token);
                case CodeFixType.AddBaseExecuteFunction:
                    return await AddBaseExecuteFunctionAsync(document, declaration, token);
                case CodeFixType.AddAsyncBaseExecuteFunction:
                    return await AddAsyncBaseExecuteFunctionAsync(document, declaration, token);
                case CodeFixType.AddAsyncBaseExecuteFunctionWithResult:
                    return await AddAsyncBaseExecuteFunctionWithResultAsync(document, declaration, token);
            }

            return document;
        }

        private async Task<Document> AddBaseExecuteMethodAsync(Document document, MethodDeclarationSyntax declaration, CancellationToken token)
        {
            // Replace the old local declaration with the new local declaration.
            SyntaxNode oldRoot = await document.GetSyntaxRootAsync(token).ConfigureAwait(false);
            SyntaxNode newRoot = oldRoot;

            if (declaration.Body != null)
            {
                var newDeclaration = declaration.WithBody(
                    SyntaxFactory.Block(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.BaseExpression(),
                                    SyntaxFactory.IdentifierName("ExecuteMethod")),
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SeparatedList<ArgumentSyntax>()
                                        .Add(SyntaxFactory.Argument(NameOfExpression(declaration.Identifier)))
                                        .Add(SyntaxFactory.Argument(
                                                SyntaxFactory.AnonymousMethodExpression()
                                                    .WithDelegateKeyword(SyntaxFactory.Token(SyntaxKind.DelegateKeyword))
                                                    .WithParameterList(SyntaxFactory.ParameterList())
                                                    .WithBlock(SyntaxFactory.Block(declaration.Body.Statements))
                                            ))
                                )
                           )
                       )
                    )
                );

                newRoot = oldRoot.ReplaceNode(declaration, newDeclaration).WithAdditionalAnnotations(Formatter.Annotation);
            }
            else if (declaration.ExpressionBody != null)
            {
                var newDeclaration = declaration.WithExpressionBody(
                    SyntaxFactory.ArrowExpressionClause(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.BaseExpression(),
                                SyntaxFactory.IdentifierName("ExecuteMethod")),
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>()
                                    .Add(SyntaxFactory.Argument(NameOfExpression(declaration.Identifier)))
                                    .Add(SyntaxFactory.Argument(
                                            SyntaxFactory.AnonymousMethodExpression()
                                                .WithDelegateKeyword(SyntaxFactory.Token(SyntaxKind.DelegateKeyword))
                                                .WithParameterList(SyntaxFactory.ParameterList())
                                                .WithBlock(
                                                    SyntaxFactory.Block(
                                                        SyntaxFactory.ExpressionStatement(declaration.ExpressionBody.Expression)))
                                        ))
                            )
                        )
                    )
                );

                newRoot = oldRoot.ReplaceNode(declaration, newDeclaration).WithAdditionalAnnotations(Formatter.Annotation);
            }

            // Return document with transformed tree.
            return document.WithSyntaxRoot(newRoot);
        }

        private async Task<Document> AddBaseExecuteFunctionAsync(Document document, MethodDeclarationSyntax declaration, CancellationToken token)
        {
            // Replace the old local declaration with the new local declaration.
            SyntaxNode oldRoot = await document.GetSyntaxRootAsync(token).ConfigureAwait(false);
            SyntaxNode newRoot = oldRoot;

            if (declaration.Body != null)
            {
                var newDeclaration = declaration.WithBody(
                    SyntaxFactory.Block(
                        SyntaxFactory.ReturnStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.BaseExpression(),
                                    SyntaxFactory.IdentifierName("ExecuteFunction")),
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SeparatedList<ArgumentSyntax>()
                                        .Add(SyntaxFactory.Argument(NameOfExpression(declaration.Identifier)))
                                        .Add(SyntaxFactory.Argument(
                                                SyntaxFactory.AnonymousMethodExpression()
                                                    .WithDelegateKeyword(SyntaxFactory.Token(SyntaxKind.DelegateKeyword))
                                                    .WithParameterList(SyntaxFactory.ParameterList())
                                                    .WithBlock(SyntaxFactory.Block(declaration.Body.Statements))
                                            ))
                                )
                           )
                       )
                    )
                );

                newRoot = oldRoot.ReplaceNode(declaration, newDeclaration).WithAdditionalAnnotations(Formatter.Annotation);
            }
            else if (declaration.ExpressionBody != null)
            {
                var newDeclaration = declaration.WithExpressionBody(
                    SyntaxFactory.ArrowExpressionClause(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.BaseExpression(),
                                SyntaxFactory.IdentifierName("ExecuteFunction")),
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>()
                                    .Add(SyntaxFactory.Argument(NameOfExpression(declaration.Identifier)))
                                    .Add(SyntaxFactory.Argument(
                                            SyntaxFactory.AnonymousMethodExpression()
                                                .WithDelegateKeyword(SyntaxFactory.Token(SyntaxKind.DelegateKeyword))
                                                .WithParameterList(SyntaxFactory.ParameterList())
                                                .WithBlock(
                                                    SyntaxFactory.Block(
                                                        SyntaxFactory.ReturnStatement(declaration.ExpressionBody.Expression)))
                                        ))
                            )
                        )
                    )
                );

                newRoot = oldRoot.ReplaceNode(declaration, newDeclaration).WithAdditionalAnnotations(Formatter.Annotation);
            }

            // Return document with transformed tree.
            return document.WithSyntaxRoot(newRoot);
        }

        private async Task<Document> AddAsyncBaseExecuteFunctionAsync(Document document, MethodDeclarationSyntax declaration, CancellationToken token)
        {
            // Replace the old local declaration with the new local declaration.
            SyntaxNode oldRoot = await document.GetSyntaxRootAsync(token).ConfigureAwait(false);
            SyntaxNode newRoot = oldRoot;

            if (declaration.Body != null)
            {
                var newDeclaration = declaration.WithBody(
                    SyntaxFactory.Block(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.AwaitExpression(
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.BaseExpression(),
                                        SyntaxFactory.IdentifierName("ExecuteFunction")),
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SeparatedList<ArgumentSyntax>()
                                            .Add(SyntaxFactory.Argument(NameOfExpression(declaration.Identifier)))
                                            .Add(SyntaxFactory.Argument(
                                                    SyntaxFactory.AnonymousMethodExpression()
                                                        .WithAsyncKeyword(SyntaxFactory.Token(SyntaxKind.AsyncKeyword))
                                                        .WithDelegateKeyword(SyntaxFactory.Token(SyntaxKind.DelegateKeyword))
                                                        .WithParameterList(SyntaxFactory.ParameterList())
                                                        .WithBlock(SyntaxFactory.Block(declaration.Body.Statements))
                                                ))
                                    )
                               )
                           )
                       )
                    )
                );

                newRoot = oldRoot.ReplaceNode(declaration, newDeclaration).WithAdditionalAnnotations(Formatter.Annotation);
            }
            else if (declaration.ExpressionBody != null)
            {
                var newDeclaration = declaration.WithExpressionBody(
                    SyntaxFactory.ArrowExpressionClause(
                        SyntaxFactory.AwaitExpression(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.BaseExpression(),
                                    SyntaxFactory.IdentifierName("ExecuteFunction")),
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SeparatedList<ArgumentSyntax>()
                                        .Add(SyntaxFactory.Argument(NameOfExpression(declaration.Identifier)))
                                        .Add(SyntaxFactory.Argument(
                                                SyntaxFactory.AnonymousMethodExpression()
                                                    .WithAsyncKeyword(SyntaxFactory.Token(SyntaxKind.AsyncKeyword))
                                                    .WithDelegateKeyword(SyntaxFactory.Token(SyntaxKind.DelegateKeyword))
                                                    .WithParameterList(SyntaxFactory.ParameterList())
                                                    .WithBlock(
                                                        SyntaxFactory.Block(
                                                            SyntaxFactory.ExpressionStatement(declaration.ExpressionBody.Expression)))
                                            ))
                                )
                            )
                        )
                    )
                );

                newRoot = oldRoot.ReplaceNode(declaration, newDeclaration).WithAdditionalAnnotations(Formatter.Annotation);
            }

            // Return document with transformed tree.
            return document.WithSyntaxRoot(newRoot);
        }

        private async Task<Document> AddAsyncBaseExecuteFunctionWithResultAsync(Document document, MethodDeclarationSyntax declaration, CancellationToken token)
        {
            // Replace the old local declaration with the new local declaration.
            SyntaxNode oldRoot = await document.GetSyntaxRootAsync(token).ConfigureAwait(false);
            SyntaxNode newRoot = oldRoot;

            if (declaration.Body != null)
            {
                var newDeclaration = declaration.WithBody(
                    SyntaxFactory.Block(
                        SyntaxFactory.ReturnStatement(
                            SyntaxFactory.AwaitExpression(
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.BaseExpression(),
                                        SyntaxFactory.IdentifierName("ExecuteFunction")),
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SeparatedList<ArgumentSyntax>()
                                            .Add(SyntaxFactory.Argument(NameOfExpression(declaration.Identifier)))
                                            .Add(SyntaxFactory.Argument(
                                                    SyntaxFactory.AnonymousMethodExpression()
                                                        .WithAsyncKeyword(SyntaxFactory.Token(SyntaxKind.AsyncKeyword))
                                                        .WithDelegateKeyword(SyntaxFactory.Token(SyntaxKind.DelegateKeyword))
                                                        .WithParameterList(SyntaxFactory.ParameterList())
                                                        .WithBlock(SyntaxFactory.Block(declaration.Body.Statements))
                                                ))
                                    )
                               )
                           )
                       )
                    )
                );

                newRoot = oldRoot.ReplaceNode(declaration, newDeclaration).WithAdditionalAnnotations(Formatter.Annotation);
            }
            else if (declaration.ExpressionBody != null)
            {
                var newDeclaration = declaration.WithExpressionBody(
                    SyntaxFactory.ArrowExpressionClause(
                        SyntaxFactory.AwaitExpression(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.BaseExpression(),
                                    SyntaxFactory.IdentifierName("ExecuteFunction")),
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SeparatedList<ArgumentSyntax>()
                                        .Add(SyntaxFactory.Argument(NameOfExpression(declaration.Identifier)))
                                        .Add(SyntaxFactory.Argument(
                                                SyntaxFactory.AnonymousMethodExpression()
                                                    .WithAsyncKeyword(SyntaxFactory.Token(SyntaxKind.AsyncKeyword))
                                                    .WithDelegateKeyword(SyntaxFactory.Token(SyntaxKind.DelegateKeyword))
                                                    .WithParameterList(SyntaxFactory.ParameterList())
                                                    .WithBlock(
                                                        SyntaxFactory.Block(
                                                            SyntaxFactory.ReturnStatement(declaration.ExpressionBody.Expression)))
                                            ))
                                )
                            )
                        )
                    )
                );

                newRoot = oldRoot.ReplaceNode(declaration, newDeclaration).WithAdditionalAnnotations(Formatter.Annotation);
            }

            // Return document with transformed tree.
            return document.WithSyntaxRoot(newRoot);
        }

        private InvocationExpressionSyntax NameOfExpression(SyntaxToken identifierName)
        {
            return SyntaxFactory.InvocationExpression(NameOfIdentifierName())
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList(
                                SyntaxFactory.Argument(SyntaxFactory.IdentifierName(identifierName)))));
        }

        private static IdentifierNameSyntax NameOfIdentifierName()
        {
            return SyntaxFactory.IdentifierName(
                SyntaxFactory.Identifier(
                    SyntaxFactory.TriviaList(), 
                    SyntaxKind.NameOfKeyword,
                    "nameof",
                    "nameof",
                    SyntaxFactory.TriviaList()));
        }

        private CodeFixType DetermineCodeFixType(MethodDeclarationSyntax declaration)
        {
            if (declaration.Modifiers.Any(SyntaxKind.AsyncKeyword))
            {
                if (IsSystemThreadingTasksTask(declaration.ReturnType)
                    || IsVoid(declaration.ReturnType))
                {
                    return CodeFixType.AddAsyncBaseExecuteFunction;
                }

                return CodeFixType.AddAsyncBaseExecuteFunctionWithResult;
            }

            if (!IsVoid(declaration.ReturnType))
            {
                return CodeFixType.AddBaseExecuteFunction;
            }

            return CodeFixType.AddBaseExecuteMethod;
        }

        private bool IsSystemThreadingTasksTask(TypeSyntax returnType)
        {
            return returnType is IdentifierNameSyntax identifier
                && identifier.Identifier.ValueText == "Task";
        }

        private bool IsVoid(TypeSyntax returnType)
        {
            return returnType.Kind() == SyntaxKind.VoidKeyword
                || returnType is PredefinedTypeSyntax predefinedTypeSyntax
                && predefinedTypeSyntax.Keyword.IsKind(SyntaxKind.VoidKeyword);
        }
    }
}
