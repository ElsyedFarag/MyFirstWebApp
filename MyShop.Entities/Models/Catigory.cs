using System.ComponentModel.DataAnnotations;

namespace MyShop.Entities
{
    public class Catigory
    {
        
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreate { get; set; }  = DateTime.Now;

    }
}
