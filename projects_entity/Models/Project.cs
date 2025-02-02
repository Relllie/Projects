using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace projects_entity.Models
{
    /// <summary>
    /// Проект
    /// </summary>
    [Table("project")]
    public class Project
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }
        /// <summary>
        /// Шифр
        /// </summary>
        [Column("cipher")]
        public string Cipher { get; set; }
        /// <summary>
        /// Название проекта
        /// </summary>
        [Column("name")]
        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<DesignObject>? Objects { get; set; }
    }
}
