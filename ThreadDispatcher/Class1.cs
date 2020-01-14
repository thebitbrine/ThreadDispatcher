using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using System.Linq;

namespace ThreadDispatcher
{
    class Program
    {
        public static List<ThreadManager> ThreadManagerPool = new List<ThreadManager>();
        public static string GetManagerStatus(ThreadManager Manager)
        {
            return Manager.Status; 
        }

        public static dynamic GetManagerOutcome(ThreadManager Manager)
        {
            if (!Manager.Thread.IsAlive)
            {
                var Outcome = Manager.ThreadOutcome;
                if (Manager.Disposable)
                    ThreadManagerPool.Remove(Manager);
                return Outcome;
            }
            return null;
        }

        public static ThreadManager FindManager(string ThreadName, bool CaseSensitive = true)
        {
            ThreadManager TargetManager = null;

            if (!CaseSensitive)
            {
                TargetManager = ThreadManagerPool.Find(x => x.ThreadName.ToUpper() == ThreadName.ToUpper());
                if (TargetManager == null)
                    TargetManager = ThreadManagerPool.Find(x => x.Thread.Name.ToUpper() == ThreadName.ToUpper());
            }
            else
            {
                TargetManager = ThreadManagerPool.Find(x => x.ThreadName == ThreadName);
                if (TargetManager == null)
                    TargetManager = ThreadManagerPool.Find(x => x.Thread.Name == ThreadName);
            }

            return TargetManager;
        }

        public static ThreadManager[] FindManagers(string ThreadName, bool CaseSensitive = true)
        {
            ThreadManager[] TargetManager = null;

            if (!CaseSensitive)
            {
                TargetManager = ThreadManagerPool.Where(x => x.ThreadName.ToUpper().StartsWith(ThreadName.ToUpper())).ToArray();
                if (TargetManager.Length == 0)
                    TargetManager = ThreadManagerPool.Where(x => x.Thread.Name.ToUpper().StartsWith(ThreadName.ToUpper())).ToArray();
            }
            else
            {
                TargetManager = ThreadManagerPool.Where(x => x.ThreadName.StartsWith(ThreadName)).ToArray();
                if (TargetManager.Length == 0)
                    TargetManager = ThreadManagerPool.Where(x => x.Thread.Name.StartsWith(ThreadName)).ToArray();
            }

            return TargetManager;
        }

        public static void InitThread(ThreadManager Target)
        {
            if (Target == null || Target.Thread == null && Target.Action == null) return;
            if (Target.ThreadStart == null) Target.ThreadStart = DateTime.UtcNow;
            if (Target.Thread == null) Target.Thread = new Thread(() => Target.Action(Target));
            if (string.IsNullOrWhiteSpace(Target.Thread.Name)) Target.Thread.Name = $"{Target.ThreadName}.{Guid.NewGuid()}";
            if (string.IsNullOrWhiteSpace(Target.Status)) Target.Status = "Started.";
            Target.Thread.Start();
            ThreadManagerPool.Add(Target);
        }

        public static void ThreadDummy(ThreadManager Intake)
        {
            while (true)
            {
                Intake.Status = Intake.Thread.Name + " - " + DateTime.Now.Second.ToString();
                Thread.Sleep(100);
            }
        }

        public class ThreadManager
        {
            public Thread Thread;
            public Action<ThreadManager> Action;
            public dynamic ActionIntake;
            public string ThreadName = "Thread";
            public string Status;
            public DateTime? ThreadStart;
            public DateTime? ThreadEnd;
            public dynamic ThreadOutcome;
            public bool Disposable;
        }
    }
}
