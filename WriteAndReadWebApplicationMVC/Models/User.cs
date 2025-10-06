using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WriteAndReadWebApplicationMVC.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public string login { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string country { get; set; }
        [Required]
        public string street { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public string postcode { get; set; }
        [Required]
        public DateTime birthDate { get; set; }
        [Required]
        public bool admin { get; set; }
        public List<Book> mybooks = new List<Book>();
        public List<Comment> comments = new List<Comment>();
        public List<Block> blocks = new List<Block>();
        public List<Readed> readed = new List<Readed>();
        public User(string login, string email, string password, string country, string street, string city, string postcode, DateTime birthDate, bool admin = false)
        {
            this.login = login;
            this.email = email;
            this.password = password;
            this.country = country;
            this.street = street;
            this.city = city;
            this.postcode = postcode;
            this.birthDate = birthDate;
            this.admin = admin;
        }
        public User(int id, string login, string email, string password, string country, string street, string city, string postcode, DateTime birthDate, bool admin = false)
        {
            this.id = id;
            this.login = login;
            this.email = email;
            this.password = password;
            this.country = country;
            this.street = street;
            this.city = city;
            this.postcode = postcode;
            this.birthDate = birthDate;
            this.admin = admin;
        }
        public User() { }
    }
}
