using System.Data;
using Microsoft.Data.SqlClient;
using WebApp.Services;

namespace WebApp.Models;

public class MemberRepository : BaseRepository
{
    public MemberRepository(IConfiguration configuration) : base(configuration)
    {
    }
    public List<Member> GetMembers()
    {
        using IDbConnection connection = new SqlConnection(connectionString);
        using IDbCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Member";
        connection.Open();
        IDataReader reader = command.ExecuteReader();
        List<Member> list = new List<Member>();
        while (reader.Read())
        {
            list.Add(new Member
            {
                Id = (string)reader["MemberId"],
                Email = (string)reader["Email"],
                GivenName = (string)reader["GivenName"],
                SurName = (string)reader["SurName"],
                Name = (string)reader["Name"],
                IsActived = (bool)reader["IsActived"]
            });
        }
        return list;
    }
    public Member? GetMember(LoginModel obj)
    {
        using IDbConnection connection = new SqlConnection(connectionString);
        using IDbCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Member WHERE Email = @Email AND Password = @Password";
        command.Add(new Parameter{ Name = "@Email", Value = obj.Email});
        command.Add(new Parameter{ Name = "@Password", Value = Helper.Hash(obj.Password), DbType = DbType.Binary});
        connection.Open();
        using IDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Member
            {
                Id = (string) reader["MemberId"],
                GivenName = (string) reader["GivenName"],
                SurName = (string) reader["Surname"],
                Email = (string) reader["Email"],
                Name = (string) reader["Name"]
            };
            
        }
        return null;
    }
    public int Active(string token)
    {
        using IDbConnection connection = new SqlConnection(connectionString);
        using IDbCommand command = connection.CreateCommand();
        command.CommandText = "ActiveAccount";
        command.CommandType = CommandType.StoredProcedure;
        command.Add(new Parameter{ Name = "@Token", Value = token});
        connection.Open();
        return command.ExecuteNonQuery();
    }
    public int Change(ChangeModel obj)
    {
        using IDbConnection connection = new SqlConnection(connectionString);
        using IDbCommand command = connection.CreateCommand();
        command.CommandText = "UPDATE Member SET Password = @NewPassword Where MemberId = @MemberId AND Password = @OldPassword";
        command.Add(new Parameter{ Name = "@OldPassword", Value = Helper.Hash(obj.OldPassword),DbType =DbType.Binary});
        command.Add(new Parameter{ Name = "@NewPassword", Value = Helper.Hash(obj.NewPassword),DbType =DbType.Binary});
        command.Add(new Parameter{ Name = "@MemberId", Value = obj.MemberId});
        connection.Open();
        return command.ExecuteNonQuery();
    }
    public int Add(RegisterModel obj)
    {
        using IDbConnection connection = new SqlConnection(connectionString);
        using IDbCommand command = connection.CreateCommand();
        command.CommandText = "AddMember";
        command.CommandType = CommandType.StoredProcedure;
        command.Add(new Parameter{ Name = "@Id", Value = Helper.RamdomString(32)});
        command.Add(new Parameter{ Name = "@GivenName", Value = obj.GivenName});
        command.Add(new Parameter{ Name = "@Surname", Value = obj.Surname});
        command.Add(new Parameter{ Name = "@Name", Value = obj.GivenName + (obj.Surname != null ? " " + obj.Surname : "")});
        command.Add(new Parameter{ Name = "@Email", Value = obj.Email});
        command.Add(new Parameter{ Name = "@Password", Value = Helper.Hash(obj.Password), DbType = DbType.Binary});
        // command.Add(new Parameter{ Name = "@Token", Value = Helper.RamdomString(32)});
        command.Add(new Parameter{ Name = "@Token", Value = obj.Token});
        connection.Open();
        return command.ExecuteNonQuery();
    }
}