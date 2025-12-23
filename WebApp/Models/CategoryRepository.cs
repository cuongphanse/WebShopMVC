using System.Data;
using Microsoft.Data.SqlClient;
using WebApp.Services;

namespace WebApp.Models;

public class CategoryRepository
{
    string connectionString;
    public CategoryRepository(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("Shop") ?? throw new Exception("Not Found Shop Data");
    }
    public List<Category> GetCategories()
    {
        using IDbConnection connection = new SqlConnection(connectionString);
        using IDbCommand command =  connection.CreateCommand(); // store procedure
        command.CommandText = "SELECT * FROM Category";
        connection.Open();
        using IDataReader reader = command.ExecuteReader();
        List<Category> list = new List<Category>() ;
        while (reader.Read())
        {
            list.Add(Fetch(reader));
        }
        return list;
    }
    // che dau khong cho ng ta thay(tinh che giau)
    static Category Fetch(IDataReader reader)
    {
        return new Category
            {
                Id = (byte)reader["CategoryId"],
                Name = (string)reader["CategoryName"],
                Description = (string)reader["Description"]
            };
    }
    public Category? GetCategoryById(byte categoryID)
    {
        using IDbConnection connection = new SqlConnection(connectionString);
        using IDbCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Category WHERE CategoryId = @categoryID";

        command.Add(new Parameter{ Name = "@categoryID", Value = categoryID });
        // IDbDataParameter parameter = command.CreateParameter();
        // parameter.ParameterName = "@CategoryId";
        // parameter.Value = categoryID;
        // command.Parameters.Add(parameter);
        connection.Open();
        using IDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            Fetch(reader);
        }
        return null;
    }
    public int AddCategory(Category category)
    {
        using IDbConnection connection = new SqlConnection(connectionString);
        using IDbCommand command = connection.CreateCommand();
        command.CommandText = "INSERT INTO Category (CategoryName, Description) VALUES (@Name, @Description)";

        command.Add(new Parameter{ Name = "@Name", Value = category.Name });
        command.Add(new Parameter{ Name = "@Description", Value = category.Description });

        // IDbDataParameter paramName = command.CreateParameter();
        // paramName.ParameterName =  "@Name";
        // paramName.Value = category.Name;
        // command.Parameters.Add(paramName);

        // IDbDataParameter paramDescription = command.CreateParameter();
        // paramDescription.ParameterName = "@Description";
        // paramDescription.Value = category.Description;
        // command.Parameters.Add(paramDescription);

        connection.Open();
        return command.ExecuteNonQuery();
    }

  

    public int UpdateCategory(Category category)
    {
        using IDbConnection connection = new SqlConnection(connectionString);
        using IDbCommand command = connection.CreateCommand();
        command.CommandText = "Update Category SET CategoryName = @Name, Description = @Description WHERE CategoryId = @Id";

        command.Add(new Parameter{ Name = "@Id", Value = category.Id });
        command.Add(new Parameter{ Name = "@Name", Value = category.Name });
        command.Add(new Parameter{ Name = "@Description", Value = category.Description });

        // IDbDataParameter paramCategoryId = command.CreateParameter();
        // paramCategoryId.ParameterName = "@Id";
        // paramCategoryId.Value = category.Id;
        // command.Parameters.Add(paramCategoryId);

        // IDbDataParameter paramName = command.CreateParameter();
        // paramName.ParameterName = "@Name";
        // paramName.Value = category.Name;
        // command.Parameters.Add(paramName);

        // IDbDataParameter paramDescription = command.CreateParameter();
        // paramDescription.ParameterName = "@Description";
        // paramDescription.Value = category.Description;
        // command.Parameters.Add(paramDescription);

        connection.Open();
        return command.ExecuteNonQuery();
    }

    public int DeleteCategory(byte categoryId)
    {
        using IDbConnection connection = new SqlConnection(connectionString);
        using IDbCommand command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Category WHERE CategoryId = @CategoryId";

        command.Add(new Parameter{ Name = "@CategoryId", Value = categoryId });
        // IDbDataParameter paramCategoryId = command.CreateParameter();
        // paramCategoryId.ParameterName = "@CategoryId";
        // paramCategoryId.Value = categoryId;
        // command.Parameters.Add(paramCategoryId);

        connection.Open();
        return command.ExecuteNonQuery();
    }
}