namespace HrbiApp.API.Models.Account
{
    public class UserNotificationModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public int Type { get; set; }
        public string DataID { get; set; }
        public string Time { get; set; }
    }
}
