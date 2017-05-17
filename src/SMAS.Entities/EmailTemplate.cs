using SMAS.Entities.Enum;
using System.ComponentModel.DataAnnotations;
using OrangeJetpack.Localization;

namespace SMAS.Entities
{
    public class EmailTemplate: EntityBase, ILocalizable
    {
        public int EmailTemplateId { get; set; }

        public EmailTemplateType TemplateType { get; set; }

        [Required]
        public string Description { get; set; }

        [Required, Localized, Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required, Localized, Display(Name = "Message")]
        [UIHint("TextArea")]
        public string Message { get; set; }
    }
}
