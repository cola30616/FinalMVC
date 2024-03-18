using FinalGroupMVCPrj.Models.DTO;

namespace FinalGroupMVCPrj.Services
{
    public interface IMailService
    {
        bool SendMail(MailData mailData);
    }
}
