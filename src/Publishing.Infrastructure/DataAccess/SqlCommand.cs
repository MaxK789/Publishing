namespace Publishing.Infrastructure.DataAccess;

/// <summary>
/// Base class for command objects which modify data.
/// </summary>
public abstract class SqlCommand
{
    /// <summary>SQL statement to execute.</summary>
    public abstract string Sql { get; }

    /// <summary>Parameters object for the statement.</summary>
    public virtual object? Parameters => null;
}
