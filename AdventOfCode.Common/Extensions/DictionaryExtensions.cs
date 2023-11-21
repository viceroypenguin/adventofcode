using System.Runtime.InteropServices;

namespace AdventOfCode.Common.Extensions;

public static class DictionaryExtensions
{
	public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> func)
		where TKey : notnull
	{
		ref var value = ref CollectionsMarshal.GetValueRefOrAddDefault(dict, key, out var exists);
		if (!exists) value = func(key);
		return value!;
	}
}
