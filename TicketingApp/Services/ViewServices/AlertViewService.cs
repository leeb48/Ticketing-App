namespace TicketingApp.Services;

public static class AlertViewService
{
    public enum AlertType
    {
        primary,
        success,
        danger
    }

    public static string SendAlert(AlertType alertType, string message)
    {
        return $@"
            <div class='alert alert-{alertType}' role='alert'>
                {message}
            </div>";
    }
}