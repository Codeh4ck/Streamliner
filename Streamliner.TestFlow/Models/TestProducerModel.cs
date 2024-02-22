using Streamliner.Actions;

namespace Streamliner.TestFlow.Models;

public class TestProducerModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public TestProducerModel(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}