using System.Text;
using Infrastructure.Service;
using Infrastructure.Entities;

namespace Infrastructure.Utils
{
    public interface IUtils
    {
        string GenerateRandomString(int length);
        string EncodeInformation(params string[] infos);
        string[] DecodeInformation(string code);
        ValidationByEmail TokenToValidationByEmailEntity(string token);
        string ValidationByEmailEntityToToken(ValidationByEmail entity);
        List<int> GetRandomNumbers(int min, int max, int count);
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

        public ValidationByEmail TokenToValidationByEmailEntity(string token)
        {
            var information = DecodeInformation(token!);
            return new ValidationByEmail()
            {
            return new ValidationByEmail()
            {
                Email = information[0],
                ValidationCode = information[1],
                ValidationType = int.Parse(information[2])

            };
        }

        public string ValidationByEmailEntityToToken(ValidationByEmail entity)
        {
            return EncodeInformation(entity.Email, entity.ValidationCode, entity.ValidationType.ToString());
        }
        public List<int> GetRandomNumbers(int min, int max, int count)
        {
            if (count > max - min + 1)
            {
                throw new Exception($"Không thể sinh ngẫu nhiên {count} số trong khoảng từ {min} đến {max}");
            }

            var result = new List<int>();
            var random = new Random();
            while (result.Count < count)
            {
                int randomNumber = random.Next(min, max + 1);
                if (!result.Contains(randomNumber))
                {
                    result.Add(randomNumber);
                }
            }

            return result;
        }
        public static async Task WaitUntil(Func<bool> condition, int frequency = 25, int timeout = -1)
        {
            var waitTask = Task.Run(async () =>
            {
                while (!condition()) await Task.Delay(frequency);
            });

            if (waitTask != await Task.WhenAny(waitTask,
                    Task.Delay(timeout)))
                throw new TimeoutException();
        }
    }
}