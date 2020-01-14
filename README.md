# ThreadDispatcher
### Easy thread dispatching and managment system

#### Example:
```C#
        using static TheBitBrine.ThreadDispatcher;
        
        static void Main(string[] args)
        {

            for (int i = 0; i < 100; i++)
            {
                var Task = new Action<ThreadManager>(ThreadDummy);
                var x = new ThreadManager() { Action = Task, ThreadName = $"Test" };
                InitThread(x);
            }

            while (true)
            {
                var s = FindManagers("Test");
                foreach (var item in s)
                {
                    Console.WriteLine(item.Status);
                }
                Thread.Sleep(1000);
            }
        }


        public static void ThreadDummy(ThreadManager Intake)
        {
            while (true)
            {
                Intake.Status = Intake.Thread.Name + " - " + DateTime.Now.Second.ToString();
                Thread.Sleep(100);
            }
        }
```        
