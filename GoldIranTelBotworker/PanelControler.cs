using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
namespace GoldIranTelBotworker;

public class PanelControler
{
     private static long Gb = 1073741824;
    private readonly Dictionary<int, string> ActionPathes = new Dictionary<int, string>()
    {
        { 0 , "panel/api/inbounds/" },
        { 1, "panel/api/inbounds/list" },
        { 2, "panel/api/inbounds/get/" },
        { 3, "panel/api/inbounds/getClientTraffics/" },
        { 4, "panel/api/inbounds/getClientTrafficsById/" },
        { 5, "panel/api/inbounds/createbackup" },
        { 6, "panel/api/inbounds/add" },
        { 7, "panel/api/inbounds/del/" },
        { 8, "panel/api/inbounds/update/" },
        { 9, "panel/api/inbounds/clientIps/" },
        { 10, "panel/api/inbounds/clearClientIps/" },
        { 11, "panel/api/inbounds/addClient" },
        { 12, "panel/api/inbounds/updateClient/" },
        { 13, "panel/api/inbounds/resetClientTraffic/" },
        { 14, "panel/api/inbounds/resetAllTraffics" },
        { 15, "panel/api/inbounds/resetAllClientTraffics/" },
        { 16, "panel/api/inbounds/delDepletedClients/" },
        { 17, "panel/api/inbounds/onlines" }
    };
    private const string Username = "StrStark";
    private const string Password = "Mr5568###";
    private const string LoginPath = "login";
    private const string PanelBaseUrl = "https://goldiran.info:20300/strstark/";
    private readonly bool LogedInSuccessfully;
    private readonly HttpClient Server;
    public PanelControler()
    {
        Server = new HttpClient();
        LogedInSuccessfully = Login(Username, Password).Result;
    }
    private async Task<bool> Login(string username, string password)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, PanelBaseUrl + LoginPath);
        var collection = new List<KeyValuePair<string, string>>
        {
            new("username", username),
            new("password", password)
        };
        request.Content = new FormUrlEncodedContent(collection);
        var response = await Server.SendAsync(request);
        response.EnsureSuccessStatusCode();
        Console.WriteLine(await response.Content.ReadAsStringAsync());
        return true;
    }
    public async Task<List<Client?>> GetAllUsersClients(Admins Admin)
    {
        await Login(Username, Password);
        var request = new HttpRequestMessage(HttpMethod.Get, PanelBaseUrl + ActionPathes.First(x => x.Key == 2).Value + $"{Admin.InboundId}");
        request.Headers.Add("Accept", "application/json");
        var response = await Server.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var responsdto = await response.Content.ReadAsAsync<InboundDto>();
        return JsonConvert.DeserializeObject<Setting>(responsdto.obj.settings)?.clients!;
    }
    private string GeneraetSetting(string emailRemark, int TotalGb, int Month)
    {
        return "{\"clients\":[{\"id\":\"" + Guid.NewGuid().ToString() + "\",\"alterId\":0,\"email\":\"" + emailRemark + "\",\"limitIp\":2,\"totalGB\":" + TotalGb * Gb + ",\"expiryTime\":" + ((Month != 0) ? new DateTimeOffset(DateTime.UtcNow.AddMonths(Month)).AddDays(-1).ToUnixTimeMilliseconds() : 0 )+ ",\"enable\":true,\"tgId\":\"\",\"subId\":\"\"}]}";
    }
    /*
    private string GeneratestreamSettings()
    public async Task<int> AddNewINbound()
    {
        
    }*/
    public async Task<bool> AddClientToAdmin(Admins Admin, string emailRemark, int TotalGb, int Month)
    {
        await Login(Username, Password);
        var body = new { id = Admin.InboundId, settings = GeneraetSetting(emailRemark, TotalGb, Month) };
        JsonContent content = JsonContent.Create(body);
        var response = await Server.PostAsync(PanelBaseUrl + ActionPathes.First(x => x.Key == 11).Value , content);
        if (!response.IsSuccessStatusCode)
            return false;
        var resualt  = JsonConvert.DeserializeObject<InboundDto>(await response.Content.ReadAsStringAsync());
        if (resualt!.msg != "Client(s) added Successfully")
        {
            return false;
        }
        return true;
    }
    public async Task<bool> RemoveClientFromAdmin(Admins Admin , string id)
    {
        await Login(Username, Password);
        var request = new HttpRequestMessage(HttpMethod.Post, PanelBaseUrl + ActionPathes.First(x => x.Key == 0).Value + $"{Admin.InboundId}/delClient/" + id);
        request.Headers.Add("Accept", "application/json");
        var response = await Server.SendAsync(request);
        if(!response.IsSuccessStatusCode)
            return false;
        var resualt = JsonConvert.DeserializeObject<InboundDto>(await response.Content.ReadAsStringAsync());
        if (resualt!.msg != "Client deleted Successfully")
        {
            return false;
        }
        return true;
    }
    public async Task<bool> ResetClientsStat(Admins Admin , string email)
    {
        await Login(Username, Password);
        var request = new HttpRequestMessage(HttpMethod.Post, PanelBaseUrl + ActionPathes.First(x => x.Key == 0).Value + $"{Admin.InboundId}/resetClientTraffic/" + email);
        request.Headers.Add("Accept", "application/json");
        var response = await Server.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            return false;
        var resualt = JsonConvert.DeserializeObject<InboundDto>(await response.Content.ReadAsStringAsync());
        if (resualt!.msg != "Traffic has been reset Successfully")
        {
            return false;
        }
        return true;

    }
    public async Task<string> GetClientUrl(Client Cl)
    {
        Code C = new Code
        {
            v = "2",
            ps = $"StrStark-{Cl.email}",
            add = "goldiran.info",
            port = 20303,
            id = Cl?.id!,
            scy = "auto",
            net = "tcp",
            type = "none",
            tls = "tls"
        };
        return "vmess://" + Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(C)));
    
    }
}