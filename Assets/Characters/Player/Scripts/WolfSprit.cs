using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpritState
{
    Orbitaround,
    Attack
}

public class WolfSprit : MonoBehaviour
{
    public Transform target; // The main character or central point

    private float currentAngle; // Current angle of rotation
    private float speed = 75f; // Rotation speed of the spirit
    private float radius; // Rotation radius of the spirit
    private Vector3 previousPosition; // The spirit's position in the last frame

    private SpritState spritstate;

    public SpritState SpritState
    {
        get => spritstate;
        private set
        {
            spritstate = value;
            switch (spritstate)
            {
                case SpritState.Orbitaround:
                    break;
                case SpritState.Attack:
                    // Add attack logic here in the future
                    break;
            }
        }
    }

    void Start()
    {
        target = transform.parent;
        // Calculate the radius based on the initial position of the spirit and the target
        radius = Vector3.Distance(transform.position, target.position);

        // Initialize the rotation angle based on the spirit's initial position
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        currentAngle = Mathf.Atan2(directionToTarget.z, directionToTarget.x) * Mathf.Rad2Deg;

        previousPosition = transform.position;
    }

    void LateUpdate()
    {
        OrbAround();
    }

    private void OrbAround()
    {
        currentAngle += speed * Time.deltaTime;

        // Calculate new position based on the rotation angle
        float x = target.position.x + radius * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float z = target.position.z + radius * Mathf.Sin(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y; // maintain the original y position

        Vector3 newPosition = new Vector3(x, target.position.y + 1, z);
        //newPosition.y = 1;
        transform.position = newPosition;

        // Rotate the spirit to face the direction of its movement
        SetDirection(newPosition, previousPosition);

        //previousPosition.y = 1;
        previousPosition = newPosition;
    }

    // sets direction that the wolf head faces
    private void SetDirection(Vector3 current, Vector3 previous)
    {
        // Rotate the spirit to face the direction of its movement
        Vector3 moveDirection = current - previous;
        if (moveDirection != Vector3.zero) // Ensure we don't attempt to look in a zero direction
        {
            Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
}