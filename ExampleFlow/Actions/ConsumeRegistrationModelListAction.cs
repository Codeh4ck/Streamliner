using System;
using System.Collections.Generic;
using System.Threading;
using ExampleFlow.Models;
using Streamliner.Actions;

namespace ExampleFlow.Actions
{
    public class ConsumeRegistrationModelListAction : ConsumerBlockActionBase<List<UserRegistrationModel>>
    {
        public override void Consume(List<UserRegistrationModel> model, CancellationToken token = default(CancellationToken))
        {
            if (model.Count == 0) return;

            foreach (UserRegistrationModel registrationModel in model)
            {
                Console.WriteLine("----------------------------------");

                Console.WriteLine($"Request ID: {registrationModel.RequestId}");
                Console.WriteLine($"Username: {registrationModel.Username}");
                Console.WriteLine($"Password: {registrationModel.Password}");
                Console.WriteLine($"E-mail: {registrationModel.Email}");
                Console.WriteLine($"Country: {registrationModel.Country}");
                Console.WriteLine($"Timezone: {registrationModel.Timezone}");

                Console.WriteLine("----------------------------------");
            }
        }
    }
}
