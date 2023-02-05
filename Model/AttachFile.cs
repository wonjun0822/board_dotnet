using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Model
{
    [Table("attachFile")]
    [PrimaryKey(nameof(id))]
    public class AttachFile
    {
        public AttachFile(long articleId, string fileName, string blobName) 
        {
            this.articleId = articleId;
            this.fileName = fileName;
            this.blobName = blobName;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; }

        [Column("article_id")]
        public long articleId { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string fileName { get; set; } = string.Empty;
        
        [Column(TypeName = "varchar(200)")]
        public string blobName { get; set; } = string.Empty;

        [Column("create_by", TypeName = "varchar(50)")]
        public string createBy { get; private set; } = string.Empty;

        [Column("create_at")]
        public DateTime createAt { get; private set; }

        [Column("update_by", TypeName = "varchar(100)")]
        public string updateBy { get; private set; } = string.Empty;

        [Column("update_at")]
        public DateTime updateAt { get; private set; }
    }
}