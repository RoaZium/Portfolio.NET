
namespace PrismWPF.Common.Infrastructure
{
    public static class StringUtils
    {
        public static bool IsEmail(this string txt)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt))
                {
                    System.Net.Mail.MailAddress addr = new System.Net.Mail.MailAddress(txt);
                    return addr.Address == txt;
                }
            }
            catch
            {
            }
            return false;
        }
    }
}
