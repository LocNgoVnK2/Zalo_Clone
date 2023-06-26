using Infrastructure.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zalo_Clone.ModelViews
{
    public class ToDoListModel
    {
        public long Id { get; set; }
        public string? UserSrc { get; set; }
        public string? Content { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Title { get; set; }
        [NotMapped]
        public List<string>? UserToDoTask { get; set; }
        public int? RemindCount { get; set; }
        public bool? IsDone { get; set; } =false;
    }
}
