namespace NoEntropy.Writers;

internal abstract record AttributeWriter(string Name)
{
    public string LongName => $"{Name}Attribute";

    public abstract string Target { get; }
    public abstract string AllowMultiple { get; }

    public virtual string ClassBody => "";

    public bool Matches(string attrName, string attrNamespace)
    {
        return attrName == LongName && attrNamespace == nameof(NoEntropy);
    }

    public string GenerateHintName() => $"{Name}Attribute.g.cs";
    public string GenerateSource() => $$"""
        using System;

        namespace {{nameof(NoEntropy)}}
        {
            [AttributeUsage(AttributeTargets.{{Target}}, Inherited = false, AllowMultiple = {{AllowMultiple}})]
            internal sealed class {{Name}}Attribute : Attribute
            {
                {{ClassBody}}
            }
        }
        """;
}
