using System.Data;
using Microsoft.Data.SqlClient;
using WebApp.Models;
using WebApp.Services;

public class ProductRepository
{
    string connectionString;
    public ProductRepository(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("Shop") ?? throw new Exception("Connection string 'Shop' not found.");
    }

    static Product Fetch(IDataReader reader)
    {
        return new Product
            {
                Id = (int)reader["ProductId"],
                CategoryId = (byte)reader["CategoryId"],
                // CategoryName = (string)reader["CategoryName"],
                Name = (string)reader["ProductName"],
                Description = (string)reader["Description"],
                Content = (string)reader["Content"],
                Price = (decimal)reader["Price"],
                Quantity = (short)reader["Quantity"],
                SaleOff = reader["SaleOff"] == DBNull.Value ? null : (decimal)reader["SaleOff"],
                ImageUrl = (string)reader["ImageUrl"]
            };
    }

    static Product FetchWithCategory(IDataReader reader)
    {
        Product product = Fetch(reader);
        product.CategoryName = (string)reader["CategoryName"];
        return product;
    }

    public Product? GetProduct(int Id)
    {
        IDbConnection connection = new SqlConnection(connectionString);
        IDbCommand command = connection.CreateCommand();
        command.CommandText = "SELECT Product.*, CategoryName FROM Product JOIN Category ON Product.CategoryId = Category.CategoryId WHERE ProductId = @Id";
        command.Add(new Parameter{ Name= "@Id", Value=Id});
        connection.Open();
        using IDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            return FetchWithCategory(reader);
        }
        return null;
    }

    public int Add(Product obj)
    {
        IDbConnection connection = new SqlConnection(connectionString);
        IDbCommand command = connection.CreateCommand();
        // command.CommandText = "INSERT INTO Product (CategoryId, ProductName, Description, Content, Price, Quantity, SaleOff, ImageUrl) " +
        //     "VALUES (@CategoryId, @Name, @Description, @Content, @Price, @Quantity, @SaleOff, @ImageUrl)";
        command.CommandText ="AddProduct";
        command.CommandType = CommandType.StoredProcedure;
        command.Add(new Parameter{ Name = "@CategoryId", Value = obj.CategoryId });
        command.Add(new Parameter{ Name = "@Name", Value = obj.Name });
        command.Add(new Parameter{ Name = "@Description", Value = obj.Description });
        command.Add(new Parameter{ Name = "@Content", Value = obj.Content });
        command.Add(new Parameter{ Name = "@Price", Value = obj.Price });
        command.Add(new Parameter{ Name = "@Quantity", Value = obj.Quantity });
        command.Add(new Parameter{ Name = "@SaleOff", Value = obj.SaleOff });
        command.Add(new Parameter{ Name = "@ImageUrl", Value = obj.ImageUrl });
        connection.Open();
        return command.ExecuteNonQuery();
    }

    public List<Product> GetProducts()
    {
        using IDbConnection connection = new SqlConnection(connectionString);
        using IDbCommand command = connection.CreateCommand();
        command.CommandText = "SELECT Product.*, CategoryName FROM Product JOIN Category ON Product.CategoryId = Category.CategoryId";
        connection.Open();
        using IDataReader reader = command.ExecuteReader();
        List<Product> list = new List<Product>();
        while (reader.Read())
        {
            list.Add(FetchWithCategory(reader));      
        }
        return list;
       
    }

    public int Update(Product obj)
    {
        using IDbConnection connection = new SqlConnection(connectionString);
        using IDbCommand command = connection.CreateCommand();
        command.CommandText ="UpdateProduct";
        command.CommandType = CommandType.StoredProcedure;
        command.Add(new Parameter{ Name = "@ProductId", Value = obj.Id});
        command.Add(new Parameter{ Name = "@CategoryId", Value = obj.CategoryId });
        command.Add(new Parameter{ Name = "@Name", Value = obj.Name });
        command.Add(new Parameter{ Name = "@Description", Value = obj.Description });
        command.Add(new Parameter{ Name = "@Content", Value = obj.Content });
        command.Add(new Parameter{ Name = "@Price", Value = obj.Price });
        command.Add(new Parameter{ Name = "@Quantity", Value = obj.Quantity });
        command.Add(new Parameter{ Name = "@SaleOff", Value = obj.SaleOff });
        command.Add(new Parameter{ Name = "@ImageUrl", Value = obj.ImageUrl });
        connection.Open();
        return command.ExecuteNonQuery();
    }

    public int Delete(int id)
    {
        using IDbConnection connection = new SqlConnection(connectionString);
        using IDbCommand command = connection.CreateCommand();
        command.CommandText = "DELETE Product WHERE ProductId = @Id";
        command.Add(new Parameter{ Name= "@Id", Value= id});
        connection.Open();
        return command.ExecuteNonQuery();
    }
}