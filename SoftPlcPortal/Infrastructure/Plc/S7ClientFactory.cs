using Sharp7;

namespace SoftPlcPortal.Infrastructure.Plc;

public class S7ClientFactory
{
    public S7Client Create(string address, int port)
    {
        var s7client = new S7Client() { PLCPort = port };
        s7client.ConnectTo(address, 0, 1);

        return s7client;
    }
}
