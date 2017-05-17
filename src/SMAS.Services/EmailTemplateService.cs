using Microsoft.EntityFrameworkCore;
using SMAS.Data;
using SMAS.Entities;
using SMAS.Entities.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrangeJetpack.Localization;

namespace SMAS.Services
{
    public class EmailTemplateService : ServiceBase
    {
        public EmailTemplateService(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
        }

        public async Task<IEnumerable<EmailTemplate>> GetAll()
        {
            using (var dc = DataContext())
            {
                return await dc.EmailTemplates.ToListAsync();
            }
        }

        public async Task<EmailTemplate> GetByTemplateType(EmailTemplateType emailTemplateType, string language = "en")
        {
            using (var dc = DataContext())
            {
                var template = await dc.EmailTemplates.SingleOrDefaultAsync(i => i.TemplateType == emailTemplateType);
                template.Localize(language);
                return template;

            }
        }

        public async Task<EmailTemplate> Save(EmailTemplate emailTemplate)
        {
            using (var dc = DataContext())
            {
                dc.SetModified(emailTemplate);
                await dc.SaveChangesAsync();
                return emailTemplate;
            }
        }
    }
}
