#nullable enable

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// copied from https://github.com/dotnet/runtime/blob/main/src/libraries/System.Collections/src/System/Collections/Generic/PriorityQueue.cs
// and further edited

using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AdventOfCode;

/// <summary>
///  Represents a min priority queue.
/// </summary>
/// <typeparam name="TElement">Specifies the type of elements in the queue.</typeparam>
/// <remarks>
///  Implements an array-backed quaternary min-heap. Each element is enqueued with an associated priority
///  that determines the dequeue order: elements with the lowest priority get dequeued first.
/// </remarks>
[DebuggerDisplay("Count = {Count}")]
public class UpdateablePriorityQueue<TElement>
	where TElement : notnull
{
	/// <summary>
	/// Represents an implicit heap-ordered complete d-ary tree, stored as an array.
	/// </summary>
	private (TElement Element, int Priority)[] _nodes;

	/// <summary>
	/// Identifies the location of an element
	/// </summary>
	private readonly Dictionary<TElement, int> _elementIndex;

	/// <summary>
	/// Specifies the arity of the d-ary heap, which here is quaternary.
	/// It is assumed that this value is a power of 2.
	/// </summary>
	private const int Arity = 4;

	/// <summary>
	/// The binary logarithm of <see cref="Arity" />.
	/// </summary>
	private const int Log2Arity = 2;

#if DEBUG
	static UpdateablePriorityQueue()
	{
		Debug.Assert(Log2Arity > 0 && Math.Pow(2, Log2Arity) == Arity);
	}
#endif

	/// <summary>
	///  Initializes a new instance of the <see cref="PriorityQueue{TElement, int}"/> class.
	/// </summary>
	public UpdateablePriorityQueue()
	{
		_nodes = Array.Empty<(TElement, int)>();
		_elementIndex = new();
	}

	/// <summary>
	///  Initializes a new instance of the <see cref="PriorityQueue{TElement, int}"/> class
	///  with the specified initial capacity.
	/// </summary>
	/// <param name="initialCapacity">Initial capacity to allocate in the underlying heap array.</param>
	/// <exception cref="ArgumentOutOfRangeException">
	///  The specified <paramref name="initialCapacity"/> was negative.
	/// </exception>
	public UpdateablePriorityQueue(int initialCapacity)
	{
		if (initialCapacity < 0)
		{
			throw new ArgumentOutOfRangeException(
				nameof(initialCapacity));
		}

		_nodes = new (TElement, int)[initialCapacity];
		_elementIndex = new(initialCapacity);
	}

	/// <summary>
	///  Initializes a new instance of the <see cref="PriorityQueue{TElement, int}"/> class
	///  that is populated with the specified elements and priorities.
	/// </summary>
	/// <param name="items">The pairs of elements and priorities with which to populate the queue.</param>
	/// <exception cref="ArgumentNullException">
	///  The specified <paramref name="items"/> argument was <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///  Constructs the heap using a heapify operation,
	///  which is generally faster than enqueuing individual elements sequentially.
	/// </remarks>
	public UpdateablePriorityQueue(IEnumerable<(TElement Element, int Priority)> items)
	{
		if (items is null)
		{
			throw new ArgumentNullException(nameof(items));
		}

		_nodes = items.ToArray();
		_elementIndex = new(_nodes.Length);

		if (Count > 1)
		{
			Heapify();
		}
	}

	/// <summary>
	///  Gets the number of elements contained in the <see cref="PriorityQueue{TElement, int}"/>.
	/// </summary>
	public int Count { get; private set; }

	/// <summary>
	///  Adds the specified element with associated priority to the <see cref="PriorityQueue{TElement, int}"/>.
	/// </summary>
	/// <param name="element">The element to add to the <see cref="PriorityQueue{TElement, int}"/>.</param>
	/// <param name="priority">The priority with which to associate the new element.</param>
	public void Enqueue(TElement element, int priority)
	{
		if (_elementIndex.ContainsKey(element))
		{
			var index = _elementIndex[element];
			if (priority >= _nodes[index].Priority)
				return;

			MoveUpDefaultComparer((element, priority), index);
		}
		else
		{
			// Virtually add the node at the end of the underlying array.
			// Note that the node being enqueued does not need to be physically placed
			// there at this point, as such an assignment would be redundant.

			int currentSize = Count++;

			if (_nodes.Length == currentSize)
			{
				Grow(currentSize + 1);
			}

			MoveUpDefaultComparer((element, priority), currentSize);
		}
	}

	/// <summary>
	///  Returns the minimal element from the <see cref="PriorityQueue{TElement, int}"/> without removing it.
	/// </summary>
	/// <exception cref="InvalidOperationException">The <see cref="PriorityQueue{TElement, int}"/> is empty.</exception>
	/// <returns>The minimal element of the <see cref="PriorityQueue{TElement, int}"/>.</returns>
	public TElement Peek()
	{
		if (Count == 0)
		{
			throw new InvalidOperationException();
		}

		return _nodes[0].Element;
	}

	/// <summary>
	///  Removes and returns the minimal element from the <see cref="PriorityQueue{TElement, int}"/>.
	/// </summary>
	/// <exception cref="InvalidOperationException">The queue is empty.</exception>
	/// <returns>The minimal element of the <see cref="PriorityQueue{TElement, int}"/>.</returns>
	public TElement Dequeue()
	{
		if (Count == 0)
		{
			throw new InvalidOperationException();
		}

		TElement element = _nodes[0].Element;
		_elementIndex.Remove(element);
		RemoveRootNode();
		return element;
	}

	/// <summary>
	///  Removes the minimal element from the <see cref="PriorityQueue{TElement, int}"/>,
	///  and copies it to the <paramref name="element"/> parameter,
	///  and its associated priority to the <paramref name="priority"/> parameter.
	/// </summary>
	/// <param name="element">The removed element.</param>
	/// <param name="priority">The priority associated with the removed element.</param>
	/// <returns>
	///  <see langword="true"/> if the element is successfully removed;
	///  <see langword="false"/> if the <see cref="PriorityQueue{TElement, int}"/> is empty.
	/// </returns>
	public bool TryDequeue([MaybeNullWhen(false)] out TElement element, [MaybeNullWhen(false)] out int priority)
	{
		if (Count != 0)
		{
			(element, priority) = _nodes[0];
			_elementIndex.Remove(element);
			RemoveRootNode();
			return true;
		}

		element = default;
		priority = default;
		return false;
	}

	/// <summary>
	///  Returns a value that indicates whether there is a minimal element in the <see cref="PriorityQueue{TElement, int}"/>,
	///  and if one is present, copies it to the <paramref name="element"/> parameter,
	///  and its associated priority to the <paramref name="priority"/> parameter.
	///  The element is not removed from the <see cref="PriorityQueue{TElement, int}"/>.
	/// </summary>
	/// <param name="element">The minimal element in the queue.</param>
	/// <param name="priority">The priority associated with the minimal element.</param>
	/// <returns>
	///  <see langword="true"/> if there is a minimal element;
	///  <see langword="false"/> if the <see cref="PriorityQueue{TElement, int}"/> is empty.
	/// </returns>
	public bool TryPeek([MaybeNullWhen(false)] out TElement element, [MaybeNullWhen(false)] out int priority)
	{
		if (Count != 0)
		{
			(element, priority) = _nodes[0];
			return true;
		}

		element = default;
		priority = default;
		return false;
	}

	/// <summary>
	///  Adds the specified element with associated priority to the <see cref="PriorityQueue{TElement, int}"/>,
	///  and immediately removes the minimal element, returning the result.
	/// </summary>
	/// <param name="element">The element to add to the <see cref="PriorityQueue{TElement, int}"/>.</param>
	/// <param name="priority">The priority with which to associate the new element.</param>
	/// <returns>The minimal element removed after the enqueue operation.</returns>
	/// <remarks>
	///  Implements an insert-then-extract heap operation that is generally more efficient
	///  than sequencing Enqueue and Dequeue operations: in the worst case scenario only one
	///  sift-down operation is required.
	/// </remarks>
	public TElement EnqueueDequeue(TElement element, int priority)
	{
		if (Count != 0)
		{
			(TElement Element, int Priority) = _nodes[0];
			_elementIndex.Remove(Element);

			if (priority > Priority)
			{
				MoveDownDefaultComparer((element, priority), 0);
				return Element;
			}
		}

		return element;
	}

	/// <summary>
	///  Enqueues a sequence of element/priority pairs to the <see cref="PriorityQueue{TElement, int}"/>.
	/// </summary>
	/// <param name="items">The pairs of elements and priorities to add to the queue.</param>
	/// <exception cref="ArgumentNullException">
	///  The specified <paramref name="items"/> argument was <see langword="null"/>.
	/// </exception>
	public void EnqueueRange(IEnumerable<(TElement Element, int Priority)> items)
	{
		if (items is null)
		{
			throw new ArgumentNullException(nameof(items));
		}

		int count = 0;
		var collection = items as ICollection<(TElement Element, int Priority)>;
		if (collection is not null && (count = collection.Count) > _nodes.Length - Count)
		{
			Grow(Count + count);
		}

		if (Count == 0)
		{
			// build using Heapify() if the queue is empty.

			if (collection is not null)
			{
				collection.CopyTo(_nodes, 0);
				Count = count;
			}
			else
			{
				int i = 0;
				(TElement, int)[] nodes = _nodes;
				foreach ((TElement element, int priority) in items)
				{
					if (nodes.Length == i)
					{
						Grow(i + 1);
						nodes = _nodes;
					}

					nodes[i++] = (element, priority);
				}

				Count = i;
			}

			if (Count > 1)
			{
				Heapify();
			}
		}
		else
		{
			foreach ((TElement element, int priority) in items)
			{
				Enqueue(element, priority);
			}
		}
	}

	/// <summary>
	///  Enqueues a sequence of elements pairs to the <see cref="PriorityQueue{TElement, int}"/>,
	///  all associated with the specified priority.
	/// </summary>
	/// <param name="elements">The elements to add to the queue.</param>
	/// <param name="priority">The priority to associate with the new elements.</param>
	/// <exception cref="ArgumentNullException">
	///  The specified <paramref name="elements"/> argument was <see langword="null"/>.
	/// </exception>
	public void EnqueueRange(IEnumerable<TElement> elements, int priority)
	{
		if (elements is null)
		{
			throw new ArgumentNullException(nameof(elements));
		}

		int count;
		if (elements is ICollection<(TElement Element, int Priority)> collection &&
			(count = collection.Count) > _nodes.Length - Count)
		{
			Grow(Count + count);
		}

		if (Count == 0)
		{
			// build using Heapify() if the queue is empty.

			int i = 0;
			(TElement, int)[] nodes = _nodes;
			foreach (TElement element in elements)
			{
				if (nodes.Length == i)
				{
					Grow(i + 1);
					nodes = _nodes;
				}

				nodes[i++] = (element, priority);
			}

			Count = i;

			if (i > 1)
			{
				Heapify();
			}
		}
		else
		{
			foreach (TElement element in elements)
			{
				Enqueue(element, priority);
			}
		}
	}

	/// <summary>
	///  Removes all items from the <see cref="PriorityQueue{TElement, int}"/>.
	/// </summary>
	public void Clear()
	{
		if (RuntimeHelpers.IsReferenceOrContainsReferences<(TElement, int)>())
		{
			// Clear the elements so that the gc can reclaim the references
			Array.Clear(_nodes, 0, Count);
			_elementIndex.Clear();
		}
		Count = 0;
	}

	/// <summary>
	///  Ensures that the <see cref="PriorityQueue{TElement, int}"/> can hold up to
	///  <paramref name="capacity"/> items without further expansion of its backing storage.
	/// </summary>
	/// <param name="capacity">The minimum capacity to be used.</param>
	/// <exception cref="ArgumentOutOfRangeException">
	///  The specified <paramref name="capacity"/> is negative.
	/// </exception>
	/// <returns>The current capacity of the <see cref="PriorityQueue{TElement, int}"/>.</returns>
	public int EnsureCapacity(int capacity)
	{
		if (capacity < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(capacity));
		}

		if (_nodes.Length < capacity)
		{
			Grow(capacity);
		}

		return _nodes.Length;
	}

	/// <summary>
	///  Sets the capacity to the actual number of items in the <see cref="PriorityQueue{TElement, int}"/>,
	///  if that is less than 90 percent of current capacity.
	/// </summary>
	/// <remarks>
	///  This method can be used to minimize a collection's memory overhead
	///  if no new elements will be added to the collection.
	/// </remarks>
	public void TrimExcess()
	{
		int threshold = (int)(_nodes.Length * 0.9);
		if (Count < threshold)
		{
			Array.Resize(ref _nodes, Count);
		}
	}

	/// <summary>
	/// Grows the priority queue to match the specified min capacity.
	/// </summary>
	private void Grow(int minCapacity)
	{
		Debug.Assert(_nodes.Length < minCapacity);

		const int GrowFactor = 2;
		const int MinimumGrow = 4;

		int newcapacity = GrowFactor * _nodes.Length;

		// Allow the queue to grow to maximum possible capacity (~2G elements) before encountering overflow.
		// Note that this check works even when _nodes.Length overflowed thanks to the (uint) cast
		if ((uint)newcapacity > Array.MaxLength) newcapacity = Array.MaxLength;

		// Ensure minimum growth is respected.
		newcapacity = Math.Max(newcapacity, _nodes.Length + MinimumGrow);

		// If the computed capacity is still less than specified, set to the original argument.
		// Capacities exceeding Array.MaxLength will be surfaced as OutOfMemoryException by Array.Resize.
		if (newcapacity < minCapacity) newcapacity = minCapacity;

		Array.Resize(ref _nodes, newcapacity);
	}

	/// <summary>
	/// Removes the node from the root of the heap
	/// </summary>
	private void RemoveRootNode()
	{
		int lastNodeIndex = --Count;

		if (lastNodeIndex > 0)
		{
			(TElement Element, int Priority) lastNode = _nodes[lastNodeIndex];
			MoveDownDefaultComparer(lastNode, 0);
		}

		if (RuntimeHelpers.IsReferenceOrContainsReferences<(TElement, int)>())
		{
			_nodes[lastNodeIndex] = default;
		}
	}

	/// <summary>
	/// Gets the index of an element's parent.
	/// </summary>
	private static int GetParentIndex(int index) => (index - 1) >> Log2Arity;

	/// <summary>
	/// Gets the index of the first child of an element.
	/// </summary>
	private static int GetFirstChildIndex(int index) => (index << Log2Arity) + 1;

	/// <summary>
	/// Converts an unordered list into a heap.
	/// </summary>
	private void Heapify()
	{
		for (int index = Count - 1; index >= 0; --index)
			_elementIndex[_nodes[index].Element] = index;

		// Leaves of the tree are in fact 1-element heaps, for which there
		// is no need to correct them. The heap property needs to be restored
		// only for higher nodes, starting from the first node that has children.
		// It is the parent of the very last element in the array.

		(TElement Element, int Priority)[] nodes = _nodes;
		int lastParentWithChildren = GetParentIndex(Count - 1);

		for (int index = lastParentWithChildren; index >= 0; --index)
		{
			MoveDownDefaultComparer(nodes[index], index);
		}
	}

	/// <summary>
	/// Moves a node up in the tree to restore heap order.
	/// </summary>
	private void MoveUpDefaultComparer((TElement Element, int Priority) node, int nodeIndex)
	{
		// Instead of swapping items all the way to the root, we will perform
		// a similar optimization as in the insertion sort.

		Debug.Assert(0 <= nodeIndex && nodeIndex < Count);

		(TElement Element, int Priority)[] nodes = _nodes;

		while (nodeIndex > 0)
		{
			int parentIndex = GetParentIndex(nodeIndex);
			(TElement Element, int Priority) parent = nodes[parentIndex];

			if (node.Priority < parent.Priority)
			{
				nodes[nodeIndex] = parent;
				_elementIndex[parent.Element] = nodeIndex;
				nodeIndex = parentIndex;
			}
			else
			{
				break;
			}
		}

		nodes[nodeIndex] = node;
		_elementIndex[node.Element] = nodeIndex;
	}

	/// <summary>
	/// Moves a node down in the tree to restore heap order.
	/// </summary>
	private void MoveDownDefaultComparer((TElement Element, int Priority) node, int nodeIndex)
	{
		// The node to move down will not actually be swapped every time.
		// Rather, values on the affected path will be moved up, thus leaving a free spot
		// for this value to drop in. Similar optimization as in the insertion sort.

		Debug.Assert(0 <= nodeIndex && nodeIndex < Count);

		(TElement Element, int Priority)[] nodes = _nodes;
		int size = Count;

		int i;
		while ((i = GetFirstChildIndex(nodeIndex)) < size)
		{
			// Find the child node with the minimal priority
			(TElement Element, int Priority) minChild = nodes[i];
			int minChildIndex = i;

			int childIndexUpperBound = Math.Min(i + Arity, size);
			while (++i < childIndexUpperBound)
			{
				(TElement Element, int Priority) nextChild = nodes[i];
				if (nextChild.Priority < minChild.Priority)
				{
					minChild = nextChild;
					minChildIndex = i;
				}
			}

			// Heap property is satisfied; insert node in this location.
			if (node.Priority <= minChild.Priority)
			{
				break;
			}

			// Move the minimal child up by one node and
			// continue recursively from its location.
			nodes[nodeIndex] = minChild;
			_elementIndex[minChild.Element] = nodeIndex;
			nodeIndex = minChildIndex;
		}

		nodes[nodeIndex] = node;
		_elementIndex[node.Element] = nodeIndex;
	}
}

#nullable restore

