using Infrastructure.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zalo_Clone.ModelViews
{
    public class UncompleteTaskUser
    {
        public long IdTask { get; set; }
        public string? UserSrc { get; set; }
        public string? Content { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Title { get; set; }
        public List<Contact>? ListUserUncompleteThisTask { get; set; }
        public int? RemindCount { get; set; }
        public bool? IsDone { get; set; } =false;
    }
}
