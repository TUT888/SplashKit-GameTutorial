using SplashKitSDK;

public abstract class Item {
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
}

public class Bomb : Item {
    public Bomb(Window gameWindow) : base(gameWindow) {
        _ItemBitmap = new Bitmap("Bomb", "resources/images/Bomb.png");
        isDanger = true;
    }
}

// New changes below
public class Apple : Item {
    public Apple(Window gameWindow) : base(gameWindow) {
        _ItemBitmap = new Bitmap("Apple", "resources/images/Apple.png");
        isDanger = false;
    }
}