using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] float turretRotationSpeed = 10f;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player Tank");
    }

    // Update is called once per frame
    void Update()
    {
        RotateToFacePlayer();
    }

    private void RotateToFacePlayer()
    {
        // Calculate the vector between the turret position and the player position
        Vector3 relativePosition = player.transform.position - this.transform.position;

        // Rotate the turret object to face the current player position
        float angle = Mathf.Atan2(relativePosition.normalized.y, relativePosition.normalized.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, q, Time.deltaTime * turretRotationSpeed);
    }
}
