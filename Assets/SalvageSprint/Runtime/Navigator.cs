using SalvageSprint.Core;
using UnityEngine;
using UnityEngine.AI;

namespace SalvageSprint
{
    [RequireComponent(typeof(NavMeshAgent))]
    public sealed class Navigator : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] private float failedRequestRetryDelay = 1f;
        [SerializeField] private bool logNavigationFailures = true;
        [SerializeField, TextArea] private string lastFailure = "None";

        private NavMeshAgent agent;
        private bool requested;
        private int goalKey = int.MinValue;
        private int requestFrame;

        private Vector3 goal;
        Vector3 lastProgressPosition;
        private float lastProgressTime;
        private float retryAt;

        public TravelStatus Status { get; private set; } = TravelStatus.Idle;
        public Vector3 Goal => goal;
        public int DestinationRequests { get; private set; }
        public string LastFailure => lastFailure;

        private void Awake() 
        { 
            agent = GetComponent<NavMeshAgent>(); 
        }

        public void Request(int key, Vector3 destination)
        {
            bool sameGoal = 
                (requested && 
                key == goalKey && 
                (destination - goal).sqrMagnitude < .01f);

            if (sameGoal && Status != TravelStatus.Failed || Time.time < retryAt) 
                return;

            requested = true;
            goalKey = key;
            goal = destination;
            requestFrame = Time.frameCount;

            lastProgressTime = Time.time;
            lastProgressPosition = transform.position;

            if (!CanNavigate())
            {
                Fail("Agent unavailable or no longer on a NavMesh");
                return;
            }

            agent.isStopped = false;
            DestinationRequests++;

            if (agent.SetDestination(destination))
            {
                Status = TravelStatus.Pending;
                return;
            }
            else
                Fail("SetDestination rejected the request");
            
        }

        public TravelStatus Poll()
        {
            if (!requested || Status == TravelStatus.Failed) 
                return Status;

            if (Time.frameCount == requestFrame) 
                return Status;

            if (!CanNavigate())
            {
                Fail("Agent unavailable or no longer on a NavMesh");
                return Status;
            }

            if (agent == null || !agent.isOnNavMesh)
            { 
                Status = TravelStatus.Failed; 
                return Status; 
            }

            Status = NavigationFeedback.Classify
                (agent.isOnNavMesh, 
                agent.pathPending,
                agent.pathStatus == NavMeshPathStatus.PathComplete, 
                agent.hasPath,
                agent.remainingDistance, 
                agent.stoppingDistance + .12f);

            if ((transform.position - lastProgressPosition).sqrMagnitude > .04f)
            { 
                lastProgressPosition = transform.position;
                lastProgressTime = Time.time; 
            }

            if (Status == TravelStatus.Failed)
            {
                Fail("Navigation feedback rejected the route");
            }
            else if ((Status == TravelStatus.Moving || Status == TravelStatus.Pending) &&
                    Time.time - lastProgressTime > 5)
                Fail("No  significant movement for five seconds");

            return Status;
        }

        public void Cancel()
        {
            if (!requested) 
                return;

            if (agent != null && agent.isOnNavMesh)
            { 
                agent.isStopped = true; 
                agent.ResetPath(); 
            }

            requested = false;
            goalKey = int.MinValue;
            Status = TravelStatus.Idle;
        }
        private bool CanNavigate()
        {
            return agent != null &&
                agent.isActiveAndEnabled &&
                agent.isOnNavMesh;
        }
        private void Fail(string reason)
        {
            bool onMesh = CanNavigate();

            lastFailure =
                $"{reason}; key = {goalKey}; goal = {goal}" +
                $"; position = {transform.position}" +
                $"; on mesh = {onMesh}" +
                $"; no progress for = {Time.time - lastProgressTime:F2}s";

            if (onMesh)
            {
                lastFailure +=
                    $"; pending = {agent.pathPending}" +
                    $"; has path = {agent.hasPath}" +
                    $"; path status = {agent.pathStatus}" +
                    $"; remaining distance = {agent.remainingDistance}" +
                    $"; stopping distance = {agent.stoppingDistance}" +
                    $"; is stopped = {agent.isStopped}";
            }

            Status = TravelStatus.Failed;
            retryAt = Time.time + failedRequestRetryDelay;

            if (onMesh)
            {
                agent.isStopped = true;
                agent.ResetPath();
            }
            if (logNavigationFailures)
                Debug.LogWarning("[Navigator] " + lastFailure, this);
        }

        private void OnDrawGizmosSelected()
        {
            NavMeshAgent navigation = GetComponent<NavMeshAgent>();

            if (navigation == null
                || !navigation.isActiveAndEnabled
                || !navigation.isOnNavMesh
                || !navigation.hasPath)
            {
                return;
            }

            Vector3[] corners = navigation.path.corners;
            Gizmos.color = Color.yellow;

            for (int i = 1; i < corners.Length; i++)
            {
                Gizmos.DrawLine(
                    corners[i - 1] + Vector3.up * .15f,
                    corners[i] + Vector3.up * .15f);
            }
        }
    }
}
