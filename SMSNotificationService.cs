using System;

namespace LibraryManagementSystem
{
    public class SMSNotificationService : INotificationService<string>
    {
        public void SendNotificationOnFailure(string data)
        {
            Console.WriteLine(
                $"""
                Error adding '{data}'. Please email support@library.com for assistance.
                """);
        }

        public void SendNotificationOnSuccess(string data)
        {
            Console.WriteLine(
                $"""
                Success! '{data}' added to Library. Thank you!
                """);
        }
        public void SendNotificationOnDuplicate(string data)
        {
            Console.WriteLine(
                $"""
                Duplicate entry: '{data}'. Please check your input.
                """);
        }
    }
}
