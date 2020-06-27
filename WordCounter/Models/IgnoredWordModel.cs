using System.ComponentModel.DataAnnotations;

namespace WordCounter.Models
{
    public class IgnoredWordModel
    {
        [Key]
        public string Word { get; set; }
    }
}
