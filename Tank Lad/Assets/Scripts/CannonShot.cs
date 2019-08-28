using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShot : MonoBehaviour
{
    [SerializeField] private float shotVelocity = 15f;
    [SerializeField] private GameObject smoke;
    [SerializeField] public LayerMask mask;

    private Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        rigidbody.velocity = this.transform.up * shotVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != mask)
        {
            print("Shot hit object. Destroying shot.");
            Instantiate(smoke, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
