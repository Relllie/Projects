using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projects_entity.Models
{
    /// <summary>
    /// Справочник марок
    /// </summary>
    [Table("mark")]
    public class Mark
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }
        /// <summary>
        /// Краткон название
        /// </summary>
        [Column("shortname")]
        public string ShortName { get; set; }
        /// <summary>
        /// Полное название
        /// </summary>
        [Column("fullname")]
        public string FullName { get; set; }
    }
}
