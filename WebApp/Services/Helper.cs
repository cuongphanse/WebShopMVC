using System.Data;

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
}