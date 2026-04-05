using System.Collections;
using System.Collections.ObjectModel;

namespace G2D;

/// <summary>
///     Represents a group of disposable resources that are disposed together.
/// </summary>
public sealed class CompositeDisposable : IDisposable, ICollection<IDisposable>
{
    private readonly object _gate = new();
    private readonly ReadOnlyCollection<IDisposable> _readOnlyDisposables;
    private readonly List<IDisposable> _disposables;
    private bool _disposed;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CompositeDisposable" /> class.
    /// </summary>
    public CompositeDisposable()
    {
        _disposables = new List<IDisposable>();
        _readOnlyDisposables = new ReadOnlyCollection<IDisposable>(_disposables);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="CompositeDisposable" /> class with the specified number of disposables.
    /// </summary>
    /// <param name="capacity">The initial number of disposables the collection can hold.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity" /> is less than zero.</exception>
    public CompositeDisposable(int capacity)
    {
        if (capacity < 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity cannot be less than zero.");

        _disposables = new List<IDisposable>(capacity);
        _readOnlyDisposables = new ReadOnlyCollection<IDisposable>(_disposables);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="CompositeDisposable" /> class with the specified disposables.
    /// </summary>
    /// <param name="disposables">The disposables to add to the collection.</param>
    /// <exception cref="ArgumentNullException"><paramref name="disposables" /> is null.</exception>
    public CompositeDisposable(params IDisposable[] disposables)
    {
        ArgumentNullException.ThrowIfNull(disposables);

        _disposables = new List<IDisposable>(disposables);
        _readOnlyDisposables = new ReadOnlyCollection<IDisposable>(_disposables);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="CompositeDisposable" /> class with the specified disposables.
    /// </summary>
    /// <param name="disposables">The disposables to add to the collection.</param>
    /// <exception cref="ArgumentNullException"><paramref name="disposables" /> is null.</exception>
    public CompositeDisposable(IEnumerable<IDisposable> disposables)
    {
        ArgumentNullException.ThrowIfNull(disposables);

        _disposables = new List<IDisposable>(disposables);
        _readOnlyDisposables = new ReadOnlyCollection<IDisposable>(_disposables);
    }

    /// <summary>
    ///     Gets an enumerator that iterates through the collection.
    /// </summary>
    public ReadOnlyCollection<IDisposable> Disposables
    {
        get
        {
            lock (_gate)
            {
                return _readOnlyDisposables;
            }
        }
    }

    /// <summary>
    ///     Gets the number of disposables in the collection.
    /// </summary>
    public int Count
    {
        get
        {
            lock (_gate)
            {
                return _disposables.Count;
            }
        }
    }

    /// <summary>
    ///     Gets a value indicating whether the collection is read-only.
    /// </summary>
    public bool IsReadOnly => false;

    /// <summary>
    ///     Adds a disposable to the collection.
    /// </summary>
    /// <param name="item">The disposable to add.</param>
    /// <exception cref="ArgumentNullException"><paramref name="item" /> is null.</exception>
    public void Add(IDisposable item)
    {
        ArgumentNullException.ThrowIfNull(item);

        lock (_gate)
        {
            if (_disposed)
            {
                item.Dispose();
                return;
            }

            _disposables.Add(item);
        }
    }

    /// <summary>
    ///     Removes and disposes the first occurrence of a disposable from the collection.
    /// </summary>
    /// <param name="item">The disposable to remove.</param>
    /// <returns>true if the disposable was successfully removed; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="item" /> is null.</exception>
    public bool Remove(IDisposable item)
    {
        ArgumentNullException.ThrowIfNull(item);

        lock (_gate)
        {
            if (_disposed)
                return false;

            var success = _disposables.Remove(item);
            if (success) item.Dispose();
            return success;
        }
    }

    /// <summary>
    ///     Removes all disposables from the collection without disposing them.
    /// </summary>
    public void Clear()
    {
        IDisposable[] currentDisposables;
        lock (_gate)
        {
            currentDisposables = _disposables.ToArray();
            _disposables.Clear();
        }

        foreach (var disposable in currentDisposables) disposable?.Dispose();
    }

    /// <summary>
    ///     Determines whether the collection contains a specific disposable.
    /// </summary>
    /// <param name="item">The disposable to locate.</param>
    /// <returns>true if the disposable is found; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="item" /> is null.</exception>
    public bool Contains(IDisposable item)
    {
        ArgumentNullException.ThrowIfNull(item);

        lock (_gate)
        {
            return _disposables.Contains(item);
        }
    }

    /// <summary>
    ///     Copies the disposables to an array, starting at a particular array index.
    /// </summary>
    /// <param name="array">The array to copy to.</param>
    /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
    /// <exception cref="ArgumentNullException"><paramref name="array" /> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex" /> is less than zero.</exception>
    /// <exception cref="ArgumentException">The number of elements to copy exceeds the available space.</exception>
    public void CopyTo(IDisposable[] array, int arrayIndex)
    {
        ArgumentNullException.ThrowIfNull(array);
        if (arrayIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Array index cannot be less than zero.");

        lock (_gate)
        {
            if (array.Length - arrayIndex < _disposables.Count)
                throw new ArgumentException("The number of elements to copy exceeds the available space in the array.");

            _disposables.CopyTo(array, arrayIndex);
        }
    }

    /// <summary>
    ///     Returns an enumerator that iterates through the collection.
    /// </summary>
    public IEnumerator<IDisposable> GetEnumerator()
    {
        lock (_gate)
        {
            return new List<IDisposable>(_disposables).GetEnumerator();
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    ///     Disposes all disposables in the collection and removes them.
    /// </summary>
    public void Dispose()
    {
        IDisposable[] currentDisposables;
        lock (_gate)
        {
            if (_disposed)
                return;

            _disposed = true;
            currentDisposables = _disposables.ToArray();
            _disposables.Clear();
        }

        foreach (var disposable in currentDisposables) disposable?.Dispose();
    }
}