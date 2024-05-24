using System;
using SplashKitSDK;

namespace TutorialGame
{
    public class Program
    {
        public static void Main()
        {
            Window gameWindow = new Window("Game Window", 1200, 600);
            
            ItemCatch itemCatch = new ItemCatch(gameWindow);
            while ( !itemCatch.Quit ) {
                SplashKit.ProcessEvents();

                itemCatch.HandleInput();
                itemCatch.Update();
                itemCatch.Draw();

                if ( gameWindow.CloseRequested ) {
                    break;
                }
            }
            gameWindow.Close();
            
            Console.WriteLine($"Time record:    {itemCatch.TimeRecord}  second(s)");
            Console.WriteLine($"Score record:   {itemCatch.Scores}      apple(s)");
        }
    }
}
