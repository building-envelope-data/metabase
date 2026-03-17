using System.Linq;
using HotChocolate;
using Microsoft.Extensions.Logging;

namespace Metabase.Logging;

public static class HotChocolateIErrorTagProvider
{
    public static void RecordTags(ITagCollector collector, IError error)
    {
        collector.Add($"{nameof(error.Message)}", error.Message);
        if (error.Exception is not null)
        {
            collector.Add($"{nameof(error.Exception)}", error.Exception);
        }
        if (error.Code is not null)
        {
            collector.Add($"{nameof(error.Code)}", error.Code);
        }
        if (error.Path is not null)
        {
            collector.Add($"{nameof(error.Path)}", string.Join(".", error.Path));
        }
        if (error.Locations is not null)
        {
            foreach (var (j, location) in error.Locations.Index())
            {
                collector.Add($"{nameof(error.Locations)}.[{j}]", $"{location.Line}:{location.Column}");
            }
        }
        if (error.Extensions is not null)
        {
            foreach (var (j, extension) in error.Extensions.Index())
            {
                collector.Add($"{nameof(error.Extensions)}.[{j}].Key", extension.Key);
                collector.Add($"{nameof(error.Extensions)}.[{j}].Value", extension.Value);
            }
        }
    }
}