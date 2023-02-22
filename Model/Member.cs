using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Model
{
    //[SwaggerExclude]
    [Table("member")]
    [PrimaryKey(nameof(id))]
    [Index(nameof(email), IsUnique = true)]
    public class Member
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string pwd { get; set; } = string.Empty;

        [Column(TypeName = "varchar(50)")]
        public string email { get; set; } = string.Empty;

        [Column(TypeName = "varchar(20)")]
        public string nickname { get; set; } = string.Empty;
    }
}