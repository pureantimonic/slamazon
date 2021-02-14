// Wraps list so I can serialize it

using System.Collections.Generic;

[System.Serializable]
public class ListWrapper<T>
{
    public List<T> innerList;
    public T this[int key]
    {
        get
        {
            return innerList[key];
        }
        set
        {
            innerList[key] = value;
        }
    }
}