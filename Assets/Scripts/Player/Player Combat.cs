using System;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Transform forwardAttackPoint;
    [SerializeField] private Transform upwardAttackPoint;
    [SerializeField] private Transform downwardAttackPoint;
    
    [Header("Prefabs")] 
    [SerializeField] private GameObject forwardEffect;
    [SerializeField] private GameObject upwardEffect;
    [SerializeField] private GameObject downwardEffect;
    
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public Vector2 attackDir;
    
    private PlayerController playerController;
    private float attackBufferTimer;
    private float attackCooldownTimer;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        GatherInput();
    }

    private void GatherInput()
    {
        // --- Attack Input Buffer ---
        if (Input.GetButtonDown("Attack"))
            attackBufferTimer = playerController.stats.attackBufferTime;
        else
            attackBufferTimer -= Time.deltaTime;

        attackCooldownTimer -= Time.deltaTime;

        if (attackBufferTimer > 0 && attackCooldownTimer <= 0)
        {
            PerformAttack();
            attackBufferTimer = 0;
        }
    }
    
    void PerformAttack()
    {
        isAttacking = true;
        attackCooldownTimer = playerController.stats.attackCooldown;

        // Decide attack direction
        attackDir = Vector2.right * playerController.facingDir;
        var attackPoint = forwardAttackPoint;
        var attackEffect = forwardEffect;
        if (Input.GetAxisRaw("Vertical") > 0.5f)
        {
            attackDir = Vector2.up;
            attackPoint = upwardAttackPoint;
            attackEffect = upwardEffect;
        }
        else if (Input.GetAxisRaw("Vertical") < -0.5f)
        {
            attackDir = Vector2.down;
            attackPoint = downwardAttackPoint;
            attackEffect = downwardEffect;
        }

        // Spawn attackEffect
        var effectObj = Instantiate(attackEffect, attackPoint.position, attackEffect.transform.rotation, attackPoint);
        var hitBox = effectObj.GetComponentInChildren<AttackHitbox>();
        hitBox.attackDir = attackDir;
        Invoke(nameof(AttackEnd), hitBox.lifetime);
        Destroy(effectObj, hitBox.lifetime);
    }

    void AttackEnd()
    {
        isAttacking = false;
    }
}
