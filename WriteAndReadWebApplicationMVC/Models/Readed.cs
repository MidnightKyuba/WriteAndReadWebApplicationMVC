using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WriteAndReadWebApplicationMVC.Models
{
    [Table("Readed")]
    public class Readed
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
        [ForeignKey(nameof(userId))]
        public Chapter chapter { get; set; }
        public int chapterId { get; set; }
        public Readed(User user, Chapter chapter)
        {
            this.user = user;
            this.userId = user.id;
            this.chapter = chapter;
            this.chapterId = chapter.id;
        }

        public Readed(int userId, int chapterId)
        {
            this.userId = userId;
            this.chapterId = chapterId;
        }
        public Readed() { }
    }
}
