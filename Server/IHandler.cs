namespace Server
{
    public interface IHandler
    {
        ServerConnection connection { get; set; }

        void Start();
    }
}