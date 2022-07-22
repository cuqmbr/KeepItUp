using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    [SerializeField] private ScoreManager _scoreManager;
    
    private void OnCollisionEnter2D(Collision2D coll)
    {
        switch (coll.gameObject.tag)
        {
            case "Wall":
                PlayerEvents.SendWallTouched();
                break;
            case "Floor":
                PlayerEvents.SendDeath();
                GameStateManager.Instance.ChangeState(GameState.GameOver);
                break;
        }
    }
}