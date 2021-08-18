namespace Metabase.GraphQl.DataX
{
    public sealed class CielabColor
    {
        public double LStar { get; }
        public double AStar { get; }
        public double BStar { get; }

        public CielabColor(
            double lStar,
            double aStar,
            double bStar
        )
        {
            LStar = lStar;
            AStar = aStar;
            BStar = bStar;
        }
    }
}