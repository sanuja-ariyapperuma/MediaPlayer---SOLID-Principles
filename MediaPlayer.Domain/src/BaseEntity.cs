namespace MediaPlyer.Domain
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = String.Empty;
    }

}