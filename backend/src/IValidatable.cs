namespace Icon
{
    public interface IValidatable
    {
        public bool IsValid();

        public void EnsureValid();
    }
}