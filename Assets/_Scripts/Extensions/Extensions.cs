using System.Collections.Generic;

public static class Extensions {
    public static int LowerBoundIndex<TKey, TValue>(this SortedList<TKey, TValue> list, TKey key) {
        if (list.Count == 0)
            return -1;

        var comparer = list.Comparer;
        if (comparer.Compare(list.Keys[list.Keys.Count - 1], key) < 0)
            return -1; // if all elements are smaller, then no lower bound

        int first = 0, last = list.Count - 1;
        while (first < last) {
            var middle = first + (last - first) / 2;
            if (comparer.Compare(list.Keys[middle], key) >= 0)
                last = middle;
            else
                first = middle + 1;
        }

        return last; //list[list.Keys[last]];
    }
}