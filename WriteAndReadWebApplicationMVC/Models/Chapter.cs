using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WriteAndReadWebApplicationMVC.Models
{
    [Table("Chapters")]
    public class Chapter
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [ForeignKey(nameof(bookId))]
        [Required]
        public Book book { get; set; }
        public int bookId { get; set; }
        [Required]
        public int orderInBook { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public string content { get; set; }
        [Required]
        public int readCounter { get; set; }
        [Required]
        public int uniqueReadCounter { get; set; }
        [Required]
        public DateTime createTime { get; set; }
        [Required]
        public DateTime updateTime { get; set; }

        public List<Comment> comments = new List<Comment>();
        public List<Readed> readers = new List<Readed>();
        public Chapter(Book book, int orderInBook, string title, string content, int readCounter, int uniqueReadCounter, DateTime createTime, DateTime updateTime)
        {
            this.book = book;
            this.bookId = book.id;
            this.orderInBook = orderInBook;
            this.title = title;
            this.content = content;
            this.readCounter = readCounter;
            this.uniqueReadCounter = uniqueReadCounter;
            this.createTime = createTime;
            this.updateTime = updateTime;
        }
        public Chapter(int bookId, int orderInBook, string title, string content, int readCounter, int uniqueReadCounter, DateTime createTime, DateTime updateTime)
        {
            this.bookId = bookId;
            this.orderInBook = orderInBook;
            this.title = title;
            this.content = content;
            this.readCounter = readCounter;
            this.uniqueReadCounter = uniqueReadCounter;
            this.createTime = createTime;
            this.updateTime = updateTime;
        }
        public Chapter() { }
    }
}
