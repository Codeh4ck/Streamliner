using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using ExampleFlow.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Streamliner.Actions;

namespace ExampleFlow.Actions
{
    public class TransformRegistrationRequestToModelAction : TransformerBlockActionBase<UserRegistrationRequest, UserRegistrationModel>
    {
        public override bool TryTransform(UserRegistrationRequest input, out UserRegistrationModel model,
            CancellationToken token = default(CancellationToken))
        {
            UserRegistrationModel registrationModel = new UserRegistrationModel()
            {
                RequestId = input.RequestId,
                Username = input.Username,
                Password = HashPassword(input.Password),
                Email = input.Email
            };

            try
            {
                string jsonGeolocationData =
                    new WebClient().DownloadString($"https://freegeoip.app/json/{input.IpAddress}");

                JObject jsonObject = (JObject) JsonConvert.DeserializeObject(jsonGeolocationData);

                string country = jsonObject["country_name"].Value<string>();
                string timezone = jsonObject["time_zone"].Value<string>();

                registrationModel.Country = country;
                registrationModel.Timezone = timezone;

                model = registrationModel;

                // If no exception was thrown, return true to let the engine know that it should pass this model to the next block

                return true;
            }
            catch
            {
                // An exception was thrown, let the engine know to skip the model

                model = null;
                return false;
            }
        }

        private string HashPassword(string password)
        {
            using MD5 md5 = MD5.Create();

            byte[] inputBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();

            foreach (var t in hashBytes)
                sb.Append(t.ToString("X2"));
                
            return sb.ToString();
        }
    }
}
