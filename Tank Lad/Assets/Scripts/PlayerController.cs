using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Inspector-visible variables
    [SerializeField] private float playerMoveSpeed = 2f;

    // Private variables
    private CharacterController2D controller;
    private float playerTurningSpeed = 10f;
    private GameObject tankBaseObject;
    private GameObject tankTurretObject;
    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        // Get references to tank child objects
        tankBaseObject = transform.Find("Tank Base").gameObject;
        tankTurretObject = transform.Find("Tank Turret").gameObject;

        // Get component references
        controller = this.GetComponent<CharacterController2D>();
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = GetAxisValues();
        MoveTank(movement);
        RotateTankBase(movement);

        Vector3 mousePosition = GetMousePosition();
        RotateTankTurret(mousePosition);
    }

    private Vector3 GetAxisValues()
    {
        // Get input axis values and create movement vector
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, vertical);

        return movement;
    }

    private void MoveTank(Vector3 movement)
    {
        // Adjust movement vector by player movement speed and framerate independence
        Vector3 adjustedMovement = movement;
        adjustedMovement *= playerMoveSpeed;
        adjustedMovement *= Time.deltaTime;

        controller.Move(adjustedMovement);
    }

    private void RotateTankBase(Vector3 movement)
    {
        // If the object is moving calculate a rotation for the base
        if (movement.magnitude > 0)
        {
            float angle = Mathf.Atan2(movement.normalized.y, movement.normalized.x) * Mathf.Rad2Deg - 90f;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            tankBaseObject.transform.rotation = Quaternion.Slerp(tankBaseObject.transform.rotation, q, Time.deltaTime * playerTurningSpeed);
        }
    }

    private Vector3 GetMousePosition()
    {
        // Convert screen position to a coordinate in the game world
        Vector3 worldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0;

        return worldPosition;
    }

    private void RotateTankTurret(Vector3 mousePosition)
    {
        // Calculate the vector between the tank position and the mouse position
        Vector3 relativePosition = mousePosition - this.transform.position;

        // Rotate the turret object to face the current mouse position
        float angle = Mathf.Atan2(relativePosition.normalized.y, relativePosition.normalized.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        tankTurretObject.transform.rotation = Quaternion.Slerp(tankTurretObject.transform.rotation, q, Time.deltaTime * playerTurningSpeed);
    }
}
