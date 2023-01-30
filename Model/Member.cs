using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Model
{
    [SwaggerExclude]
    [Table("member")]
    [PrimaryKey(nameof(member_id))]
    public class Member
    {
        [Key]
        [Column(TypeName = "varchar(50)")]
        public string member_id { get; set; } = string.Empty;

        [Column(TypeName = "varchar(20)")]
        public string pwd { get; set; } = string.Empty;

        [Column(TypeName = "varchar(20)")]
        public string email { get; set; } = string.Empty;

        [Column(TypeName = "varchar(20)")]
        public string nickname { get; set; } = string.Empty;
    }
}