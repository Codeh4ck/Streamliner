using Streamliner.Actions;
using Streamliner.TestFlow.Models;

namespace Streamliner.TestFlow.Actions;

public class TestConsumerAction : ConsumerBlockActionBase<List<TestTransformerModel>>
{
    public override async Task Consume(List<TestTransformerModel> model, CancellationToken token = default)
    {
        await Task.Delay(3000, token);
        
        foreach(var item in model)
            Console.WriteLine($"Consumer: {item.Id} - {item.Name} - {item.Age} - {item.Height} - Partition Count: {model.Count}");
    }
}