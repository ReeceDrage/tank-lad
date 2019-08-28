using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CharacterController2D : MonoBehaviour
{
    // Raycasting & collision detection variables
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private int xRayCount = 4;
    [SerializeField] private int yRayCount = 4;
    [SerializeField] private float colliderPaddingWidth = 0.15f;
    private float xRaySpacing;
    private float yRaySpacing;
    private BoxCollider2D thisCollider;
    private RaycastOrigins raycastOrigins;
    public CollisionData collisionData;

    // Start is called before the first frame update
    void Start()
    {
        Initialise();
    }

    private void Initialise()
    {
        // Get component references
        thisCollider = GetComponent<BoxCollider2D>();

        // Calculate collision detection variables
        CalculateRaySpacing();
    }

    /// <summary>
    /// Re-calculates the 4 corners of the BoxCollider2D component to accomodate for actor movement
    /// </summary>
    private void UpdateRaycastOrigins()
    {
        // Calculate boundary box of the BoxCollider2D component
        Bounds bounds = thisCollider.bounds;
        bounds.Expand(colliderPaddingWidth * -4);

        // Calculate the origins based on the boundary of the collider
        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    /// <summary>
    /// Calculate the spacing between raycast origin points
    /// </summary>
    private void CalculateRaySpacing()
    {
        // Calculate boundary box of the BoxCollider2D component
        Bounds bounds = thisCollider.bounds;
        bounds.Expand(colliderPaddingWidth * -4);

        // Ray count values must be between 2 and the maximum value of an integer data type
        yRayCount = Mathf.Clamp(yRayCount, 2, int.MaxValue);
        xRayCount = Mathf.Clamp(xRayCount, 2, int.MaxValue);

        // Calculate the spacing between raycast origin points
        xRaySpacing = bounds.size.x / (xRayCount - 1);
        yRaySpacing = bounds.size.y / (yRayCount - 1);
    }

    /// <summary>
    /// Moves the actor according to the input velocity values, in keeping with collision detection
    /// </summary>
    /// <param name="velocity">The current velocity the actor should move at</param>
    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        ResetValues();

        // if there is no velocity, no need to calculate collisions
        if (velocity.x != 0)
        {
            HorizontalCollision(ref velocity);
        }

        if (velocity.y != 0)
        {
            VerticalCollision(ref velocity);
        }

        transform.Translate(velocity);
    }

    /// <summary>
    /// Clear temporary values for collision data and moving platform references.
    /// </summary>
    private void ResetValues()
    {
        // Clear collision data so that detected collisions do not persist.
        collisionData.Reset();
    }

    /// <summary>
    /// Calculates collisions on the Y axis, adjusting velocity and CollisionData values accordingly
    /// </summary>
    /// <param name="velocity">A reference to the current velocity the actor should move at</param>
    private void VerticalCollision(ref Vector3 velocity)
    {
        // Calculate direction and distance travelled
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + colliderPaddingWidth * 4;

        // Cast as many rays as define in the yRayCount field
        for (int i = 0; i < xRayCount; i++)
        {
            Vector2 rayOrigin;

            // if moving downwards, cast from the bottom left, else, cast from the top left
            if (directionY == -1)
            {
                rayOrigin = raycastOrigins.bottomLeft + new Vector2(0, colliderPaddingWidth * 2);
            }
            else
            {
                rayOrigin = raycastOrigins.topLeft - new Vector2(0, colliderPaddingWidth * 2);
            }

            // Cast rays from points incrementally to the right of the original point
            rayOrigin += Vector2.right * (xRaySpacing * i + velocity.x);
            RaycastHit2D collision = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            // If a collision occurs, change the velocity to the distance from the collided object, adjusting for collider padding
            if (collision)
            {
                velocity.y = (collision.distance - colliderPaddingWidth * 4) * directionY;
                rayLength = collision.distance;

                collisionData.above = (directionY == 1);
                collisionData.below = (directionY == -1);
            }
        }
    }

    /// <summary>
    /// Calculates collisions on the X axis, adjusting velocity and CollisionData values accordingly
    /// </summary>
    /// <param name="velocity">A reference to the current velocity the actor should move at</param>
    private void HorizontalCollision(ref Vector3 velocity)
    {
        // Calculate direction and distance travelled
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + colliderPaddingWidth * 4;

        // Cast as many rays as define in the xRayCount field
        for (int i = 0; i < yRayCount; i++)
        {
            Vector2 rayOrigin;

            // if moving left, cast from the top left, else, cast from the top right
            if (directionX == -1)
            {
                rayOrigin = raycastOrigins.topLeft + new Vector2(colliderPaddingWidth * 2, 0);
            }
            else
            {
                rayOrigin = raycastOrigins.topRight - new Vector2(colliderPaddingWidth * 2, 0);
            }

            // Cast rays from points incrementally below the original point
            rayOrigin += Vector2.down * (yRaySpacing * i);
            RaycastHit2D collision = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red, 0f);

            // If a collision occurs, change the velocity to the distance from the collided object, adjusting for collider padding, then register
            // that a collision occured in the collisionData struct
            if (collision)
            {
                velocity.x = ((collision.distance - colliderPaddingWidth * 4) * directionX);
                rayLength = collision.distance;

                collisionData.right = (directionX == 1);
                collisionData.left = (directionX == -1);
            }
        }
    }

    // Structs for holding collision information

    /// <summary>
    /// Raycast origin points. Correspond to the 4 corners of a 2D box collider
    /// </summary>
    struct RaycastOrigins
    {
        public Vector2 topLeft;
        public Vector2 topRight;
        public Vector2 bottomLeft;
        public Vector2 bottomRight;
    }

    /// <summary>
    /// Booleans for collisions in both directions on the X and Y axes of the collider
    /// </summary>
    public struct CollisionData
    {
        public bool above { get; set; }
        public bool below { get; set; }
        public bool left { get; set; }
        public bool right { get; set; }

        /// <summary>
        /// Sets all boolean properties to false.
        /// </summary>
        public void Reset()
        {
            above = false;
            below = false;
            left = false;
            right = false;
        }
    }
}
