namespace Example.BusinessApp.Infrastructure.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public Customer Clone()
        {
            return new Customer()
            {
                Id = Id,
                Name = Name,
                Email = Email
            };
        }

        public bool Equals(Customer obj)
        {
            if (ReferenceEquals(this, null)) return false;
            if (ReferenceEquals(obj, null)) return false;

            return obj.Id == Id && obj.Name == Name && obj.Email == Email;
        }
    }
}
