namespace EinsTools.ServiceMonitor;

public class RetryPolicy : IEquatable<RetryPolicy>
{
    public bool Equals(RetryPolicy? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return MaxRetryCount == other.MaxRetryCount && DelayInSeconds == other.DelayInSeconds;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((RetryPolicy)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(MaxRetryCount, DelayInSeconds);
    }

    public static bool operator ==(RetryPolicy? left, RetryPolicy? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(RetryPolicy? left, RetryPolicy? right)
    {
        return !Equals(left, right);
    }

    // -1 means infinite
    public int MaxRetryCount { get; init; } = -1;
    public int DelayInSeconds { get; init; } = 30;
}