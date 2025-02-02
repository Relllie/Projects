using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projects_models.Models.CreateModels
{
    public class NewDocModel
    {
        /// <summary>
        /// Идентификатор марки
        /// </summary>
        public int MarkId { get; set; }
        /// <summary>
        /// Номер комплекта
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// Идентификатор объекта проектирования
        /// </summary>
        public int ObjectId { get; set; }
    }
}
