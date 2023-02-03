using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Model
{
    [Table("file")]
    [PrimaryKey(nameof(id))]
    public class File
    {
        public File(long articleId, string fileName) 
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
    }
}