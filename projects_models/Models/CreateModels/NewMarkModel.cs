using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projects_models.Models.CreateModels
{
    public class NewMarkModel
    {
        /// <summary>
        /// Краткое название
        /// </summary>
        public string ShortName { get; set; }
        /// <summary>
        /// Полное название
        /// </summary>
        public string FullName { get; set; }
    }
}
