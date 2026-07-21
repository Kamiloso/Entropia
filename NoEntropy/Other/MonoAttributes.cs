using NoEntropy.Writers;

namespace NoEntropy.Other;

internal static class MonoAttributes
{
    public static readonly FieldMarkerAttributeWriter NullCheck = new("NullCheck");
    public static readonly DependencyAttributeWriter UseComponent = new("UseComponent");
    public static readonly DependencyAttributeWriter UseInjection = new("UseInjection");
    public static readonly DependencyAttributeWriter UseUnitySingleton = new("UseUnitySingleton");

    public static IEnumerable<AttributeWriter> All()
    {
        yield return NullCheck;
        yield return UseComponent;
        yield return UseInjection;
        yield return UseUnitySingleton;
    }
}
