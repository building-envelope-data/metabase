using NUnitLite;

namespace Metabase.Tests;

public static class Program
{
    public static int Main(
        string[] commandLineArguments
    )
    {
        return new AutoRun().Execute(commandLineArguments);
    }
}