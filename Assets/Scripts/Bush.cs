using UnityEngine;

public class Bush : MonoBehaviour, IDamageable
{
    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        Debug.Log("Bush destroyed!");
        Destroy(gameObject);
    }
}