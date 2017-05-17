using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OrangeJetpack.Base.Core.Formatting;


namespace SMAS.Entities
{
    public class User : IdentityUser<int>
    {
        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }
        [StringLength(4, MinimumLength = 2)]
        public string PhoneCountryCode { get; set; }
        [StringLength(15, MinimumLength = 7)]
        public string PhoneLocalNumber { get; set; }
        public override string PhoneNumber => PhoneFormatter.Format(PhoneCountryCode, PhoneLocalNumber);

        public bool IsDeleted { get; set; }

        [Required, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public DateTime CreatedUtc { get; set; }
        public DateTime? ModifiedUtc { get; set; }
        public DateTime? DeletedUtc { get; set; }
        [Column(TypeName = "date")]
        public DateTime? SubscriptionExpiration { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        
        [NotMapped]
        public string SubScription
        {
            get
            {
                if (!SubscriptionExpiration.HasValue)
                {
                    return "No Subscription";
                }
                else if (SubscriptionExpiration.Value >= DateTime.Today)
                {
                    return "Active Subscription";
                }
                else if (SubscriptionExpiration.Value < DateTime.Today)
                {
                    return "Expired Subscription";
                }
                return "";
            }
        }
        [NotMapped]
        public string Expiration
        {
            get
            {
                if (!SubscriptionExpiration.HasValue)
                {
                    return "";
                }
                if (SubscriptionExpiration.Value >= DateTime.Today)
                {
                    return SubscriptionExpiration.Value.Subtract(DateTime.Today).Days.ToString();
                }
                return "";
            }
        }
    }
}
