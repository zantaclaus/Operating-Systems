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

    static object _Lock = new Object();

    static void EnQueue(int eq)
    {
      while (Count == 10) { }

      lock (_Lock)
      {
        TSBuffer[Back] = eq;
        Back++;
        Back %= 10;
        Count += 1;
      }
    }

    static int DeQueue()
    {
      lock (_Lock)
      {
        int x = 0;
        x = TSBuffer[Front];
        Front++;
        Front %= 10;
        Count -= 1;
        return x;
      }
    }

    static void th01()
    {
      int i;

      for (i = 1; i < 51; i++)
      {
        EnQueue(i);
        Thread.Sleep(5);
      }
    }

    static void th011()
    {
      int i;

      for (i = 100; i < 151; i++)
      {
        EnQueue(i);
        Thread.Sleep(5);
      }
    }

    static void th02(object t)
    {
      int i;
      int j;

      for (i = 0; i < 50; i++)
      {
        while (Count == 0) { }

        j = DeQueue();
        Console.WriteLine("j={0}, thread:{1}", j, t);
        Thread.Sleep(100);
      }
    }

    static void Main(string[] args)
    {
      Thread t1 = new Thread(th01);
      Thread t11 = new Thread(th011);
      Thread t2 = new Thread(th02);
      // Thread t21 = new Thread(th02);
      // Thread t22 = new Thread(th02);

      t1.Start();
      t11.Start();
      t2.Start(1);
      // t21.Start(2);
      // t22.Start(3);
    }
  }
}

// Add 1 Enqueue Thread
// ต้องเพ่ิม while-loop ใน th011 ด้วย ก็เลย
// move while-loop to Enqueue function

// th01 กับ th011 มีโอกาสผ่าน while-loop มาพร้อมกัน ทำให้รอใช้ฟังก์ชัน Equeue เลย
// พอ th01 เพิ่มตัวที่ 10 เข้าคิว, th011 ก็เพิ่มตัวต่อไปมาทับเลย เพราะผ่าน while-loop มาเเล้ว