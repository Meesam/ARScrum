using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARScrum.Model.AppModel
{
    public class Sprint : DateTimeClass
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "SprintTitle cannot be empty")]
        public string SprintTitle { get; set; } = string.Empty;
        public string? SprintDescription { get; set; }
        public DateTime? SprintStartDate { get; set; }
        public DateTime? SprintEndDate { get; set; }
        [Required]
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
        public List<AppTask> SprintTasks { get; set; } = new List<AppTask>();

    }
}
