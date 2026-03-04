using System.Collections;
using UnityEngine;

namespace Enemy.Traps
{
    public class SawTrap : MonoBehaviour
    {
        [Header(("Config"))] [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private bool isStopingAtWaypoints;
        [SerializeField] private bool isLoopMoving;
        [SerializeField] private float waitTimeAtWaypoint = 0.5f;
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private Vector3[] waypointVectors;

        private static readonly int StopMovingParam = Animator.StringToHash("isWorking");

        private Animator _animator;
        private int _currentWaypointIndex;
        private bool _isWaiting;
        private const float CheckDistance = 0.2f;
        private Vector3 _targetPosition;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            ConvertWaypointsToVectors();
            _animator.SetBool(StopMovingParam, false);
        }

        private void Update()
        {
            if (_isWaiting) return;
            HandleSawMovement();
        }

        private void HandleSawMovement()
        {
            _targetPosition = waypointVectors[_currentWaypointIndex];

            transform.position = Vector2.MoveTowards(
                transform.position,
                _targetPosition,
                moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, _targetPosition) < CheckDistance)
            {
                if (isStopingAtWaypoints) StartCoroutine(StopAtWaypointCo());
                _targetPosition = GetNextPosition();
            }
        }

        private Vector3 GetNextPosition()
        {
            if (isLoopMoving)
            {
                if (_currentWaypointIndex == waypointVectors.Length - 1)
                {
                    _currentWaypointIndex = 0;
                    return waypointVectors[_currentWaypointIndex];
                }
            }

            _currentWaypointIndex++;
            return waypointVectors[_currentWaypointIndex];
        }

        private IEnumerator StopAtWaypointCo()
        {
            _isWaiting = true;
            _animator.SetBool(StopMovingParam, true);
            yield return new WaitForSeconds(waitTimeAtWaypoint);
            _isWaiting = false;
            _animator.SetBool(StopMovingParam, true);
        }


        private void ConvertWaypointsToVectors()
        {
            waypointVectors = new Vector3[waypoints.Length];
            for (int i = 0; i < waypoints.Length; i++)
            {
                waypointVectors[i] = waypoints[i].position;
            }
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            Gizmos.color = Color.red;
            foreach (var t in waypointVectors)
            {
                Gizmos.DrawWireSphere(t, 0.2f);
            }
        }
    }
}