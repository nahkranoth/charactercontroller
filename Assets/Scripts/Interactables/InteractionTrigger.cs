using UnityEngine;

public class InteractionTrigger:MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Weapon")
        {
            Debug.Log("Hit By Weapon");
        }
    }
}
