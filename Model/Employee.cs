using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Employee
    {
        public int Id
        { get;set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        public int? CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
