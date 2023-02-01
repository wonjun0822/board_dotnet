using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Model
{
    [Table("comment")]
    [PrimaryKey(nameof(id))]
    [Index(nameof(comment))]
    [Index(nameof(createBy))]
    [Index(nameof(createAt))]
    public class Comment
    {
        public Comment(long articleId, string comment) 
        {
            this.articleId = articleId;
            this.comment = comment;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; }

        [Column("article_id")]
        public long articleId { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string comment { get; set; } = string.Empty;

        [Column("create_by", TypeName = "varchar(50)")]
        public string createBy { get; private set; } = string.Empty;

        [Column("create_at")]
        public DateTime createAt { get; private set; }

        [Column("update_by", TypeName = "varchar(100)")]
        public string updateBy { get; private set; } = string.Empty;

        [Column("update_at")]
        public DateTime updateAt { get; private set; }

        public virtual Member member { get; set; } = new Member();
    }
}