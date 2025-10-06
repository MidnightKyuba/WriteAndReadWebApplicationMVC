using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WriteAndReadWebApplicationMVC.Models
{
    [Table("Books")]
    public class Book
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [ForeignKey(nameof(authorId))]
        public User author { get; set; }
        public int authorId { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public string description { get; set; }
        [Required]
        public DateTime createDate { get; set; }
        [Required]
        public DateTime updateDate { get; set; }
        public List<Chapter> chapters = new List<Chapter>();
        public Book(User author, string title, string description, DateTime createDate, DateTime updateDate)
        {
            this.author = author;
            this.authorId = author.id;
            this.title = title;
            this.description = description;
            this.createDate = createDate;
            this.updateDate = updateDate;
        }
        public Book(int authorId, string title, string description, DateTime createDate, DateTime updateDate)
        {
            this.authorId = authorId;
            this.title = title;
            this.description = description;
            this.createDate = createDate;
            this.updateDate = updateDate;
        }
        public Book() { }
    }
}
