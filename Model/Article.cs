using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Model
{
    [Table("article")]
    [PrimaryKey(nameof(id))]
    [Index(nameof(title))]
    //[Index(nameof(content))]
    [Index(nameof(createBy))]
    [Index(nameof(createAt))]
    public class Article // : IEquatable<Article>
    {
        public Article(string title, string content) 
        {
            this.title = title;
            this.content = content;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; }

        [Column(TypeName = "varchar(100)")]
        public string title { get; set; } = string.Empty;

        [Column(TypeName = "text")]
        public string content { get; set; } = string.Empty;

        [Column("view_count")]
        public int viewCount { get; set; } = 0;

        [Column("hash_tag", TypeName = "varchar(100)")]
        public string? hashTag { get; set; }

        [Column("create_by", TypeName = "varchar(50)")]
        public string createBy { get; private set; } = string.Empty;

        [Column("create_at")]
        public DateTime createAt { get; private set; }

        [Column("update_by", TypeName = "varchar(100)")]
        public string updateBy { get; private set; } = string.Empty;

        [Column("update_at")]
        public DateTime updateAt { get; private set; }

        public virtual ICollection<Comment> articleComments { get; set; } = new HashSet<Comment>();

        public virtual Member member { get; set; } = new Member();
    }
}