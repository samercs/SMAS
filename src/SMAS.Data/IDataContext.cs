using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SMAS.Entities;

namespace SMAS.Data
{
    public interface IDataContext : IDisposable
    {
        DbSet<EmailTemplate> EmailTemplates { get; set; }
        DbSet<Address> Addresses { get; set; }

        Task<int> SaveChangesAsync();

        void SetModified(object entity);
    }
}
