using RM_API.Data;

public class DatabaseTestUtil
{
    private readonly ApplicationDbContext _context;

    public DatabaseTestUtil(ApplicationDbContext context)
    {
        _context = context;
    }

    public bool TestConnection()
    {
        try
        {
            return _context.Database.CanConnect();
        }
        catch
        {
            return false;
        }
    }
}