using System;
using Streamliner.Core.Utilities;

namespace Streamliner.Actions;

public class BlockActionIocFactory : IBlockActionFactory
{
    private readonly DependencyRegistrar _registrar;

    public BlockActionIocFactory(DependencyRegistrar registrar) => 
        _registrar = registrar ?? throw new ArgumentNullException(nameof(registrar));

    public ProducerBlockActionBase<T> CreateProducerAction<T>(Type type) => 
        (ProducerBlockActionBase<T>) _registrar.Resolve(type);

    public TransformerBlockActionBase<TIn, TOut> CreateTransformerAction<TIn, TOut>(Type type) => 
        (TransformerBlockActionBase<TIn, TOut>) _registrar.Resolve(type);

    public ConsumerBlockActionBase<TOut> CreateConsumerAction<TOut>(Type type) => 
        (ConsumerBlockActionBase<TOut>) _registrar.Resolve(type);
}