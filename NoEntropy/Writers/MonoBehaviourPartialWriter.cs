using Microsoft.CodeAnalysis;
using static NoEntropy.Other.NamingConventions;
using static NoEntropy.Other.Utils;

namespace NoEntropy.Writers;

internal readonly record struct MonoInfo(
    IReadOnlyList<string> NullCheckNames,
    IReadOnlyList<string> UseComponentTypes,
    IReadOnlyList<string> UseDependencyTypes
);

internal record MonoBehaviourPartialWriter(string Name, string Namespace, MonoInfo MonoInfo)
{
    public string Hash { get; } = HashString($"{Namespace}::{Name}").Substring(0, 16);

    public string GenerateHintName() => $"{Name}.g.cs";
    public string GenerateSource()
    {
        string classBody = $$"""
            {{RequireUnityAttributes(MonoInfo.UseComponentTypes)}}
            partial class {{Name}}
            {
                {{PropertyDeclarations(MonoInfo.UseComponentTypes)}}
                {{PropertyDeclarations(MonoInfo.UseDependencyTypes)}}
                
                [Inject]
                private void Construct_{{Hash}}({{DependencyArgs(MonoInfo.UseDependencyTypes)}})
                {
                    {{NullChecks(MonoInfo.NullCheckNames)}}
                    {{ComponentAssignments(MonoInfo.UseComponentTypes)}}
                    {{DependencyAssignments(MonoInfo.UseDependencyTypes)}}

                    OnInitialize();
                }
            
                partial void OnInitialize();
            }
            """;

        if (Namespace == string.Empty)
        {
            return FormatCode($$"""
                using System;
                using UnityEngine;
                using VContainer;

                {{classBody}}
                """);
        }
        else
        {
            return FormatCode($$"""
                using System;
                using UnityEngine;
                using VContainer;

                namespace {{Namespace}}
                {
                    {{classBody}}
                }
                """);
        }
    }

    private string NullChecks(IReadOnlyList<string> names)
    {
        return string.Join("\n", names.Select(name => $"""
            if ({name} == null)
                throw new ArgumentNullException(nameof({name}));
            """));
    }

    private string DependencyArgs(IReadOnlyList<string> types)
    {
        return string.Join(", ", types.Select(type => $"{type} {TypeToArgName(type)}"));
    }

    private string RequireUnityAttributes(IReadOnlyList<string> types)
    {
        return string.Join("\n", types.Select(type => $"""
            [RequireComponent(typeof({type}))]
            """));
    }

    private string PropertyDeclarations(IReadOnlyList<string> types)
    {
        return string.Join("\n", types.Select(type => $$"""
            public {{type}} {{TypeToFieldName(type)}} { get; private set; }
            """));
    }

    private string ComponentAssignments(IReadOnlyList<string> types)
    {
        return string.Join("\n", types.Select(type => $"""
            {TypeToFieldName(type)} = GetComponent<{type}>();
            """));
    }

    private string DependencyAssignments(IReadOnlyList<string> types)
    {
        return string.Join("\n", types.Select(type => $"""
            {TypeToFieldName(type)} = {TypeToArgName(type)};
            """));
    }
}
