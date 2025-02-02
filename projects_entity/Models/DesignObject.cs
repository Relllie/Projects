using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace projects_entity.Models
{
    /// <summary>
    /// Объект проектирования
    /// </summary>
    [Table("object")]
    public class DesignObject
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }
        /// <summary>
        /// Код объекта
        /// </summary>
        [Column("code")]
        public string Code { get; set; }
        /// <summary>
        /// Название объекта
        /// </summary>
        [Column("name")]
        public string Name { get; set; }
        /// <summary>
        /// Идентификатор проекта
        /// </summary>
        [Column("project_id")]
        public int ProjectId { get; set; }
        /// <summary>
        /// Идентификатор родительского объекта
        /// </summary>
        [Column("parent_object_id")]
        public int? ParentObjectId { get; set; }
        [JsonIgnore]
        public virtual Project Project { get; set; }
        [JsonIgnore]
        public virtual DesignObject? ParentObject { get; set; }
        [JsonIgnore]
        public virtual Documentation? Documentation { get; set; }
        public virtual ICollection<DesignObject> ChildObjects { get; set; }
    }
}
