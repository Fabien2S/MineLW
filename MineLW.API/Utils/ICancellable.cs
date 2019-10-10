namespace MineLW.API.Utils
{
    public interface ICancellable
    {
        bool Canceled { get; set; }
    }
}