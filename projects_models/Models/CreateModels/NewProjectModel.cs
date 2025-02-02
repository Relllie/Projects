using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace projects_models.Models.CreateModels
{
    public class NewProjectModel
    {
        /// <summary>
        /// Шифр
        /// </summary>
        [JsonPropertyName("cipher")]
        public string Cipher { get; set; }
        /// <summary>
        /// Название проекта
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
