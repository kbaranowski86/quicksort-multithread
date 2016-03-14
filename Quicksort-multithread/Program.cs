using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quicksort_multithread
{
    class ThreadedQuickSort
    {
        private List<int> array;
        private int minIndex;
        private static int[] resultingArray;

        public ThreadedQuickSort(List<int> array, int minIndex)
        {
            this.array = array;
            this.minIndex = minIndex;
            if (resultingArray == null) resultingArray = new int[array.Count];
        }

        private void Sort()
        {
            List<int> leftArray = new List<int>();
            List<int> rightArray = new List<int>();
            int midIndex = (array.Count - 1) / 2;
            int midValue = array[midIndex];

            for(int i = 0; i < array.Count; i++)
            {
                if(i != midIndex)
                {
                    if(array[i] <= midValue)
                    {
                        leftArray.Add(array[i]);
                    }
                    else
                    {
                        rightArray.Add(array[i]);
                    }
                }
            }

            resultingArray[minIndex + leftArray.Count] = midValue;

            Thread leftNodeThread = null;
            Thread rightNodeThread = null;
            if (leftArray.Count > 1)
            {
                ThreadedQuickSort processLeft = new ThreadedQuickSort(leftArray, minIndex);
                leftNodeThread = new Thread(new ThreadStart(processLeft.Sort));
                leftNodeThread.Start();
            }
            else if(leftArray.Count == 1)
            {
                resultingArray[minIndex] = leftArray[0];
            }

            if (rightArray.Count > 1)
            {
                ThreadedQuickSort processRight = new ThreadedQuickSort(rightArray, minIndex + leftArray.Count + 1);
                rightNodeThread = new Thread(new ThreadStart(processRight.Sort));
                rightNodeThread.Start();
            }
            else if(rightArray.Count == 1)
            {
                resultingArray[minIndex + leftArray.Count + 1] = rightArray[0];
            }

            if(leftNodeThread != null) leftNodeThread.Join();
            if (rightNodeThread != null) rightNodeThread.Join();
        }

        public int[] GetSorted() 
        {
            this.Sort();
            return resultingArray;
        }
    };

    class Program
    {
        static void Main(string[] args)
        {
            List<int> inputList = new List<int> { 0, 3, 2, 7, 5, 11, 2, 8, 8 };
            ThreadedQuickSort qSort = new ThreadedQuickSort(inputList, 0);
            int[] resultsArray = qSort.GetSorted();
            for (int i = 0; i < resultsArray.Length; i++)
            {
                Console.WriteLine(resultsArray[i]);
            }
            Console.ReadKey();
        }
    }
}
