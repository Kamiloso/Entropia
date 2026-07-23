using NoEntropy.Writers;

namespace NoEntropy;

internal static class MonoAttributes
{
    public static readonly FieldMarkerAttributeWriter NullCheck = new("NullCheck");
    public static readonly DependencyAttributeWriter UseComponent = new("UseComponent");
    public static readonly DependencyAttributeWriter UseDependency = new("UseDependency");

    public static IEnumerable<AttributeWriter> All()
    {
        yield return NullCheck;
        yield return UseComponent;
        yield return UseDependency;
    }
}
