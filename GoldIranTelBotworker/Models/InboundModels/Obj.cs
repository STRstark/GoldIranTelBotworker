using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Obj
{
    public int id { get; set; }
    public Int64 up { get; set; }
    public Int64 down { get; set; }
    public Int64 total { get; set; }
    public string? remark { get; set; }
    public bool enable { get; set; }
    public Int64 expiryTime { get; set; }
    public object? clientStats { get; set; }
    public string? listen { get; set; }
    public int port { get; set; }
    public string? protocol { get; set; }
    public Setting? settings { get; set; }
    public StreamSetting? streamSettings { get; set; }
    public string? tag { get; set; }
    public Sniffing? sniffing { get; set; }
}
