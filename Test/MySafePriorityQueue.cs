namespace Test;

public class MySafePriorityQueue<T, K> where K : IComparable<K>
{
    private SortedList<K, T> _list = new SortedList<K, T>();
    private int _size;
    private readonly object _locker = new();

    public void Enqueue(T value, K priority)
    {
        lock (_locker)
        {
            _list.Add(priority, value);
            _size++;
            Monitor.Pulse(_locker);
        }
    }

    public T Dequeue()
    {
        lock (_locker)
        {
            if (_list.Count == 0)
            {
                Monitor.Wait(_locker);
            }

            var el = _list.Values.Last();
            _list.RemoveAt(_list.Count - 1);
            _size--;
            return el;
        }
    }

    public int Size => _size;
}