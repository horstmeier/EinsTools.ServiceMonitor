namespace EinsTools.ServiceMonitor;

public class LogfileDescription : IEquatable<LogfileDescription>
{
    public bool Equals(LogfileDescription? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return FileName == other.FileName 
               && MaxSizeInMB == other.MaxSizeInMB 
               && RetainedFileCountLimit == other.RetainedFileCountLimit;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((LogfileDescription)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FileName, MaxSizeInMB, RetainedFileCountLimit);
    }

    public static bool operator ==(LogfileDescription? left, LogfileDescription? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(LogfileDescription? left, LogfileDescription? right)
    {
        return !Equals(left, right);
    }

    public string? FileName { get; init; } = null;
    public int MaxSizeInMB { get; init; } = 10;
    public int RetainedFileCountLimit { get; init; } = 5;
}