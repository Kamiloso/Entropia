#nullable enable

using System;

namespace Entropia.Core;

/// <summary>
/// A marker attribute that indicates that the class or method has to be thread-safe.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class MultiThreadedAttribute : Attribute { }
