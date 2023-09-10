using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class RangedEnemyAI : MonoBehaviour
{
    public Transform target;
    public LightDroneAnimationController animationController;
    [Space(5)]
    public float startFollowingDistance = 50;
    public float stopDistance = 30;
    public float marginOfErrorDistance = 2;
    public float startFleeingDistance = 25;
    public int fleeingPathDepth = 5;
    public float nextWaypointDistance = 2;
    [Space(5)]
    public float maxSpeed = 10;
    public float accelaration = 5;
    [Header("Debug Info")]
    [SerializeField] private float distanceToTarget;


    private Path path;
    private int currentWaypoint = 0;

    private Seeker seeker;
    private new Rigidbody2D rigidbody;

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (target == null)
        {
            target = GameObject.Find("Player").transform;
        }
        InvokeRepeating(nameof(UpdatePath), 0f, 0.5f);
    }

    private void UpdatePath()
    {
        if(seeker.IsDone()&&target != null)
        {
            float distance = Vector2.Distance(rigidbody.position, target.position);
            if (distance > startFollowingDistance) return;

            if (distance <= stopDistance)
            {
                if (distance <= startFleeingDistance) 
                {
                    MakeFleeingPath();
                }
            }
            else
            {
                seeker.StartPath(rigidbody.position, target.position, OnPathComplete);
            }
            
        }
    }

    private void MakeFleeingPath()
    {
        if (!seeker.IsDone()) return;

        GraphNode currentNode = AstarPath.active.GetNearest(rigidbody.position).node;
        List<GraphNode> nearbyNodes = PathUtilities.BFS(currentNode, fleeingPathDepth);

        float longestDistance = 0;
        GraphNode farthestNode = null;
        foreach (GraphNode node in nearbyNodes )
        {
            float distance = Vector2.Distance((Vector3)node.position, target.position);
            if (distance > longestDistance) 
            {
                longestDistance = distance;
                farthestNode = node;
            }
        }

        if (farthestNode != null)
        {
            seeker.StartPath(rigidbody.position, (Vector3)farthestNode.position, OnPathComplete);
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void FixedUpdate()
    {
        if(target==null) return;
        distanceToTarget = Vector2.Distance(rigidbody.position, target.position);

        if (path == null) 
        {
            return;
        }

        if ((distanceToTarget > stopDistance - marginOfErrorDistance && distanceToTarget < stopDistance + marginOfErrorDistance) || distanceToTarget > startFollowingDistance)
        {
            return;
        }

        if (currentWaypoint>=path.vectorPath.Count)
        {
            return;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rigidbody.position).normalized;
        if (direction.x > 0.01f)
        {
            animationController.IsFacingLeft = false;
        }
        else if (direction.x < -0.01f) 
        {
            animationController.IsFacingLeft = true;
        }

        if(Vector2.Dot(rigidbody.velocity,direction)<maxSpeed)
        {
            rigidbody.AddForce(accelaration * rigidbody.mass * direction, ForceMode2D.Force);
        }

        float distance = Vector2.Distance(rigidbody.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance) 
        {
            currentWaypoint++;
        }
    }
}
