namespace Example.BusinessApp.Infrastructure.Models
{
    public class User
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public int Id { get; set; }

        public bool IsCool { get; set; }

        public bool? IsCoolNullable { get; set; }

        public double Length { get; set; }

        public int BirthYear { get; set; }
            
        public decimal NetValue { get; set; }

        public User Clone()
        {
            return new User()
            {
                Id = Id,
                Name = Name,
                Email = Email
            };
        }
    }
}
