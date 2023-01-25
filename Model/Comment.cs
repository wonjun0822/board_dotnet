using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Model
{
    [Table("comment")]
    [PrimaryKey(nameof(id))]
    [Index(nameof(comment))]
    [Index(nameof(createId))]
    [Index(nameof(createDate))]
    public class Comment
    {
        public Comment(string comment) 
        {
            this.comment = comment;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; }

        [Column(TypeName = "varchar(200)")]
        public string comment { get; set; } = string.Empty;

        [Column("create_id", TypeName = "varchar(50)")]
        public string createId { get; private set; } = string.Empty;

        [Column("create_date")]
        public DateTime createDate { get; private set; }

        [Column("update_id", TypeName = "varchar(100)")]
        public string updateId { get; private set; } = string.Empty;

        [Column("update_date")]
        public DateTime updateDate { get; private set; }

        [Column("article_id")]
        public long articleId { get; set; }
    }
}