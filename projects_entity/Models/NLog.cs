using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projects_entity.Models
{
    [Table("nlog")]
    public class NLog
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }
        /// <summary>
        /// Приложение 
        /// </summary>
        [Column("application")]
        public string Application { get; set; }
        /// <summary>
        /// Время логирования
        /// </summary>
        [Column("logged")]
        public DateTime Logged { get; set; }
        [Column("level")]
        public string Level { get; set; }
        [Column("message")]
        public string Message { get; set; }
        [Column("exception")]
        public string Exception { get; set; }
        [Column("stacktrace")]
        public string StackTrace { get; set; }
        [Column("ip")]
        public string Ip { get; set; }
    }
}
