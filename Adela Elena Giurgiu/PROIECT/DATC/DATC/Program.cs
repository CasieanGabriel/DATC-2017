using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DATC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var accessToken = "EAACEdEose0cBAAcr3TR4i4jNzafqaDZBGsD13fXKdyKmdsQbemVlrNBket9CCFJstpsubRbepVmOw3E4WD6zkyqgNQVzz2pYTWLT9Bkj8zl3sdpTbvGDEjBofD76jRpbMBi2ZBYnLSq6aZA9VZCM9TWiZBh3STsKW9Uuy66m2V9SBUyvme07XCZAoawCJWTzAZD";
            var facebookClient = new FacebookClient();
            var facebookService = new FacebookService(facebookClient);

            var getAccountTask = facebookService.GetAccountAsync(accessToken);
            Task.WaitAll(getAccountTask);
            var account = getAccountTask.Result;
            Console.WriteLine($"Hello {account.Name}!");

            int opt;
            do
            {
                Console.WriteLine();
                Console.WriteLine("0. Exit");
                Console.WriteLine("1. Store the account to the database");
                Console.WriteLine("2. Get the account from the database");
                Console.WriteLine("3. Get the number of likes");
                Console.WriteLine("4. Update the account with the number of likes");
                Console.WriteLine("5. Delete the account from the database");
                Console.WriteLine("Please choose an options: ");
                opt = int.Parse(Console.ReadLine());
                switch(opt)
                {
                    case 0:
                        break;
                    case 1:
                        var postAccountTask = facebookService.PostAccountAsync(account);
                        Task.WaitAll(postAccountTask);
                        Console.WriteLine();
                        Console.WriteLine("Account stored!");
                        break;
                    case 2:
                        var getAccountFromStorageTask = facebookService.GetAccountFromStorageAsync(account.Id);
                        Task.WaitAll(getAccountFromStorageTask);
                        var storageAccount = getAccountFromStorageTask.Result;
                        Console.WriteLine();
                        Console.WriteLine($"ID: {storageAccount.Id}");
                        Console.WriteLine($"First name: {storageAccount.FirstName}");
                        Console.WriteLine($"Last name: {storageAccount.LastName}");
                        Console.WriteLine($"Email: {storageAccount.Email}");
                        Console.WriteLine($"Gender: {storageAccount.Gender}");
                        Console.WriteLine($"Likes: {storageAccount.Likes}");
                        break;
                    case 3:
                        var getLikesTask = facebookService.GetAccountLikesAsync(accessToken);
                        Task.WaitAll(getLikesTask);
                        var likes = getLikesTask.Result;
                        Console.WriteLine();
                        Console.WriteLine($"The total number of likes is {likes}");
                        account.Likes = likes;
                        break;
                    case 4:
                        var putAccountTask = facebookService.PutAccountAsync(account.Id, account);
                        Task.WaitAll(putAccountTask);
                        Console.WriteLine();
                        Console.WriteLine("Account updated!");
                        break;
                    case 5:
                        var deleteAccountTask = facebookService.DeleteAccountAsync(account.Id);
                        Task.WaitAll(deleteAccountTask);
                        Console.WriteLine();
                        Console.WriteLine("Account deleted!");
                        break;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("Wrong option!");
                        break;
                }
            } while (opt != 0);
        }
    }
}
