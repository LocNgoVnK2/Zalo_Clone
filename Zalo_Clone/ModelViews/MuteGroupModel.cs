using System.ComponentModel.DataAnnotations;

namespace Zalo_Clone.ModelViews
{
    public class MuteGroupModel
    {
        public string User { get; set; }
        public string GroupId { get; set; }
        public DateTime MuteTime { get; set; }
        public DateTime? EndMuteTime { get; set; }
    }
}
