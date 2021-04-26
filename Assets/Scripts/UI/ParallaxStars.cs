
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ParallaxStars: MonoBehaviour {

    private PlayerMovement _playerMovement;
    
    private Vector2 _randomCenter;
    private SpriteRenderer _renderer;

    [SerializeField] private bool _tryToFollowPlayer = true;
    [SerializeField] private float _parallaxMultiplier = 1f;
    [SerializeField] private Vector2 _currentSpeed = new Vector2(0f, 0f);

    private void Awake() {
        if (_tryToFollowPlayer) { 
            _playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        }
        
        _renderer = GetComponent<SpriteRenderer>();
        var gen = new System.Random();
        
        _randomCenter = new Vector2((float) gen.NextDouble() * 2f - 1f, (float) gen.NextDouble() * 2f - 1f);
        transform.SetPositionAndRotation(_randomCenter, transform.rotation);
    }

    private void Update() {
        var individualSize = _renderer.size;
        individualSize /= 4.0f;

        if (_tryToFollowPlayer) {
            _currentSpeed = _playerMovement.GetVelocity();

            var nextPosition = transform.position;
            nextPosition += (Vector3) _currentSpeed * (_parallaxMultiplier * Time.deltaTime);

            transform.SetPositionAndRotation(nextPosition, transform.rotation);
        } else {
            var nextPosition = transform.position;
            nextPosition += (Vector3) _currentSpeed * (_parallaxMultiplier * Time.deltaTime);

            if (Mathf.Abs(nextPosition.x - _randomCenter.x) >= individualSize.x) {
                nextPosition.x = _randomCenter.x;
            }

            if (Mathf.Abs(nextPosition.y - _randomCenter.y) >= individualSize.y) {
                nextPosition.y = _randomCenter.y;
            }

            transform.SetPositionAndRotation(nextPosition, transform.rotation);            
        }
    }

    public void SetParallaxMultiplier(float mult) {
        _parallaxMultiplier = mult;
    }

    public void SetSpeed(Vector2 speed) {
        _currentSpeed = speed;
    }

}
