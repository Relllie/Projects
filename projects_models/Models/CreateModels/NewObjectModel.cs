using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projects_models.Models.CreateModels
{
    public class NewObjectModel
    {
        /// <summary>
        /// Код объекта
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Название объекта
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Идентификатор проекта
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Идентификатор родительского объекта
        /// </summary>
        public int? ParentObjectId { get; set; }
    }
}
