#nullable enable

using System;

namespace Entropia.Core;

public class ConcurrentAccessException : Exception
{
    public ConcurrentAccessException()
        : base("Concurrent access is not allowed") { }

    public ConcurrentAccessException(Type type)
        : base($"Concurrent access to type '{type.Name}' is not allowed") { }
}
