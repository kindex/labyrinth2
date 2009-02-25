namespace Game
{
    public static class EntryPoint
    {
        static void Main()
        {
            //StreamWriter output = new StreamWriter("output.txt");
            //output.AutoFlush = true;
            //Console.SetOut(output);

            NativeLoader.AutoLoad();

            using (Game game = new Game())
            {
                game.Run();
            }
        }
    }
}
