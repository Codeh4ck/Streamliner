using System;
using System.Text;
using System.Threading;
using ExampleFlow.Models;
using Streamliner.Actions;

namespace ExampleFlow.Actions
{
    public class ProduceUserRegistrationRequestAction : ProducerBlockActionBase<UserRegistrationRequest>
    {
        public override bool TryProduce(out UserRegistrationRequest model, CancellationToken token = default(CancellationToken))
        {
            // We instantiate a new UserRegistrationRequest here
            // In a production environment, this would be received from a queue, a web request or any other data source

            UserRegistrationRequest request = new UserRegistrationRequest()
            {
                RequestId = Guid.NewGuid(),
                Email = $"{GenerateRandomString(8)}@domain.com",
                Username = $"SampleUser_{GenerateRandomString(5)}",
                Password = GenerateRandomString(),
                IpAddress = "8.8.8.8"
            };

            model = request;

            return true;
        }

        // Default length = 0 - will be translated as random length unless specified
        private string GenerateRandomString(int length = 0)
        {
            Random random = new Random(DateTime.Now.Millisecond);

            string charPool = "abcdefghijklmnopqrstuvwxyz";
            charPool += charPool.ToUpper();
            charPool += "01234567890";

            if (length == 0)
                length = random.Next(5, 15);

            StringBuilder builder = new StringBuilder(length);

            for (int x = 0; x < length; x++)
                builder.Append(charPool[random.Next(0, charPool.Length - 1)]);

            return builder.ToString();
        }
    }
}
