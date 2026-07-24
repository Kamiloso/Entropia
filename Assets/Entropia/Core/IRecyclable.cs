#nullable enable

namespace Entropia.Core;

public interface IAbandonable
{
    void Abandon();
}

public interface IRecyclable<T> : IAbandonable
{
    void Recycle(T data);
}
