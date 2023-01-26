using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Model
{
    [Table("article")]
    [PrimaryKey(nameof(id))]
    [Index(nameof(title))]
    //[Index(nameof(content))]
    [Index(nameof(createId))]
    [Index(nameof(createDate))]
    public class Article // : IEquatable<Article>
    {
        public Article(string title, string content, string hashTag) 
        {
            this.title = title;
            this.content = content;
            this.hashTag = hashTag;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; }

        [Column(TypeName = "varchar(100)")]
        public string title { get; set; } = string.Empty;

        [Column(TypeName = "text")]
        public string content { get; set; } = string.Empty;

        [Column("view_count")]
        public int viewCount { get; private set; } = 0;

        [Column("hash_tag", TypeName = "varchar(100)")]
        public string? hashTag { get; set; }

        [Column("create_id", TypeName = "varchar(50)")]
        public string createId { get; private set; } = string.Empty;

        [Column("create_date")]
        public DateTime createDate { get; private set; }

        [Column("update_id", TypeName = "varchar(100)")]
        public string updateId { get; private set; } = string.Empty;

        [Column("update_date")]
        public DateTime updateDate { get; private set; }

        public virtual ICollection<Comment> articleComments { get; set; } = new HashSet<Comment>();

        public static explicit operator Articles(Article article)
        {
            var response = new Articles
            {
                id = article.id,
                title = article.title,
                viewCount = article.viewCount
            };

            return response;
        }
    }

    [NotMapped]
    public class Articles
    {
        public long id { get; set; }
        public string title { get; set; } = string.Empty;
        public int viewCount { get; set; } = 0;
    }
}