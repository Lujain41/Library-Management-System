namespace LibraryManagementSystem
{
    public interface INotificationService<T>
{ 
    // Method to send a notification when an operation is successful
    void SendNotificationOnFailure(T data);
    // Method to send a notification when an operation fails
    void SendNotificationOnSuccess(T data);
    // Method to send a notification when an operation duplicate
     void SendNotificationOnDuplicate(T data);
}
}
