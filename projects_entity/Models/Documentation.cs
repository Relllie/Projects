using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projects_entity.Models
{
    /// <summary>
    /// Комплект документации
    /// </summary>
    [Table("documentation")]
    public class Documentation
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Идентификатор марки
        /// </summary>
        [Column("mark_id")]
        public int MarkId { get; set; }
        /// <summary>
        /// Номер комплекта
        /// </summary>
        [Column("number")]
        public int Number { get; set; }
        /// <summary>
        /// Идентификатор объекта проектирования
        /// </summary>
        [Column("object_id")]
        public int ObjectId { get; set; }
        public virtual Mark Mark { get; set; }
        public virtual DesignObject Object { get; set; }
    }
}
