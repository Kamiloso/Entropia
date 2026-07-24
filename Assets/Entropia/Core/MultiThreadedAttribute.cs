#nullable enable

using System;

namespace Entropia.Core;

/// <summary>
/// A marker attribute that indicates that the class or method must be thread-safe.
/// </summary>
[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Interface,
    Inherited = false,
    AllowMultiple = false)]
public sealed class MultiThreadedAttribute : Attribute { }
