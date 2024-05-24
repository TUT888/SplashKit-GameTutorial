# Tutorial on ItemCatch Game using C# SplashKit

---

This is a short tutorial on creating a simple game using SplashKit. The three main steps I mainly go through when developing a game. Complex game may requires more analysis and extra steps, but for a simple game, it mainly includes below three steps:

1. Identifying the game and ideas.
2. Identifying the classes and logic that the game should have.
3. Implement it using C# SplashKit.

As an example, this brief tutorial will implement a game named **ItemCatch**. 

![Untitled](Tutorial%20on%20ItemCatch%20Game%20using%20C#%20SplashKit%206473e31927294509aa708e3968fa497a/Untitled.png)

# 1. Identifying the game and ideas

The ideas of **ItemCatch** game is simple, which are listed as below:

- Player is able to move on the screen horizontally.
- Items will slowly fall from the top. There are 2 types of items:
    - Bomb: A danger item that we need to dodge it, player will reduce lives when interact with bombs.
    - Apple: A safe item that we should receive, player will increase score when interact with apples.
- Game records includes remaining lives, scores gained from receiving apples, and the time we can survive.
- There is no limit of time for playing, player can press ESC to exit the game.

# 2. Identifying the classes and logic

With the above description, we can identify classes and their relationship one by one based on listed features.

- As the analyzed above, the game should includes classes such as:
    - The `Player` and `Item` class, which are the essential objects in this game.
    - The `ItemCatch` class, which will manage the player and items, as well as their interaction.
- For the overall analysis of the `Player` class:
    - It should includes fields for:
        - Tracking the player location.
        - Saving the game record that the player has achieved.
    - It should includes methods for:
        - Handle user input to move and keep the player on the screen.
        - Receive the falling items, and make changes to game record based on the item’s type.
- For the overall analysis of the `Item` class:
    - It should includes fields for:
        - Tracking the item location.
        - Checking the collision circle with the player.
    - It should includes methods for:
        - Automatically update the Item location, since they are falling from the top.
    - The `Item` class should also have 2 child classes, `Bomb` and `Apple` . These 2 child class should:
        - Have different appearance.
        - Can be distinguished by their characteristic (is danger or not).
- For the overall analysis of the `ItemCatch` class:
    - It should includes fields for:
        - Managing a player and items
        - Managing the game window.
    - It should includes methods for:
        - Initialize player and randomly generate 2 types of falling items.
        - Handle the user input, update the position and draw objects.
        - Checking the collision between player and items, and remove items if they are received or offscreen.

# 3. Implementation using C# and SplashKit

The last step is the implementation of the game. I assume that the programming environment has been setup successfully (C#, SplashKit, code editor like Visual Studio Code, etc) and readers should have some basic knowledge in programming.

In the first step, we need to create a directory containing new project with resources to store images using below commands:

```bash
mkdir TutorialGame
cd TutorialGame

skm dotnet new console
skm resources
```

## 3.1. The Player Class

For the `Player` class, we need to create a **Player.cs** file to store and add some codes.

- As discussed above, the player should includes the fields:
    - **Player Bitmap, Width** and **Height:** which define player’s appearance.
    - **SPEED** and **Coordinates (X and Y):** which allows the player to move on the screen.
    - **The height of the land** where the player stand on.
    - **The game record** (Lives, Time and Score) which the player has achieved.
    
    ```csharp
    // Basic player information
    private Bitmap _PlayerBitmap;
    private const double SPEED = 5.0;
    public double X { get; private set; }
    public double Y { get; private set; }
    public int Width { get { return _PlayerBitmap.Width; } }
    public int Height { get { return _PlayerBitmap.Height; } }
    
    // Extra fields
    public double LAND_HEIGHT { get; set; }
    public bool Quit { get; private set; }
    public int Lives { get; private set; }
    public int TimeRecord { get; private set; }
    public int Scores { get; private set; }
    ```
    
- About the methods:
    - Initialize the fields in the constructor:
        
        ```csharp
        public Player(Window gameWindow) {
            // Load the player image to _PlayerBitmap
            _PlayerBitmap = new Bitmap("Player", "resources/images/Player.png");
            // Defining height of the land that the player stands on
            LAND_HEIGHT = gameWindow.Height / 5;
            // The player start in middle of the screen
            X = (gameWindow.Width - Width) / 2;
            Y = (gameWindow.Height - Height) - LAND_HEIGHT;
        
            Lives = 5;                  
            TimeRecord = 0;
            Scores = 0;
        }
        ```
        
    - Creating methods allowing the player to appear and move around the screen:
        
        ```csharp
        public void Draw() {
            // Draw bitmap 
            _PlayerBitmap.Draw(X, Y);
        }
        
        public void HandleInput() {
            // Speed up for dashing
            double actual_spd = SPEED; 
            if ( SplashKit.KeyDown(KeyCode.SpaceKey) ) { actual_spd *= 2; }
            // Basic move
            if ( SplashKit.KeyDown(KeyCode.LeftKey) ) { X -= actual_spd; }
            if ( SplashKit.KeyDown(KeyCode.RightKey) ) { X += actual_spd; }
            if ( SplashKit.KeyDown(KeyCode.EscapeKey) ) { Quit = true; }
        }
        
        public void StayOnWindow(Window limit) {
            // Define the gap between player and window frames
            const int GAP = 10;
            int GAP_left = GAP;
            int GAP_top = GAP;
            int GAP_right = limit.Width - GAP;
            int GAP_bottom = limit.Height - GAP;
            // Keep the player in the window
            if ( X < GAP_left ) { X = GAP_left; }
            if ( (X+Width) > GAP_right ) { X = GAP_right - Width; }
            if ( Y < GAP_top ) { Y = GAP_top; }
            if ( (Y+Height) > GAP_bottom ) { Y = GAP_bottom - Height; }
        }
        ```
        
    - The methods for interacting with items:
        
        ```csharp
        public bool ReceiveItem(Item other) {
            // Check if the item fall on the player
            return _PlayerBitmap.CircleCollision(X, Y, other.CollisionCircle);
        }
        
        public void ReduceLive() {
            if (Lives > 0) {
                Lives -= 1;
            } else {
                Lives = 0;
            }
        }
        
        public void increaseScore() {
            Scores += 1;
        }
        ```
        
    - The methods for updating and displaying the progress:
        
        ```csharp
        public void UpdateProgress(SplashKitSDK.Timer timer) {
            if (Lives == 0) { Quit = true; }
            TimeRecord = Convert.ToInt32(timer.Ticks / 1000);
        }
        
        public void DrawProgress() {
            SplashKit.DrawText($"TimeRecord: {TimeRecord}", SplashKitSDK.Color.Black, "BOLD_FONT", 12, 20, 20);
            SplashKit.DrawText($"Lives: {Lives}", SplashKitSDK.Color.Black, "BOLD_FONT", 12, 20, 50);
            SplashKit.DrawText($"Score: {Scores}", SplashKitSDK.Color.Black, "BOLD_FONT", 12, 20, 80);
        }
        ```
        

## 3.2. The Item Class

For the `Item` class, we need to make this class **abstract** and store it in **Item.cs** file.

- As discussed above, the abstract item class should includes the fields:
    - **Item Bitmap, Width** and **Height:** which define item’s appearance.
    - **SPEED** and **Coordinates (X and Y):** which allows the item to fall from the top of the screen.
    - **The isDanger field** which store boolean values to identify if the item is danger or not.
    - **The Collision Circle** which will check if the item interact with the player.
    
    ```csharp
    // Basic item information
    protected Bitmap _ItemBitmap;
    private const double SPEED = 3.0;
    public double X { get; set; }
    public double Y { get; set; }
    public int Width { get { return _ItemBitmap.Width; } }
    public int Height { get { return _ItemBitmap.Width; } }
    // Extra fields
    public bool isDanger { get; set; }
    public Circle CollisionCircle { get { return SplashKit.CircleAt(X+Width/2, Y+Height/2, 20); } }
    ```
    
- About the methods,
    - A constructor to initialize the location from the top.
    - It should includes methods to make the item falling from the top of the screen:
    
    ```csharp
    public Item(Window gameWindow) {        
        // Randomly pick a position on top of the screen
        X = SplashKit.Rnd(gameWindow.Width);
        Y = 0;
    }
    public void Draw() {
        // Draw bitmap 
        _ItemBitmap.Draw(X, Y);
    }
    
    public void Update() {
        // The item fall from the top, so we only update the coordinate Y
        Y += SPEED;
    }
    
    public bool IsOffScreen(Window screen) {
        // Check if the item is out of window screen
        return (Y<-Height || Y>screen.Height);
    }
    ```
    

Below the abstract class, create 2 child class `Bomb` and `Apple` that inherit from `Item` class. These 2 class have different constructors that load different images and different characteristics (is danger or not)

```csharp
// Inside the contructor of Bomb
_ItemBitmap = new Bitmap("Bomb", "resources/images/Bomb.png");
isDanger = true;

// Inside the constructor of Apple
_ItemBitmap = new Bitmap("Apple", "resources/images/Apple.png");
isDanger = false;
```

## 3.3. The ItemCatch Class

This class is where we manage the game.

- Starting with the fields, includes the player and list of items as analyzed before.
    
    ```csharp
    private Player _Player;
    private Window _GameWindow;
    private List<Item> _Items;
    public bool Quit { get { return _Player.Quit; } }
    private SplashKitSDK.Timer _GameTimer;
    public int TimeRecord { get { return _Player.TimeRecord; } }
    public int Scores { get { return _Player.Scores; } }
    ```
    
- For the methods and constructor:
    - Initialize the fields in constructor:
    
    ```csharp
    public ItemCatch(Window gameWindow) {
        _GameWindow = gameWindow;
        _GameTimer = new SplashKitSDK.Timer("Game Timer");
        _GameTimer.Start();
    		// Initialize Player
        _Player = new Player(_GameWindow);
        // Initialize Item
        _Items = new List<Item>();
        _Items.Add(RandomItem());
    }
    ```
    
    - Adding methods for handling inputs and randomly create items, then draw all objects
    
    ```csharp
    public void HandleInput() {
        _Player.HandleInput();
        _Player.StayOnWindow(_GameWindow);
    }
    
    public void Update() {
        _Player.UpdateProgress(_GameTimer);
        // Update all Items and check collision
        foreach (Item Item in _Items) {
            Item.Update();
        }
        CheckCollision();
        // Limit the amount of Items
        if ( _Items.Count < 8 ) { _Items.Add(RandomItem()); }
    }
    
    public void Draw() {
        _GameWindow.Clear(Color.White);
        // Draw the land that the player stands on
        SplashKit.FillRectangle(
            SplashKitSDK.Color.SandyBrown, 
            0, (_GameWindow.Height-_Player.LAND_HEIGHT), 
            _GameWindow.Width, _Player.LAND_HEIGHT
        );
        // Draw all Items
        _Player.Draw();
        foreach (Item Item in _Items) {
            Item.Draw();
        }
        // Draw game progress
        _Player.DrawProgress();
        // Refresh window to make changes
        _GameWindow.Refresh(60);
    }
    
    // New change in RandomItem()
    public Item RandomItem() {
        int rndNum = SplashKit.Rnd(2);
        Item Item;
        if ( rndNum==0 ) {
            Item = new Bomb(_GameWindow);
        } else {
            Item = new Apple(_GameWindow);
        }
        return Item;
    }
    ```
    
    - We must also implement a method to check the collision for updating the game progress:
    
    ```csharp
    private void CheckCollision() {
        List<Item> itemsToBeRemoved = new List<Item>();
        // Loop all items
        foreach (Item Item in _Items) {
            // Check if the player receive any item
            if ( _Player.ReceiveItem(Item) ) {
                itemsToBeRemoved.Add(Item);
                // Check if the item is danger
                if ( Item.isDanger ) {
                    _Player.ReduceLive();
                } else {
                    _Player.increaseScore();
                }
            } else if ( Item.IsOffScreen(_GameWindow) ) {
                // Also remove item if it is offscreen
                itemsToBeRemoved.Add(Item);
            }
        }
        // Remove items
        foreach (Item Item in itemsToBeRemoved) {
            _Items.Remove(Item);
        }
    }
    ```
    

## 3.4. The Main Program

After implementing all the classes and their relationships. We move on the main program.

In this main program:

- Create a game window, then pass this window as parameter for the ItemCatch game.
- Start the loop:
    - Let the game to handle input, update data and draw objects sequentially.
    - Break the loop if the user choose to quit the game.
- Close the game window, and print out the game result on the console.

```csharp
public static void Main() {
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
```

# 4. Conclusion

That’s how I create a game using C# and SplashKit. I hope this tutorial can help readers to understand OOP concepts in C# and how to apply these knowledge to build a simple game. 

Other complex games may requires more analysis and effort to build, feel free to try and enhance your programming skills.