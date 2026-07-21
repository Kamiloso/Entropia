using static NoEntropy.Other.NamingConventions;
using static NoEntropy.Other.Utils;

namespace NoEntropy.Writers;

internal readonly record struct MonoInfo(
    IReadOnlyList<string> NullCheckNames,
    IReadOnlyList<string> UseComponentTypes,
    IReadOnlyList<string> UseInjectionTypes,
    IReadOnlyList<string> UseUnitySingletonTypes
);

internal record MonoBehaviourPartialWriter(string Name, string Namespace, MonoInfo MonoInfo)
{
    public string Hash { get; } = HashString($"{Namespace}::{Name}");

    public string GenerateHintName() => $"{Name}.g.cs";
    public string GenerateSource()
    {
        string classBody = $$"""
            {{RequireUnityAttributes(MonoInfo.UseComponentTypes)}}
            public partial class {{Name}}
            {
                {{FieldDeclarations(MonoInfo.UseComponentTypes)}}
                {{FieldDeclarations(MonoInfo.UseInjectionTypes)}}
                {{FieldDeclarations(MonoInfo.UseUnitySingletonTypes)}}
                
                [Inject]
                private void Construct_{{Hash}}({{InjectionArgs(MonoInfo.UseInjectionTypes)}})
                {
                    {{NullChecks(MonoInfo.NullCheckNames)}}
                    {{ComponentAssignments(MonoInfo.UseComponentTypes)}}
                    {{InjectionAssignments(MonoInfo.UseInjectionTypes)}}
                    {{UnitySingletonAssignments(MonoInfo.UseUnitySingletonTypes)}}

                    OnInitialize();
                }
            
                partial void OnInitialize();
            }
            """;

        if (Namespace == string.Empty)
        {
            return $$"""
                using System;
                using UnityEngine;
                using VContainer;

                {{classBody}}
                """;
        }
        else
        {
            return $$"""
                using System;
                using UnityEngine;
                using VContainer;

                namespace {{Namespace}}
                {
                    {{classBody}}
                }
                """;
        }
    }

    private string NullChecks(IReadOnlyList<string> names)
    {
        return string.Join("\n", names.Select(name => $"""
            if ({name} == null)
                throw new ArgumentNullException(nameof({name}));
            """));
    }

    private string InjectionArgs(IReadOnlyList<string> types)
    {
        return string.Join(", ", types.Select(type => $"{type} {TypeToArgName(type)}"));
    }

    private string RequireUnityAttributes(IReadOnlyList<string> types)
    {
        return string.Join("\n", types.Select(type => $"""
            [RequireComponent(typeof({type}))]
            """));
    }

    private string FieldDeclarations(IReadOnlyList<string> types)
    {
        return string.Join("\n", types.Select(type => $"""
            private {type} {TypeToFieldName(type)};
            """));
    }

    private string ComponentAssignments(IReadOnlyList<string> types)
    {
        return string.Join("\n", types.Select(type => $"""
            {TypeToFieldName(type)} = GetComponent<{type}>();
            """));
    }

    private string InjectionAssignments(IReadOnlyList<string> types)
    {
        return string.Join("\n", types.Select(type => $"""
            {TypeToFieldName(type)} = {TypeToArgName(type)};
            """));
    }

    private string UnitySingletonAssignments(IReadOnlyList<string> types)
    {
        return string.Join("\n", types.Select(type => $"""
            {TypeToFieldName(type)} = UnitySingletons.Get<{type}>();
            """));
    }
}
