namespace WSO.Interfaces
{
    public interface IWsoService
    {
        void Initialize();
        void ConnectAirSupply();
        void SendCustomCommand(string category, string action, string value = "");
    }
}