using System;
using UnityEngine;

namespace Runtime
{
    [Serializable]
    public class SerializedTuple<T1, T2>
    {
        [field:SerializeField] public T1 V1 { get; private set; }
        [field:SerializeField] public T2 V2 { get; private set; }

        public SerializedTuple(T1 v1, T2 v2)
        {
            V1 = v1;
            V2 = v2;
        }
    }
}