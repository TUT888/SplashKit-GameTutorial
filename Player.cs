using SplashKitSDK;

public class Player {
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

    public void Draw() {
        // Draw bitmap 
        _PlayerBitmap.Draw(X, Y);
    }

    public void HandleInput() {
        // Speed up for dashing
        double actual_spd = SPEED; 
        if ( SplashKit.KeyDown(KeyCode.SpaceKey) ) {
            actual_spd *= 2;
        }

        // Basic move
        if ( SplashKit.KeyDown(KeyCode.LeftKey) ) {
            X -= actual_spd;
        }
        if ( SplashKit.KeyDown(KeyCode.RightKey) ) {
            X += actual_spd;
        }
        if ( SplashKit.KeyDown(KeyCode.EscapeKey) ) {
            Quit = true;
        }
    }

    public void StayOnWindow(Window limit) {
        // Define the gap between player and window frames
        const int GAP = 10;
        int GAP_left = GAP;
        int GAP_top = GAP;
        int GAP_right = limit.Width - GAP;
        int GAP_bottom = limit.Height - GAP;

        // Keep the player in the window
        if ( X < GAP_left ) {
            X = GAP_left;
        }
        if ( (X+Width) > GAP_right ) {
            X = GAP_right - Width;
        }
        if ( Y < GAP_top ) {
            Y = GAP_top;
        }
        if ( (Y+Height) > GAP_bottom ) {
            Y = GAP_bottom - Height;
        }
    }

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

    public void UpdateProgress(SplashKitSDK.Timer timer) {
        if (Lives == 0) {
            Quit = true;
        }
        TimeRecord = Convert.ToInt32(timer.Ticks / 1000);
    }

    public void DrawProgress() {
        SplashKit.DrawText($"TimeRecord: {TimeRecord}", SplashKitSDK.Color.Black, "BOLD_FONT", 12, 20, 20);
        SplashKit.DrawText($"Lives: {Lives}", SplashKitSDK.Color.Black, "BOLD_FONT", 12, 20, 50);
        SplashKit.DrawText($"Score: {Scores}", SplashKitSDK.Color.Black, "BOLD_FONT", 12, 20, 80);
    }
}