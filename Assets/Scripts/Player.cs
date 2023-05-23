public class Player
{
    #region stats
    private bool _isAlive;
    private int _health;
    private int _lives;
    private float _speed;
    private float _jumpingPower;
    private float _crouchingSpeed;

    public bool IsAlive => _isAlive;
    public int Health => _health;
    public int Lives => _lives;
    public float Speed => _speed;
    public float JumpingPower => _jumpingPower;
    public float CrouchingSpeed => _crouchingSpeed;
    #endregion

    #region unlockables
    private bool _hasJumpBoost;

    public bool HasJumpBoost { get; set; }
    #endregion

    public Player() 
    {
        _isAlive = true;
        _health = 1;
        _lives = 1;
        _speed = 9f;
        _jumpingPower = 10f;
        _crouchingSpeed = 4f;
    }

    public Player(Player p) 
    {
        _isAlive = p.IsAlive;
        _health = p.Health;
        _lives = p.Lives;
        _speed = p.Speed;
        _jumpingPower = p.JumpingPower;
        _crouchingSpeed = p.CrouchingSpeed;
    }
}
