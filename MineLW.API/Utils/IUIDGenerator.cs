namespace MineLW.API.Utils
{
    public interface IUidGenerator
    {
        /// <summary>
        /// Generate a unique id
        /// </summary>
        /// <returns>a unique id</returns>
        int GenerateUid();
    }
}