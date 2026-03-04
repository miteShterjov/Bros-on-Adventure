using System.Collections;
using UnityEngine;

namespace Enemy.Traps
{
    public class FallingPlatform : MonoBehaviour
    {
        [Header("Config")] 
        [SerializeField] private float speed = 0.75f;
        [SerializeField] private float travelDistance;
        [SerializeField] private Vector3[] waypoints;
        [SerializeField] private bool canMove;
        [SerializeField] private float fallDelay = 0.2f;
        
        private static readonly int AnimParam = Animator.StringToHash("active");
        
        private int _currentWaypointIndex;
        private Animator _animator;
        private Rigidbody2D _rb;
        private BoxCollider2D _boxCollider;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
            _boxCollider = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            SetupWaypoints();
        }

        private void Update()
        {
            if (!canMove) return;
            HandlePlatformMovement();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")) StartCoroutine(PlatformFallingCo());
        }

        private IEnumerator PlatformFallingCo()
        {
            yield return new WaitForSeconds(fallDelay);
            
            _animator.SetBool(AnimParam, true);
            canMove = false;
            _rb.bodyType = RigidbodyType2D.Dynamic;
            _rb.gravityScale = 3.5f;
            _rb.linearDamping = 0.5f;
            _boxCollider.enabled = false;
        }

        private void HandlePlatformMovement()
        {
            const float checkDistance = 0.2f;
            
            transform.position = Vector2.MoveTowards(
                transform.position, 
                waypoints[_currentWaypointIndex], 
                speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, waypoints[_currentWaypointIndex]) < checkDistance)
            {
                _currentWaypointIndex++;
                if (_currentWaypointIndex >= waypoints.Length) _currentWaypointIndex = 0;
            }
        }

        private void SetupWaypoints()
        {
            waypoints = new Vector3[waypoints.Length];

            float offsetY = travelDistance / 2;
            
            waypoints[0] = transform.position + Vector3.up * offsetY;
            waypoints[1] = transform.position - Vector3.up * offsetY;
        }
    }
}
