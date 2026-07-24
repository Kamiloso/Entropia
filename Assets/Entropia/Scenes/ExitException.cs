#nullable enable

using System;

namespace Entropia.Scenes;

public class ExitException : Exception
{
    public ExitException(string message)
        : base(message) { }

    public ExitException(string message, Exception innerException)
        : base(message, innerException) { }
}
