namespace NoEntropy.Writers;

internal record FieldMarkerAttributeWriter(string Name) : AttributeWriter(Name)
{
    public override string Target => "Field";
    public override string AllowMultiple => "false";
}
