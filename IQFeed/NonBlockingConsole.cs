using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace IQFeed
{
    public static class NonBlockingConsole
    {
        private static ConcurrentQueue<string> m_Queue = new ConcurrentQueue<string>();

        static NonBlockingConsole()
        {
            var thread = new Thread(
              () =>
              {
                  while (true)
                  {
                      string item;
                      if (m_Queue.Count > 0)
                      {
                          m_Queue.TryDequeue(out item);
                          Console.WriteLine(item);
                      }
                  }
              });
            thread.IsBackground = true;
            thread.Start();
        }

        public static void WriteLine(string value, bool write = false)
        {
            if (write)
                m_Queue.Enqueue(value);
        }
    }
}