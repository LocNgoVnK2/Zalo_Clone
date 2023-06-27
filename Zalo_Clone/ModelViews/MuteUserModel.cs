using System.ComponentModel.DataAnnotations;

namespace Zalo_Clone.ModelViews
{
    public class MuteUserModel
    {
        public string User { get; set; }
        public string Receiver { get; set; }
        public DateTime MuteTime { get; set; }
        public DateTime? EndMuteTime { get; set; }
    }
}
