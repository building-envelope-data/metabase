namespace Metabase.Data;

public interface IReference
{
    public string? Title { get; }
    public string? Abstract { get; }
    public string? Section { get; }
}