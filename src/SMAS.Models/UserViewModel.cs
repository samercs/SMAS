using System;

namespace SMAS.Models
{
    public class UserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime SubscriptionEndDateUtc { get; set; }
        public string SubscriptionStatus { get; set; }

        public static UserViewModel Create(Entities.User user)
        {
            return new UserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                SubscriptionEndDateUtc = user.SubscriptionExpiration ?? DateTime.Today.AddDays(-1),
                SubscriptionStatus = user.SubScription

            };
        }
    }
}
