namespace Example.BusinessApp.Infrastructure.Models
{
    public class Role
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public Role Clone()
        {
            return new Role()
            {
                Id = Id,
                Description = Description
            };
        }
    }
}