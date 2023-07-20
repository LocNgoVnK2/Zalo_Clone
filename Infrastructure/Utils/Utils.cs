namespace Infrastructure.Utils
{
    public interface IUtils
    {
        string GenerateRandomString(int length);
     
    }
    public class Utils : IUtils
    {
        public string GenerateRandomString(int length)
        {
               
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var id = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return id;
        
        }
    }
}