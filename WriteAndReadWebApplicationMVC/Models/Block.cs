using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WriteAndReadWebApplicationMVC.Models
{
    [Table("BlockedUsers")]
    public class Block
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [ForeignKey(nameof(userId))]
        public User user { get; set; }
        public int userId { get; set; }
        [Required]
        public DateTime blockStart { get; set; }
        [Required]
        public DateTime blockEnd { get; set; }
        public Block(User user, DateTime blockStart, DateTime blockEnd)
        {
            this.user = user;
            this.userId = user.id;
            this.blockStart = blockStart;
            this.blockEnd = blockEnd;
        }
        public Block(int userId, DateTime blockStart, DateTime blockEnd)
        {
            this.userId = userId;
            this.blockStart = blockStart;
            this.blockEnd = blockEnd;
        }
        public Block() { }
    }
}
