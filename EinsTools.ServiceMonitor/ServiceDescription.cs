namespace EinsTools.ServiceMonitor;

public class ServiceDescription : IEquatable<ServiceDescription>
{
    public bool Equals(ServiceDescription? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return FileName == other.FileName 
               && Arguments.SequenceEqual(other.Arguments) 
               && WorkingDirectory == other.WorkingDirectory 
               && Environment.SequenceEqual(other.Environment) 
               && CreateNoWindow == other.CreateNoWindow 
               && Equals(Logfile, other.Logfile) 
               && Equals(RetryPolicy, other.RetryPolicy)
               && Enabled == other.Enabled;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ServiceDescription)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FileName, Arguments, WorkingDirectory, Environment, CreateNoWindow, Logfile, RetryPolicy);
    }

    public static bool operator ==(ServiceDescription? left, ServiceDescription? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ServiceDescription? left, ServiceDescription? right)
    {
        return !Equals(left, right);
    }

    public string FileName { get; init; } = string.Empty;
    public string[] Arguments { get; init; } = Array.Empty<string>();
    public string? WorkingDirectory { get; init; }
    public Dictionary<string, string?> Environment { get; init; } = new();
    public bool CreateNoWindow { get; init; } = true;
    public LogfileDescription? Logfile { get; init; } = null;
    public RetryPolicy? RetryPolicy { get; init; } = null;
    public bool Enabled { get; init; } = true;
}