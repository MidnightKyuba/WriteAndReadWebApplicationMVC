using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WriteAndReadWebApplicationMVC.Models
{
    [Table("Comments")]
    public class Comment
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [ForeignKey(nameof(chapterId))]
        [Required]
        public Chapter chapter { get; set; }
        public int chapterId { get; set; }
        [ForeignKey(nameof(userId))]
        [Required]
        public User user { get; set; }
        public int userId { get; set; }
        [Required]
        public string content { get; set; }
        [Required]
        public DateTime writeDate { get; set; }
        public Comment(Chapter chapter, User user, string content, DateTime writeDate)
        {
            this.chapter = chapter;
            this.chapterId = chapter.id;
            this.user = user;
            this.userId = user.id;
            this.content = content;
            this.writeDate = writeDate;
        }
        public Comment(int chapterId, int userId, string content, DateTime writeDate)
        {
            this.chapterId = chapterId;
            this.userId = userId;
            this.content = content;
            this.writeDate = writeDate;
        }
        public Comment() { }
    }
}
