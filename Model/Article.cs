using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Model
{
    [Table("article")]
    [PrimaryKey(nameof(id))]
    [Index(nameof(title))]
    [Index(nameof(content))]
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

        [Column(TypeName = "varchar(2000)")]
        public string content { get; set; } = string.Empty;

        [Column("view_count")]
        public int viewCount { get; set; } = 0;

        [Column("hash_tag", TypeName = "varchar(100)")]
        public string? hashTag { get; set; }

        [Column("create_by")]
        public long createBy { get; private set; }

        [Column("create_at")]
        public DateTime createAt { get; private set; }

        [Column("update_by")]
        public long updateBy { get; private set; }

        [Column("update_at")]
        public DateTime updateAt { get; private set; }

        public virtual ICollection<Comment> articleComments { get; set; } = new HashSet<Comment>();

        public virtual ICollection<AttachFile> articleFiles { get; set; } = new HashSet<AttachFile>();

        public virtual Member member { get; set; } = new Member();
    }
}