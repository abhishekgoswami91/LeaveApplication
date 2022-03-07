namespace LeaveApp.Core.ViewModel
{
    public class MailModel
    {
        public string From
        {
            get;
            set;
        }
        public string To
        {
            get;
            set;
        }
        public string Subject
        {
            get;
            set;
        }
        public string Body
        {
            get;
            set;
        }
        public int LeaveId { get; set; }
    }
}
