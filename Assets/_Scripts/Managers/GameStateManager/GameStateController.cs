using UnityEngine;

public class GameStateController : MonoBehaviour
{
    [Tooltip("Selected state will be applied when the game starts or on the button press")]
    public GameState ChangeToState;

    private void Awake()
    {
        // Change game state to selected in inspector state when the game starts
        GameStateManager.Instance.ChangeState(ChangeToState);

        PlayerEvents.OnBallTouched += () => GameStateManager.Instance.ChangeState(GameState.Game);
    }

    private void OnApplicationQuit()
    {
        // Change game state back to entry state when exiting playing mode
        GameStateManager.Instance.ChangeState(GameState.Enter);
    }

    public void ChangeState(string newStateStr) =>
        GameStateManager.Instance.ChangeState((GameState) System.Enum.Parse(typeof(GameState), newStateStr));
}