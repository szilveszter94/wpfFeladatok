namespace wpfFeladatok.Mediator;

public static class Mediator
{
    public delegate void LoginStatusChangedEventHandler(string status);
    public static event LoginStatusChangedEventHandler LoginStatusChanged;

    public static void NotifyLoginStatusChanged(string status)
    {
        LoginStatusChanged?.Invoke(status);
    }
}