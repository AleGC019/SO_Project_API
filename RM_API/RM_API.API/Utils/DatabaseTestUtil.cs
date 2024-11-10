using Microsoft.Data.SqlClient;

namespace RM_API.API.Utils;

public class DatabaseTestUtil
{
    private readonly IConfiguration _configuration;
    
    public DatabaseTestUtil(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public bool TestConnection()
    {
        try
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            connection.Open();
            Console.WriteLine("Conexión exitosa.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error al conectar a la base de datos: {ex.Message}");
            return false;
        }
    }

}