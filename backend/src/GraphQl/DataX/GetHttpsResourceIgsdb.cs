using System;

namespace Metabase.GraphQl.DataX;

public sealed class GetHttpsResourceIgsdb
{
    public GetHttpsResourceIgsdb(
        Uri locator
    )
    {
        Locator = locator;
    }

    public Uri Locator { get; }
}