using System.Reflection.Metadata.Ecma335;

namespace CustoomerToken.Services
{
    public class NotifyServices
    {
        public NotifyServices()
        {
            Notifications = new();
        }

        public List<Notification> Notifications { get; set; }

        public void NotifyError(string message, string header = null)
        {
            Notifications.Add(new Notification(NotificationType.Error, message, header));
        }

        public void NotifySucess(string message, string header = null)
        {
            Notifications.Add(new Notification(NotificationType.Sucess, message, header));
        }

    }

    public class Notification
    {
        public NotificationType Type { get; set; }
        public string Header { get; set; }
        public string Message { get; set; }

        public Notification(NotificationType type, string message, string header = null)
        {
            Type = type;
            Message = message;
            Header = header;
        }

    }

    public enum NotificationType
    {
        Sucess,
        Error
    }
}
