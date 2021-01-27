namespace Metabase.GraphQl
{
    public abstract class Connection<TSubject>
    {
        protected readonly TSubject Subject;

        protected Connection(
            TSubject subject
            )
        {
            Subject = subject;
        }
    }
}