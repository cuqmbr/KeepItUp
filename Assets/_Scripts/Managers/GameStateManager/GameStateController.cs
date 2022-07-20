using UnityEngine;
using UnityEngine.Serialization;

public class GameStateController : MonoBehaviour
{
    [FormerlySerializedAs("ChangeToState")] [Tooltip("Selected state will be applied when the game starts or on the button press")]
    public GameState changeToState;

    private void Start() 
    {
        // Change game state to state selected in inspector when the game starts
        GameStateManager.Instance.ChangeState(changeToState);

        PlayerEvents.OnBallTouched += () => GameStateManager.Instance.ChangeState(GameState.Game);
    }

    private void OnApplicationQuit()
    {
        // Change game state back to entry state when exiting playing mode
        GameStateManager.Instance.ChangeState(GameState.Loading);
    }

    public void ChangeState(string newStateStr) => GameStateManager.Instance.ChangeState((GameState) System.Enum.Parse(typeof(GameState), newStateStr));
}