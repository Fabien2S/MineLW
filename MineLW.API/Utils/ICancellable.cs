namespace MineLW.API.Utils
{
    public interface ICancellable
    {
        bool Cancelled { get; set; }
    }
}