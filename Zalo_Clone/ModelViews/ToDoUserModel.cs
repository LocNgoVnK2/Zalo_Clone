using System.ComponentModel.DataAnnotations;

namespace Zalo_Clone.ModelViews
{
    public class ToDoUserModel
    {
        public long TaskId { get; set; }
  
        public string UserDes { get; set; }
        public int Status { get; set; } = 0;
    }
}
