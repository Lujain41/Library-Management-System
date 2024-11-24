
using System;

namespace LibraryManagementSystem
{
    public class EmailNotificationService : INotificationService<string>
    {
        public void SendNotificationOnFailure(string data)
        {
            Console.WriteLine(
                $"""
                We encountered an issue processing the following data: '{data}'.
                Please review the input data.
                For more help, visit our FAQ at library.com/faq.
                """);
        }

        public void SendNotificationOnSuccess(string data)
        {
            Console.WriteLine(
                $"""
                Hello,
                The following data has been successfully processed: {data}.
                If you have any queries or feedback, please contact our support team at support@library.com.
                """);
        }
         public void SendNotificationOnDuplicate(string data)
        {
            Console.WriteLine(
                $"""
                Alert: The following data already exists: {data}.
                Please review the details.
                """);
        }
    }
}
