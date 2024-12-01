using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Enemy
{
    [Serializable]
    public class ListOfList<T> 
    {
        [field:SerializeField] public List<T> List { get; private set; }
    }
}