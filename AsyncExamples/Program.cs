using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncExample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await AsyncIsNotParallel();
            await MixAsyncAwait();
            ToConsumeAsyncFunctionInSyncFunction();
            ToConsumeAsyncVoidFunctionInSyncFunction();
            await Task.Delay(10000);
        }

        public static async Task MixAsyncAwait()
        {
            Console.WriteLine("MixAsyncAwait Called");
            Console.WriteLine("Calling AsyncFunc1");
            await AsyncFunc1();
            Console.WriteLine("AsyncFunc1 Returned");
            Console.WriteLine("Calling AsyncFunc2");
            await NotAwaitedAsyncFunc2();
            Console.WriteLine("AsyncFunc2 Returned");
            Console.WriteLine("Calling AsyncFunc3");
            await AsyncFunc3();
            Console.WriteLine("AsyncFunc3 Returned");
            Console.WriteLine("Returning from MixAsyncAwait");
        }

        public static async Task NotAwaitedAsyncFunc2()
        {
            Console.WriteLine("NotAwaitedAsyncFunc2 Called");
            Task.Delay(100);
            Console.WriteLine("Calling AsyncFunc4");
            AsyncFunc4();
            Console.WriteLine("AsyncFunc4 Returned");
            Console.WriteLine("Returning from NotAwaitedAsyncFunc2");
        }

        public static async Task AsyncIsNotParallel()
        {
            Console.WriteLine("AsyncIsNotParallel Called");
            Console.WriteLine("Calling AsyncFunc1");
            await AsyncFunc1();
            Console.WriteLine("AsyncFunc1 Returned");
            Console.WriteLine("Calling AsyncFunc2");
            await AsyncFunc2();
            Console.WriteLine("AsyncFunc2 Returned");
            Console.WriteLine("Calling AsyncFunc3");
            await AsyncFunc3();
            Console.WriteLine("AsyncFunc3 Returned");
            Console.WriteLine("Returning from AsyncIsNotParallel");
        }

        public static async Task AsyncFunc1()
        {
            Console.WriteLine("AsyncFunc1 Called");
            await Task.Delay(1000);
            Console.WriteLine("Returning from AsyncCall1");
        }

        public static async Task AsyncFunc2()
        {
            Console.WriteLine("AsyncFunc2 Called");
            await Task.Delay(100);
            Console.WriteLine("Calling AsyncFunc4");
            await AsyncFunc4();
            Console.WriteLine("AsyncFunc4 Returned");
            Console.WriteLine("Returning from AsyncFunc2");
        }

        public static async Task AsyncFunc3()
        {
            Console.WriteLine("AsyncFunc3 Called");
            await Task.Delay(2000);
            Console.WriteLine("Returning from AsyncFunc3");
        }

        public static async Task AsyncFunc4()
        {
            Console.WriteLine("AsyncFunc4 Called");
            await Task.Delay(3000);
            Console.WriteLine("Returning from AsyncFunc4");
        }

        public static Task<string> FunctionThatDoesNotHaveAsyncKeyword()
        {
            string str = "hi";
            //return str; This Gives Error Cannot implicitly convert type 'string' to 'System.Threading.Tasks.Task<string>'
            return Task.FromResult(str);
        }

        /// <summary>
        /// this will work properl but nothing is awaited. Its as good as a sync function and async can be removed
        /// </summary>
        /// <returns></returns>
        public static async Task<string> FunctionThatDoesNotHaveAwaitKeyword()
        {
            string str = "hi";
            return str;
        }

        /// <summary>
        /// Proper async method that returns a string
        /// </summary>
        /// <returns></returns>
        public static async Task<string> ProperFunctionThatReturnsString()
        {
            await Task.Delay(10000);
            string str = "hi";
            return str;
        }
        public static void ToConsumeAsyncFunctionInSyncFunction()
        {
            Console.WriteLine("ToConsumeAsyncFunctionInSyncFunction Called");
            Task<string> task = ProperFunctionThatReturnsString(); //Returns Immediately
            Console.WriteLine("Task Returned. Waiting now...");
            string s = task.Result; //Blocking call
            Console.WriteLine("The Function ProperFunctionThatReturnsString has returned");
        }

        public static void ToConsumeAsyncVoidFunctionInSyncFunction()
        {
            Console.WriteLine("ToConsumeAsyncVoidFunctionInSyncFunction Called");
            Task task = AsyncFunc4(); //Returns Immediately
            Console.WriteLine("Task Returned. Waiting now...");
            string s = task.Wait(); //Blocking call
            Console.WriteLine("The Function AsyncFunc4 has returned");
        }

    }
}