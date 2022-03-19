using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    [SerializeField] private ScoreManager _scoreManager;
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Wall":
                PlayerEvents.SendWallTouched();
                break;
            case "Floor":
                GameStateManager.Instance.ChangeState(GameState.GameOver);
                break;
        }
    }
}
