namespace AdventOfCode.Common.Extensions;

public static class SpanExtensions
{
	public static SpanByteLineEnumerator EnumerateLines(this ReadOnlySpan<byte> span) => new(span);
	public static SpanByteLineLengthEnumerator EnumerateLines(this ReadOnlySpan<byte> span, int length) => new(span, length);
}

/// <summary>
/// Enumerates the lines of a <see cref="ReadOnlySpan{Char}"/>.
/// </summary>
/// <remarks>
/// To get an instance of this type, use <see cref="MemoryExtensions.EnumerateLines(ReadOnlySpan{char})"/>.
/// </remarks>
public ref struct SpanByteLineEnumerator
{
	private ReadOnlySpan<byte> _remaining;
	private bool _isEnumeratorActive;

	internal SpanByteLineEnumerator(ReadOnlySpan<byte> buffer)
	{
		_remaining = buffer;
		Current = default;
		_isEnumeratorActive = true;
	}

	/// <summary>
	/// Gets the line at the current position of the enumerator.
	/// </summary>
	public ReadOnlySpan<byte> Current { get; private set; }

	/// <summary>
	/// Returns this instance as an enumerator.
	/// </summary>
	public readonly SpanByteLineEnumerator GetEnumerator() => this;

	/// <summary>
	/// Advances the enumerator to the next line of the span.
	/// </summary>
	/// <returns>
	/// True if the enumerator successfully advanced to the next line; false if
	/// the enumerator has advanced past the end of the span.
	/// </returns>
	public bool MoveNext()
	{
		if (!_isEnumeratorActive)
		{
			return false; // EOF previously reached or enumerator was never initialized
		}

		var remaining = _remaining;

		var idx = remaining.IndexOf("\n"u8);

		if ((uint)idx < (uint)remaining.Length)
		{
			Current = remaining[..idx];
			_remaining = remaining[(idx + 1)..];
		}
		else
		{
			// We've reached EOF, but we still need to return 'true' for this final
			// iteration so that the caller can query the Current property once more.

			Current = remaining;
			_remaining = default;
			_isEnumeratorActive = false;
		}

		return true;
	}
}

/// <summary>
/// Enumerates the lines of a <see cref="ReadOnlySpan{Char}"/>.
/// </summary>
/// <remarks>
/// To get an instance of this type, use <see cref="MemoryExtensions.EnumerateLines(ReadOnlySpan{char})"/>.
/// </remarks>
public ref struct SpanByteLineLengthEnumerator
{
	private readonly int _length;
	private ReadOnlySpan<byte> _remaining;
	private bool _isEnumeratorActive;

	internal SpanByteLineLengthEnumerator(ReadOnlySpan<byte> buffer, int length)
	{
		_length = length;
		_remaining = buffer;
		Current = default;
		_isEnumeratorActive = true;
	}

	/// <summary>
	/// Gets the line at the current position of the enumerator.
	/// </summary>
	public ReadOnlySpan<byte> Current { get; private set; }

	/// <summary>
	/// Returns this instance as an enumerator.
	/// </summary>
	public readonly SpanByteLineLengthEnumerator GetEnumerator() => this;

	/// <summary>
	/// Advances the enumerator to the next line of the span.
	/// </summary>
	/// <returns>
	/// True if the enumerator successfully advanced to the next line; false if
	/// the enumerator has advanced past the end of the span.
	/// </returns>
	public bool MoveNext()
	{
		if (!_isEnumeratorActive)
		{
			return false; // EOF previously reached or enumerator was never initialized
		}

		var remaining = _remaining;

		if ((uint)_length < (uint)remaining.Length)
		{
			Current = remaining[.._length];
			_remaining = remaining[(_length + 1)..];
		}
		else
		{
			// We've reached EOF, but we still need to return 'true' for this final
			// iteration so that the caller can query the Current property once more.

			Current = remaining;
			_remaining = default;
			_isEnumeratorActive = false;
		}

		return true;
	}
}
