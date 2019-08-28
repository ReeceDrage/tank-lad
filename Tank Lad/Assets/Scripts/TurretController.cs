using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] float turretRotationSpeed = 10f;
    [SerializeField] float timeBetweenShots = 3f;
    [SerializeField] float shotTimeVariation = 0.2f;
    [SerializeField] private GameObject shot;

    private GameObject player;
    private GameObject firingPoint;
    private bool canShoot = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player Tank");
        firingPoint = this.transform.Find("Fire Point").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        RotateToFacePlayer();
        Shoot();
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

    private void Shoot()
    {
        StartCoroutine(ResetShot());

        if (canShoot == true)
        {
            // Create shot object with the current rotation of the turret. Set the mask property to be this layer mask so it doesn't collide with this object
            GameObject shotFired = Instantiate(shot, firingPoint.transform.position, this.transform.rotation);
            shotFired.GetComponent<CannonShot>().mask = this.gameObject.layer;

            // Prevents repeated shots
            canShoot = false;
        }
    }

    /// <summary>
    /// Resets the canShoot boolean to the initial true value. Allows for shot spacing.
    /// </summary>
    /// <returns></returns>
    IEnumerator ResetShot()
    {
        float variedTime = timeBetweenShots + Random.Range(-shotTimeVariation, shotTimeVariation);
        yield return new WaitForSeconds(variedTime);
        canShoot = true;
    }
}
