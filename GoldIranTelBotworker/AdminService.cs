namespace GoldIranTelBotworker;

public class AdminService
{
    private readonly AdminDbContext _context;
    private readonly PanelControler _panelControler;

    public AdminService(AdminDbContext context, PanelControler panelControler)
    {
        _context = context;
        _panelControler = panelControler;
    }

    public bool NewAdmin(string name)
    {
        
        
        return false;
        
    }
}