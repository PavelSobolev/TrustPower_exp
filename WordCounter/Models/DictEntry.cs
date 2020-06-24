using System.ComponentModel.DataAnnotations.Schema;

namespace WordCounter.Models
{
    /// <summary>
    /// Model which is used in DB and classes of collections
    /// </summary>
    public class DictEntry
    {
     
        // names of properties are set according to requirements
        [Column("SaltedHash")]
        public string Id { get; set; }
        public string Word { get; set; }
        public int Count { get; set; }
    }
}
