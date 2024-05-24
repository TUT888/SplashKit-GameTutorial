using SplashKitSDK;

public class ItemCatch {
    private Player _Player;
    private Window _GameWindow;
    private List<Item> _Items;
    public bool Quit { get { return _Player.Quit; } }
    private SplashKitSDK.Timer _GameTimer;
    public int TimeRecord { get { return _Player.TimeRecord; } }
    public int Scores { get { return _Player.Scores; } }

    public ItemCatch(Window gameWindow) {
        _GameWindow = gameWindow;

        _GameTimer = new SplashKitSDK.Timer("Game Timer");
        _GameTimer.Start();

        _Player = new Player(_GameWindow);
        // Initialize Item
        _Items = new List<Item>();
        _Items.Add(RandomItem());
    }

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
        if ( _Items.Count < 8 ) {
            _Items.Add(RandomItem());
        }
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
}