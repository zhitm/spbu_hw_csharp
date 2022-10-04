using System;

namespace Task1;

public class Program
{
    public static void PrintArray(int[] arr)
    {
        foreach (int el in arr)
        {
            Console.Write("{0} ", el);
        }

        Console.WriteLine('\n');
    }

    public static bool IsSorted(int[] arr)
    {
        for (int i = 0; i < arr.Length - 1; i++)
        {
            if (arr[i] > arr[i + 1])
            {
                return false;
            }
        }

        return true;
    }

    public static void BubbleSort(int[] arr)
    {
        bool isSorted = false;
        while (!isSorted)
        {
            isSorted = true;
            int size = arr.Length;
            for (int i = 0; i < size - 1; i++)
            {
                if (arr[i] > arr[i + 1])
                {
                    (arr[i], arr[i + 1]) = (arr[i + 1], arr[i]);
                    isSorted = false;
                }
            }
        }
    }

    public static void Main(string[] args)
    {
        Console.WriteLine("Input an array like 1 2 5 6 32");

        string input = Console.ReadLine();
        try
        {
            int[] array = Array.ConvertAll(input.Trim().Split(' '), Convert.ToInt32);
            BubbleSort(array);
            Console.WriteLine("Sorted array:");
            PrintArray(array);
        }
        catch (FormatException e)
        {
            Console.WriteLine("It's not an array");
        }
    }
}