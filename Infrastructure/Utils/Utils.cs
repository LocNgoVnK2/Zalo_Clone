using System.Text;

namespace Infrastructure.Utils
{
    public interface IUtils
    {
        string GenerateRandomString(int length);
        string EncodeInformation(params string[] infos);
        string[] DecodeInformation(string code);
    }
    public class Utils : IUtils
    {
        public string[] DecodeInformation(string code)
        {
            var base64EncodedBytes = Convert.FromBase64String(code);
            return Encoding.UTF8.GetString(base64EncodedBytes).Split('|');
        }

        public string EncodeInformation(params string[] infos)
        {
            string value = "";
            for (int i = 0; i < infos.Length; i++)
            {

                value += infos[i];
                if (i != infos.Length - 1)
                    value += "|";
            }
            var valueBytes = Encoding.UTF8.GetBytes(value);

            return Convert.ToBase64String(valueBytes);
        }

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