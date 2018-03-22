using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace IfTestAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class IfTestAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "IfTestAnalyzer";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = "IfBraceTester";
        private static readonly LocalizableString MessageFormat = "You need curly braces around your if test";
        private static readonly LocalizableString Description = "You need curly braces around your if test";
        private const string Category = "Syntax";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            context.RegisterSyntaxNodeAction(AnalyzeSyntax, SyntaxKind.IfStatement);
        }

        private static void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
        {
            // TODO: Replace the following code with your own analysis, generating Diagnostic objects for any issues you find
            var ifStatement = (IfStatementSyntax)context.Node;

            if (ifStatement.Statement.IsKind(SyntaxKind.Block))
            {
                return;
            }

            var nonBlockStatment = (ExpressionStatementSyntax)ifStatement.Statement;

            if (nonBlockStatment == null)
            {
                return;
            }

            var diagnostic = Diagnostic.Create(Rule, nonBlockStatment.GetLocation());

            context.ReportDiagnostic(diagnostic);


        }
    }
}
