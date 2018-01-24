using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DATC
{
    public interface IFacebookService
    {
        Task<Account> GetAccountAsync(string accessToken);
        Task<Account> GetAccountFromStorageAsync(string id);
        Task<int> GetAccountLikesAsync(string accessToken);
        Task PostAccountAsync(Account account);
        Task PutAccountAsync(string id, Account account);
        Task DeleteAccountAsync(string id);
    }

    public class FacebookService : IFacebookService
    {
        private readonly IFacebookClient _facebookClient;

        public FacebookService(IFacebookClient facebookClient)
        {
            _facebookClient = facebookClient;
        }

        public async Task<Account> GetAccountAsync(string accessToken)
        {
            var result = await _facebookClient.GetAsync<dynamic>(
                accessToken, "me", "fields=id,email,name,first_name,last_name,gender,locale");

            if (result == null)
            {
                return new Account();
            }

            var account = new Account
            {
                Id = result.id,
                Email = result.email,
                Name = result.name,
                UserName = result.name,
                FirstName = result.first_name,
                LastName = result.last_name,
                Locale = result.locale,
                Gender = result.gender,
                Likes = 0
            };

            return account;
        }

        public async Task<Account> GetAccountFromStorageAsync(string id)
        {
            var result = await _facebookClient.GetAsync<dynamic>(id);

            if (result == null)
            {
                return new Account();
            }

            var account = new Account
            {
                Id = result.id,
                Email = result.email,
                Name = result.name,
                UserName = result.name,
                FirstName = result.firstName,
                LastName = result.lastName,
                Locale = result.locale,
                Gender = result.gender,
                Likes = result.likes
            };

            return account;
        }

        public async Task<int> GetAccountLikesAsync(string accessToken)
        {
            var postsResult = await _facebookClient.GetAsync<dynamic>(
                accessToken, "me/feed", "fields=likes.limit(0).summary(true)");

            var photosResult = await _facebookClient.GetAsync<dynamic>(
                accessToken, "me/photos", "fields=likes.limit(0).summary(true)");

            int postsSum = 0;
            int photosSum = 0;
            int totalSum = 0;

            foreach(var post in postsResult.data)
            {
                postsSum += (int)post.likes.summary.total_count;
            }
            Console.WriteLine();
            Console.WriteLine($"The number of likes on posts is {postsSum}");

            foreach (var post in photosResult.data)
            {
                photosSum += (int)post.likes.summary.total_count;
            }
            Console.WriteLine($"The number of likes on photos is {photosSum}");

            totalSum = postsSum + photosSum;

            return totalSum;
        }

        public async Task PostAccountAsync(Account account)
            => await _facebookClient.PostAsync(account);

        public async Task PutAccountAsync(string id, Account account)
            => await _facebookClient.PutAsync(id, account);

        public async Task DeleteAccountAsync(string id)
            => await _facebookClient.DeleteAsync(id);
    }
}
