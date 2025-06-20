using System.Data;

namespace Publishing.Infrastructure.DataAccess;

/// <summary>
/// Base class for query objects returning <typeparamref name="T"/>.
/// </summary>
public abstract class SqlQuery<T>
{
    /// <summary>SQL statement to execute.</summary>
    public abstract string Sql { get; }

    /// <summary>Parameters object for the statement.</summary>
    public virtual object? Parameters => null;

    /// <summary>Maps a row from <see cref="IDataReader"/> to the result type.</summary>
    public abstract T Map(IDataReader reader);
}
