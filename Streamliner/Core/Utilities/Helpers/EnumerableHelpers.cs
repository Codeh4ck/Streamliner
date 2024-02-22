using System;
using System.Collections.Generic;
using System.Linq;

namespace Streamliner.Core.Utilities.Helpers
{
    public static class EnumerableHelpers
    {
        public static IEnumerable<TSource[]> Chunk<TSource>(this IEnumerable<TSource> source, int size)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (size < 1) throw new ArgumentOutOfRangeException(nameof(size));
            
            if (source is TSource[] sources && sources?.Length == 0)
            {
                return Array.Empty<TSource[]>();
            }

            return ChunkIterator(source, size);
        }

        private static IEnumerable<TSource[]> ChunkIterator<TSource>(IEnumerable<TSource> source, int size)
        {
            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                if (!e.MoveNext()) yield break;
                
                int arraySize = Math.Min(size, 4);
                int i;
                do
                {
                    var array = new TSource[arraySize];

                    array[0] = e.Current;
                    i = 1;

                    if (size != array.Length)
                    {
                        for (; i < size && e.MoveNext(); i++)
                        {
                            if (i >= array.Length)
                            {
                                arraySize = (int)Math.Min((uint)size, 2 * (uint)array.Length);
                                Array.Resize(ref array, arraySize);
                            }

                            array[i] = e.Current;
                        }
                    }
                    else
                    {
                        TSource[] local = array; 
                        for (; (uint)i < (uint)local.Length && e.MoveNext(); i++)
                        {
                            local[i] = e.Current;
                        }
                    }

                    if (i != array.Length)
                    {
                        Array.Resize(ref array, i);
                    }

                    yield return array;
                } 
                while (i >= size && e.MoveNext());
            }
        }
    }
}