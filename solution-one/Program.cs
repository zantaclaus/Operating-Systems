﻿using System;
using System.Threading;

namespace OS_Problem_02
{
  class Thread_safe_buffer
  {
    static int[] TSBuffer = new int[10];
    static int Front = 0;
    static int Back = 0;
    static int Count = 0;

    private static object _Lock = new object();

    static void EnQueue(int eq)
    {
      TSBuffer[Back] = eq;
      Back++;
      Back %= 10;
      Count += 1;
    }

    static int DeQueue()
    {
      int x = 0;
      x = TSBuffer[Front];
      Front++;
      Front %= 10;
      Count -= 1;
      return x;
    }

    static void th01()
    {
      int i;

      for (i = 1; i < 51; i++) // 10 round [0, 1. .., 9]
      {
        while (Count == 10) { }

        lock (_Lock)
        {
          EnQueue(i);
          Thread.Sleep(5);
        }
      }
    }

    static void th011()
    {
      int i;

      for (i = 100; i < 151; i++)
      {
        while (Count == 10) { }

        lock (_Lock)
        {
          EnQueue(i);
          Thread.Sleep(5);
        }
      }
    }

    static void th02(object t)
    {
      int i;
      int j;

      for (i = 0; i < 100; i++)
      {
        while (Count == 0) { }

        lock (_Lock)
        {
          j = DeQueue();
          Console.WriteLine("j={0}, thread:{1}", j, t);
          Thread.Sleep(100);
        }
      }
    }
    static void Main(string[] args)
    {
      // 100 -> 101
      //      -> 1
      Thread t1 = new Thread(th01); // 1   2
      // Thread t11 = new Thread(th011); // 100
      Thread t2 = new Thread(th02);
      // Thread t21 = new Thread(th02);
      // Thread t22 = new Thread(th02);

      t1.Start();
      // t11.Start();
      t2.Start(1);
      // t21.Start(2);
      // t22.Start(3);
    }
  }
}
