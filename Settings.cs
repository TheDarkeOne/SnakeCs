
namespace Snake
{
    public enum Direction
    {
        Up, Down, Right, Left
    };
    class Settings
    {
        public static int Width { get; set; }
        public static int Height { get; set; }
        public static int Speed { get; set; }
        public static int Score { get; set; }
        public static int Points { get; set; }
        public static bool GameOver { get; set; }
        public static bool Win { get; set; }
        public static int winPoints { get; set; }
        public static Direction direction { get; set; }
        public static int maxScore { get; set; }


        public Settings()
        {
            //set the variable to a default value
            Width = 16;
            Height = 16;
            Speed = 16;
            Score = 0;
            Points = 300;
            winPoints = 5000;
            maxScore = 0;
            Win = false;
            GameOver = false;
            direction = Direction.Down;
        }
    }
}
