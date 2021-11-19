using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiLibrary.Models
{
    public class People
    {
        public HashSet<Person> people { get; set; } = new HashSet<Person>();
    }

    public class Person
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public int Age { get; set; }
    }

}
