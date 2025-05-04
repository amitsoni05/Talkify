namespace ChatAppMVC.Models
{
    public class UserMasterViewModel
    {

        public int? UserId { get; set; }
        public string UserName
        {
            get
            {
                if (UserId.HasValue)
                {
                    return UserId.Value == 1 ? "User 1" : UserId.Value == 2 ? "User 2" : "Unknown User";
                }
                return "Unknown User";
            }
        }
    }
}
