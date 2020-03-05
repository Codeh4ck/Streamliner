using System;

namespace Streamliner.Actions
{
    public interface IBlockActionFactory
    {
        ProducerBlockActionBase<T> CreateProducerAction<T>(Type type);
        TransformerBlockActionBase<TIn, TOut> CreateTransformerAction<TIn, TOut>(Type type);
        ConsumerBlockActionBase<TOut> CreateConsumerAction<TOut>(Type type);
    }
}
