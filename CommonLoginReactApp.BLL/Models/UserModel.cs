using System.Collections.Generic;

namespace CommonLoginReactApp.BLL.Models
{
    public class UserModel
    {
        public long Id { get; set; }

        public string Email { get; set; }

        public string Login { get; set; }

        public ICollection<string> Errors { get; set; } = new List<string>();
    }
}