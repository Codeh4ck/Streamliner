using Streamliner.Actions;
using Streamliner.TestFlow.Models;

namespace Streamliner.TestFlow.Actions;

public class TestProducerAction : ProducerBlockActionBase<TestProducerModel>
{
    public override async Task<BlockActionResult<TestProducerModel>> TryProduce(CancellationToken token = default)
    {
        await Task.Delay(3000, token);

        var result = new TestProducerModel(Guid.NewGuid(), "Test");
        
        return BlockActionResult.Success(result);
    }
}