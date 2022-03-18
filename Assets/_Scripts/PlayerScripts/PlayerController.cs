using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    
    [Header("Movement settings")]
    [SerializeField] private float _punchForce;
    [SerializeField] private float _upForceMultiplier;
    [SerializeField] private float _sideForceMultiplier;
    [Range(0.3f, 1f)] [SerializeField] private float _deadZone;

    [Header("Rigid body's settings")]
    [SerializeField] private float _gravityMultiplier;
    private float _initialPositionY;
    private float _initialGravityScale;

    [Header("Miscellaneous")]
    [SerializeField] private GameObject _touchTrigger;
    [SerializeField] private ScoreManager _scoreManager;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        
        _initialPositionY = transform.position.y;
        _initialGravityScale = _rigidbody.gravityScale;
    }

    public void OnTouch(Vector2 pointerPos)
    {
        //Get bounce direction based on the tap position
        Vector2 bounceDir = (Vector2)transform.position - pointerPos;

        //Check if pointer is placed on the touchable trigger zone
        if (bounceDir.magnitude > 0.5f * _touchTrigger.transform.lossyScale.x) return;
        
        //Check if the pointer position overlaps the dead zone and set horizontal bounce direction to maximum horizontal bounce direction
        if (Mathf.Abs(bounceDir.x) >= _deadZone * 0.5f * transform.localScale.x) bounceDir.x = (bounceDir.x > 0 ? _deadZone : -_deadZone) * 0.5f * transform.localScale.x;

        //Reset rigid body's velocity
        _rigidbody.velocity = Vector2.zero;
            
        //Apply impulse force
        _rigidbody.AddForce(_punchForce *  new Vector2(bounceDir.x * _sideForceMultiplier,  _upForceMultiplier), ForceMode2D.Impulse);
        //Add angular velocity for the visual effect
        _rigidbody.angularVelocity +=  bounceDir.x * -360f;
        
        PlayerEvents.SendBallTouched();
    }

    private void Update()
    {
        //Change gravitational force over height
        _rigidbody.gravityScale = _initialGravityScale + ((transform.position.y - _initialPositionY) * _gravityMultiplier);
    }
}
