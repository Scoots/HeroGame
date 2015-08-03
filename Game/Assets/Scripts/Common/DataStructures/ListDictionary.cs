using System;
using System.Collections.Generic; 

public class ListDictionary<TKey, TValue> : IDictionary<TKey, TValue>
{
    private readonly List<TKey> _keys = new List<TKey>();

    private readonly List<TValue> _values = new List<TValue>(); 

    public void Add(TKey key, TValue value)
    {
        for (int i = 0, count = _keys.Count; i < count; i++)
        {
            if (_keys[i].Equals(key))
            {
                _values[i] = value; 
                return; 
            }
        }

        _keys.Add(key); 
        _values.Add(value);
    }

    public bool ContainsKey(TKey key)
    {
        for (int i = 0, count = _keys.Count; i < count; i++)
        {
            if (_keys[i].Equals(key))
            {
                return true; 
            }
        }

        return false; 
    }
    
    public bool ContainsValue(TValue value)
    {
        for (int i = 0, count = _values.Count; i < count; ++i)
        {
            if (_values[i].Equals(value))
            {
                return true; 
            }
        }

        return false; 
    }

    ICollection<TKey> IDictionary<TKey, TValue>.Keys
    {
        get { return _keys; }
    }

    public List<TKey> Keys
    {
        get { return _keys; }
    }

    public bool Remove(TKey key)
    {
        for (int i = 0, count = _keys.Count; i < count; i++)
        {
            if (_keys[i].Equals(key))
            {
                _keys.RemoveAt(i);
                _values.RemoveAt(i); 
                return true; 
            }
        }

        return false; 
    }

    public bool RemoveValue(TValue value)
    {
        for (int i = 0, count = _values.Count; i < count; i++)
        {
            if (_values[i].Equals(value))
            {
                _keys.RemoveAt(i);
                _values.RemoveAt(i); 

                return true; 
            }
        }

        return false; 
    }
    
    public bool TryGetValue(TKey key, out TValue value)
    {
        for (int i = 0, count = _keys.Count; i < count; i++ )
        {
            if (_keys[i].Equals(key))
            {
                value = _values[i];
                return true;
            }
        }

        value = default(TValue);
        return false; 
    }

    ICollection<TValue> IDictionary<TKey, TValue>.Values
    {
        get { return _values; }
    }

    public List<TValue> Values
    {
        get { return _values;  }
    }

    public TValue this[TKey key]
    {
        get
        {
            TValue value; 
            if (TryGetValue(key, out value))
            {
                return value; 
            }

            throw new KeyNotFoundException(); 
        }

        set 
        { 
            Add(key, value);
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        for (int i = 0, count = _keys.Count; i < count; i++)
        {
            yield return new KeyValuePair<TKey, TValue>(_keys[i], _values[i]); 
        }
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value); 
    }

    public void Clear()
    {
        _keys.Clear();
        _values.Clear(); 
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        for (int i = 0, count = _keys.Count; i < count; i++)
        {
            if (_keys[i].Equals(item.Key) && _values[i].Equals(item.Value))
            {
                return true; 
            }   
        }

        return false; 
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        for (int i = 0, count = _keys.Count; i < count; i++)
        {
            array[arrayIndex + i] = new KeyValuePair<TKey, TValue>(_keys[i], _values[i]); 
        }
    }

    public int Count
    {
        get { return _keys.Count; }
    }

    public bool IsReadOnly
    {
        get { return false; }
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        for (int i = 0, count = _keys.Count; i < count; i++)
        {
            if (_keys[i].Equals(item.Key) && _values[i].Equals(item.Value))
            {
                _keys.RemoveAt(i);
                _values.RemoveAt(i);
                return true; 
            }
        }

        return false; 
    }

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
    {
        for (int i = 0, count = _keys.Count; i < count; i++)
        {
            yield return new KeyValuePair<TKey, TValue>(_keys[i], _values[i]); 
        }
    }
}
