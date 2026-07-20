#nullable enable

using System;

namespace Entropia.Core;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class MultiThreadedAttribute : Attribute { }
