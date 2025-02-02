using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projects_models.Models.ViewModels
{
    public class DesignObjectModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Название объекта
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Идентификатор родительского объекта
        /// </summary>
        public int? ParentObjectId { get; set; }
        public List<DesignObjectModel> ChildObjects { get; set; }
    }
}
