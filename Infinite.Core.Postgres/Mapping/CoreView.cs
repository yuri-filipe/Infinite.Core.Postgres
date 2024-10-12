namespace Infinite.Core.Postgres.Mapping
{
    public abstract class CoreView<TKey>
    {
        public TKey Id { get; set; }
    }
}
