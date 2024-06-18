namespace Metabase.GraphQl.DataX;

public sealed class GetHttpsResourceTreeIgsdb
{
    public GetHttpsResourceTreeIgsdb(
        GetHttpsResourceTreeRootIgsdb root
    )
    {
        Root = root;
    }

    public GetHttpsResourceTreeRootIgsdb Root { get; }
}