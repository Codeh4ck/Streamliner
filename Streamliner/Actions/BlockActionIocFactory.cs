using System;
using Streamliner.Core.Utilities;

namespace Streamliner.Actions
{
    public class BlockActionIocFactory : IBlockActionFactory
    {
        private readonly DependencyRegistrar _registrar;

        public BlockActionIocFactory(DependencyRegistrar registrar)
        {
            if (registrar == null) throw new ArgumentNullException(nameof(registrar));
            _registrar = registrar;
        }

        public ProducerBlockActionBase<T> CreateProducerAction<T>(Type type)
        {
            return (ProducerBlockActionBase<T>) _registrar.Resolve(type);
        }

        public TransformerBlockActionBase<TIn, TOut> CreateTransformerAction<TIn, TOut>(Type type)
        {
            return (TransformerBlockActionBase<TIn, TOut>) _registrar.Resolve(type);
        }

        public ConsumerBlockActionBase<TOut> CreateConsumerAction<TOut>(Type type)
        {
            return (ConsumerBlockActionBase<TOut>) _registrar.Resolve(type);
        }
    }
}
