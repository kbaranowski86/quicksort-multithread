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
        private QuickSortThread qst;

        public ThreadedQuickSort(List<int> array, uint parallelizationLevel = 10)
        {
            qst = new QuickSortThread(array, 0, parallelizationLevel);
        }

        public int[] GetSorted()
        {
            return qst.GetSorted();
        }

        private class QuickSortThread
        {
            private List<int> array;
            private int minIndex;
            private static int[] resultingArray;
            private uint parallelizationLevel;

            public QuickSortThread(List<int> array, int minIndex, uint parallelizationLevel = 10)
            {
                this.array = array;
                this.minIndex = minIndex;
                this.parallelizationLevel = parallelizationLevel;
                if (resultingArray == null) resultingArray = new int[array.Count];
            }

            private void SortCore(List<int> array, int minIndex)
            {
                List<int> leftArray = new List<int>();
                List<int> rightArray = new List<int>();
                int midIndex = (array.Count - 1) / 2;
                int midValue = array[midIndex];

                for (int i = 0; i < array.Count; i++)
                {
                    if (i != midIndex)
                    {
                        if (array[i] <= midValue)
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
                    if (parallelizationLevel == 0)
                    {
                        SortCore(leftArray, minIndex);
                    }
                    else
                    {
                        QuickSortThread processLeft = new QuickSortThread(leftArray, minIndex, parallelizationLevel - 1);
                        leftNodeThread = new Thread(new ThreadStart(processLeft.Sort));
                        leftNodeThread.Start();
                    }
                }
                else if (leftArray.Count == 1)
                {
                    resultingArray[minIndex] = leftArray[0];
                }

                if (rightArray.Count > 1)
                {
                    if (parallelizationLevel == 0)
                    {
                        SortCore(rightArray, minIndex + leftArray.Count + 1);
                    }
                    else
                    {
                        QuickSortThread processRight = new QuickSortThread(rightArray, minIndex + leftArray.Count + 1, parallelizationLevel - 1);
                        rightNodeThread = new Thread(new ThreadStart(processRight.Sort));
                        rightNodeThread.Start();
                    }
                }
                else if (rightArray.Count == 1)
                {
                    resultingArray[minIndex + leftArray.Count + 1] = rightArray[0];
                }

                if (leftNodeThread != null) leftNodeThread.Join();
                if (rightNodeThread != null) rightNodeThread.Join();
            }

            private void Sort()
            {
                SortCore(array, minIndex);
            }

            public int[] GetSorted()
            {
                this.Sort();
                return resultingArray;
            }
        }
    };

    class Program
    {
        static void Main(string[] args)
        {
            List<int> inputList = new List<int> { 0, 3, 2, 7, 5, 11, 2, 8, 8 };
            ThreadedQuickSort qSort = new ThreadedQuickSort(inputList);
            int[] resultsArray = qSort.GetSorted();
            for (int i = 0; i < resultsArray.Length; i++)
            {
                Console.WriteLine(resultsArray[i]);
            }
            Console.ReadKey();
        }
    }
}
