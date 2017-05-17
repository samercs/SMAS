using SMAS.Data;
using SMAS.Entities;
using System.Threading.Tasks;

namespace SMAS.Services
{
    public class AddressService : ServiceBase
    {
        public AddressService(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
        }

        public async Task<Address> Add(Address address)
        {
            using (var dc=DataContext())
            {
                dc.Addresses.Add(address);
                await dc.SaveChangesAsync();
                return address;
            }
        }

        public async Task<Address> Save(Address address)
        {
            using ( var dc= DataContext())
            {
                dc.SetModified(address);
                await dc.SaveChangesAsync();
                return address;
            }
        }
    }
}
