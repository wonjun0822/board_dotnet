using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Model
{
    [Table("attachFile")]
    [PrimaryKey(nameof(id))]
    [Index(nameof(articleId))]
    public class AttachFile
    {
        public AttachFile(long articleId, string fileName) 
        {
            this.articleId = articleId;
            this.fileName = fileName;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; }

        [Column("article_id")]
        public long articleId { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string fileName { get; set; } = string.Empty;

        [Column("create_by")]
        public long createBy { get; private set; }

        [Column("create_at")]
        public DateTime createAt { get; private set; }

        [Column("update_by")]
        public long updateBy { get; private set; }

        [Column("update_at")]
        public DateTime updateAt { get; private set; }
    }
}