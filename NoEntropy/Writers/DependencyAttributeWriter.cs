namespace NoEntropy.Writers;

internal record DependencyAttributeWriter(string Name) : AttributeWriter(Name)
{
    public override string Target => "Class";
    public override string AllowMultiple => "true";

    public override string ClassBody => $"public {LongName}(Type type) {{ }}";
}
