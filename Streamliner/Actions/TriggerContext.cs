using System;

namespace Streamliner.Actions
{
    public class TriggerContext<T>
    {
        public Guid? Id { get; set; }
        public T Item { get; set; }
    }
}
