using System.Collections.Generic;

namespace SimpleRegistryTransfer.Entities
{
    public class BaseCodec<T>
    {
        public string Type { get; set; }

        public List<T> Value { get; set; }
    }
}
