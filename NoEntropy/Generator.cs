using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NoEntropy.Writers;

namespace NoEntropy;

[Generator]
public partial class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static ctx =>
        {
            foreach (AttributeWriter attr in MonoAttributes.All())
            {
                ctx.AddSource(
                    hintName: attr.GenerateHintName(),
                    source: attr.GenerateSource()
                );
            }
        });

        var generationPipeline = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (node, _) => node is ClassDeclarationSyntax,
                transform: static (ctx, _) => GenerationTransform(ctx));

        context.RegisterSourceOutput(generationPipeline, static (spc, writer) =>
        {
            if (writer != null)
            {
                spc.AddSource(
                    hintName: writer.GenerateHintName(),
                    source: writer.GenerateSource()
                );
            }
        });
    }

    private static MonoBehaviourPartialWriter? GenerationTransform(GeneratorSyntaxContext context)
    {
        if (context.Node is not ClassDeclarationSyntax classSyntax)
            return null;

        if (context.SemanticModel.GetDeclaredSymbol(classSyntax) is not INamedTypeSymbol classSymbol)
            return null;

        if (!InheritsFromMonoBehaviour(classSymbol))
            return null;

        List<string> nullCheckNames = [];

        foreach (var member in classSymbol.GetMembers())
        {
            if (member is not IFieldSymbol)
                continue;

            foreach (var attr in member.GetAttributes())
            {
                if (IsMonoAttribute(attr) && attr.AttributeClass?.Name == MonoAttributes.NullCheck.LongName)
                {
                    nullCheckNames.Add(member.Name);
                    break;
                }
            }
        }

        List<string> useComponentTypes = [];
        List<string> useDependencyTypes = [];

        foreach (var attr in classSymbol.GetAttributes())
        {
            if (!IsMonoAttribute(attr))
                continue;

            if (attr.ConstructorArguments.Length != 1 ||
                attr.ConstructorArguments[0].Value is not ITypeSymbol ts)
                continue;

            if (attr.AttributeClass == null)
                continue;

            string attrName = attr.AttributeClass.Name;
            string typeStr = ts.ToDisplayString();

            if (attrName == MonoAttributes.UseComponent.LongName)
                useComponentTypes.Add(typeStr);

            if (attrName == MonoAttributes.UseDependency.LongName)
                useDependencyTypes.Add(typeStr);
        }

        if (nullCheckNames.Count + useComponentTypes.Count + useDependencyTypes.Count == 0)
        {
            return null;
        }

        string nameStr = classSymbol.Name;
        string namespaceStr = classSymbol.ContainingNamespace.IsGlobalNamespace
            ? string.Empty
            : classSymbol.ContainingNamespace.ToDisplayString();

        MonoInfo monoInfo = new(
            NullCheckNames: nullCheckNames,
            UseComponentTypes: useComponentTypes,
            UseDependencyTypes: useDependencyTypes
        );

        return new MonoBehaviourPartialWriter(nameStr, namespaceStr, monoInfo);
    }

    private static bool IsMonoAttribute(AttributeData attribute)
    {
        string? attrName = attribute.AttributeClass?.Name;
        if (attrName == null)
            return false;

        string? attrNamespace = attribute.AttributeClass?.ContainingNamespace?.ToDisplayString();
        if (attrNamespace == null)
            return false;

        foreach (var monoAttr in MonoAttributes.All())
        {
            if (monoAttr.Matches(attrName, attrNamespace))
                return true;
        }

        return false;
    }

    private static bool InheritsFromMonoBehaviour(INamedTypeSymbol? type)
    {
        while (type != null)
        {
            bool nameMatches = type.Name == "MonoBehaviour";
            bool namespaceMatches = type.ContainingNamespace?.ToDisplayString() == "UnityEngine";

            if (nameMatches && namespaceMatches)
                return true;

            type = type.BaseType;
        }

        return false;
    }
}