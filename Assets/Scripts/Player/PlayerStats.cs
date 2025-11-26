using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Header("Movement")]
    public float groundSpeed = 8f;
    public float airSpeed = 5f;

    [Header("Gravity")] 
    public float gravityScale = 3f;
    public float fallMultiplier = 1.5f;

    [Header("Jumping")]
    public float jumpForce = 15f;
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;

    [Header("Dash")]
    public float dashForce = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.3f;

    [Header("Attacking")]
    public float attackCooldown = 0.3f;
    public float attackBufferTime = 0.15f;
    public float pogoBounceForce = 12f;
    public LayerMask enemyLayers;

    [Header("Checks")]
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;
    public Color groundedGizmosColor = Color.green;
    public Color onAirGizmosColor = Color.yellow;
}
