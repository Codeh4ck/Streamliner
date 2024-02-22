using Streamliner.Actions;
using Streamliner.TestFlow.Models;

namespace Streamliner.TestFlow.Actions;

public class TestTransformerAction : TransformerBlockActionBase<TestProducerModel, List<TestTransformerModel>>
{
    public override async Task<BlockActionResult<List<TestTransformerModel>>> TryTransform(TestProducerModel input, CancellationToken token = default)
    {
        await Task.Delay(3000, token);

        var result = new List<TestTransformerModel>();

        for (int x = 0; x < 950; x++)
        {
            result.Add(new TestTransformerModel
            {
                Id = input.Id,
                Name = input.Name,
                Age = 42,
                Height = x
            });
        }
        
        return BlockActionResult.Success(result);
    }
}