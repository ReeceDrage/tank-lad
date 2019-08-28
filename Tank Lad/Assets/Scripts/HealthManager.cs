using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int actorHealth;

    private void ApplyDamage(int damage)
    {
        actorHealth -= damage;

        if (actorHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
