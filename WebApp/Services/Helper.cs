using System.Data;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace WebApp.Services;

public static class Helper
{ 
    public static int Add(this IDbCommand command, Parameter parameter)
    {
        IDbDataParameter dbParameter = command.CreateParameter();
        dbParameter.ParameterName = parameter.Name;
        dbParameter.Value = parameter.Value ?? DBNull.Value;
        return command.Parameters.Add(dbParameter);
    }

    public static byte[] Hash(string text)
    {
        using HashAlgorithm algorithm = SHA512.Create();
        return algorithm.ComputeHash(Encoding.ASCII.GetBytes(text));
    }
    //tao chuoi ngau nhien
    public static string RamdomString(int length)
    {
        const string pattern = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        char[] arr = new char[length];
        Random random = new Random();
        for (int i = 0; i < length; i++)
        {
            arr[i] = pattern[random.Next(0,pattern.Length)];
        }
        return string.Join(string.Empty, arr);
    }
    public static async Task<bool> SendMail(MailSetting setting, string email, string subject, string body)
    {
        try
        {
            using SmtpClient smtpClient = new SmtpClient
            {
                Host = setting.Host,
                EnableSsl = true,
                Credentials = new NetworkCredential(setting.Email, setting.Password)
            };
            MailAddress mailFrom = new MailAddress(setting.Email, displayName: setting.DisplayName);
            MailMessage message = new MailMessage{ Body = body, Subject = subject, From = mailFrom, IsBodyHtml = true};
            message.To.Add(new MailAddress(email));
            await smtpClient.SendMailAsync(message);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}