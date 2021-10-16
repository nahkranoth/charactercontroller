using System;
using UnityEngine;

public class CollisionBox : MonoBehaviour
{

    public Action<Collision2D> OnCollision;

    private void OnCollisionEnter2D(Collision2D other)
    {
        OnCollision?.Invoke(other);
    }
}