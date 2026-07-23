#nullable enable

using System;

namespace Entropia.Core;

public class ConcurrentAccessException : Exception
{
    public ConcurrentAccessException()
        : base("Concurrent access is not allowed") { }

    public ConcurrentAccessException(string objectName)
        : base($"Concurrent access to '{objectName}' is not allowed") { }
}
