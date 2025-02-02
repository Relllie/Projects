using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projects_models.Models.ViewModels
{
    public class InfoModel
    {
        public int ProjectId { get; set; }
        public string ProjectCipher { get; set; }
        public int? ObjectId { get; set; }
        public string FullObjectCode { get; set; } = string.Empty;
        public string DocMark { get; set; } = string.Empty;
        public string DocFullNumber { get; set; } = string.Empty;
        public string FullCipher { get; set; } = string.Empty;
    }
}
