﻿namespace Example.BusinessApp.Infrastructure.Models
{
    public class Language
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public bool Equals(Language obj)
        {
            return Code == obj.Code;
        }
    }
}
