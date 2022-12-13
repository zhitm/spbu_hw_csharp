namespace Task2;

public class Program
{
    public static (string, int) Bwt(string str)
    {
        var result = "";
        var suffixArray = GetSuffixIndices(str + str);
        suffixArray = suffixArray.Where(x => x < str.Length).ToArray();
        foreach (var index in suffixArray)
        {
            if (index == 0)
            {
                result += str.Last();
                continue;
            }

            result += str[index - 1];
        }

        return (result, Array.IndexOf(suffixArray, 0));
    }

    public static string ReverseBwt(string str, int initialPosition)
    {
        var result = "";
        var alphabet = new List<char>();
        foreach (var el in str)
        {
            if (!alphabet.Contains(el))
            {
                alphabet.Add(el);
            }
        }

        alphabet.Sort();

        var frequency = new int[alphabet.Count];
        foreach (var el in str)
        {
            frequency[alphabet.IndexOf(el)] += 1;
        }

        var charPositionsInSortArray = new int[str.Length];
        for (int i = 0; i < str.Length; i++)
        {
            charPositionsInSortArray[i] = frequency.Take(alphabet.IndexOf(str[i])).Sum() +
                                          str.Take(i).Count(x => x == str[i]);
        }

        var currentPosition = initialPosition;
        while (result.Length != str.Length)
        {
            result = str[currentPosition] + result;
            currentPosition = charPositionsInSortArray[currentPosition];
        }

        return result;
    }

    private static int[] GetSuffixIndices(string str)
    {
        var array = Enumerable.Range(0, str.Length).ToArray();
        Array.Sort(array, (i, j) => string.Compare(str[i..], str[j..], StringComparison.Ordinal));
        return array;
    }

    public static void Main()
    {
    }
}