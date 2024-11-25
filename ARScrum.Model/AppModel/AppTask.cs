using ARScrum.Model.AppEnums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ARScrum.Model.AppModel
{
    public class AppTask : DateTimeClass
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TaskTitle { get; set; } = string.Empty;

        public string? TaskDescription { get; set; }

        public string? AssignedTo { get; set; }

        [Required]
        public int TaskId { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TaskTypeEnum TaskType { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TaskStatusEnum TaskStatus { get; set; } = TaskStatusEnum.New;

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PriorityEnum TaskPriority { get; set; } = PriorityEnum.Medium;

        [Required]
        public int SprintId { get; set; }
        public Sprint? Sprint { get; set; }


    }
}
