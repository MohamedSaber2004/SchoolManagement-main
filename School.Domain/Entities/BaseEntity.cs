namespace School.Domain.Entities
{
    public class BaseEntity<TKey>
    {
        public TKey Id { get; set; } = default!; // PK
    }
}
