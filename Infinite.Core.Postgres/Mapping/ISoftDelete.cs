namespace Infinite.Core.Postgres.Mapping
{
    public interface ISoftDelete
    {
        bool Excluido { get; set; }
    }
}