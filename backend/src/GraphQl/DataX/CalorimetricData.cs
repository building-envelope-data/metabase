using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public sealed class CalorimetricData : Data {
      public List<double> GValues { get; set; }
      public List<double> UValues { get; set; }
    }

}
