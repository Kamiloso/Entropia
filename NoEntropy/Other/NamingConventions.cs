using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace NoEntropy.Other;

internal static class NamingConventions
{
    public static string TypeToFieldName(string type)
    {
        type = type.Split('.').Last();

        if (type.Length > 1 && type[0] == 'I' && char.IsUpper(type[1]))
            type = type.Substring(1);

        return type;
    }

    public static string TypeToArgName(string type)
    {
        return ToLowerCamelCase(TypeToFieldName(type));
    }

    public static string ToLowerCamelCase(string input)
    {
        if (input == null)
            throw new ArgumentNullException(nameof(input));

        if (input == string.Empty || char.IsLower(input[0]))
            return input;

        return char.ToLowerInvariant(input[0]) + input.Substring(1);
    }

    public static string FormatCode(string rawCode)
    {
        return CSharpSyntaxTree.ParseText(rawCode)
            .GetRoot()
            .NormalizeWhitespace()
            .ToFullString();
    }
}
