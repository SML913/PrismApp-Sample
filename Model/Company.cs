using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Company
    {
        public Company()
        {
            Employees = new List<Employee>();
        }
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
