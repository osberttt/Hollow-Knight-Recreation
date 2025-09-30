using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;
    private int currentAnim;

    // References to movement state
    private PlayerController player;
    private RuntimeAnimatorController ac;
    
    // 🔹 Animation Hashes
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int WalkStart = Animator.StringToHash("Walk_Start");
    private static readonly int JumpRise = Animator.StringToHash("Jump_Rise");
    private static readonly int JumpFall = Animator.StringToHash("Jump_Fall");
    private static readonly int JumpFallStart = Animator.StringToHash("Jump_Fall_Start");
    private static readonly int JumpLand = Animator.StringToHash("Jump_Land");
    private static readonly int Dash = Animator.StringToHash("Dash");
    private static readonly int AttackForward = Animator.StringToHash("Attack_Forward");
    private static readonly int AttackUp = Animator.StringToHash("Attack_Up");
    private static readonly int AttackDown = Animator.StringToHash("Attack_Down");
    private static readonly int Stop = Animator.StringToHash("Stop");
    private static readonly int Rotate = Animator.StringToHash("Rotate");

    private float m_landAnimTime;
    private bool m_isLanding;

    private float m_walkStartAnimTime;
    private bool m_isStartingWalk;
    
    private float m_jumpFallStartAnimTime;
    private bool m_isStartingFall;
    
    private float m_stopAnimTime;
    private bool m_isStopping;
    
    private float m_rotateAnimTime;
    private bool m_isRotating;
    
    

    void Awake()
    {
        player = GetComponent<PlayerController>();
        ac = animator.runtimeAnimatorController;
    }

    private void Start()
    {
        m_landAnimTime = GetClipLength("Jump_Land");
        m_walkStartAnimTime = GetClipLength("Walk_Start");
        m_jumpFallStartAnimTime = GetClipLength("Jump_Fall_Start");
        m_stopAnimTime = GetClipLength("Stop");
        m_rotateAnimTime = GetClipLength("Rotate");
    }

    void Update()
    {
        int nextAnim = DecideAnimation();

        if (nextAnim != currentAnim)
            PlayAnim(nextAnim);
    }

    int DecideAnimation()
    {
        // Attack overrides everything
        if (player.isAttacking)
        {
            if (player.attackDir == Vector2.up)
                return AttackUp;
            else if (player.attackDir == Vector2.down)
                return AttackDown;
            else
                return AttackForward;
        }

        // Dash overrides movement
        if (player.isDashing)
            return Dash;

        // Jump states
        if (!player.isGrounded)
        {
            if (player.rb.linearVelocity.y > 0.1f)
                return JumpRise;
            else if (player.rb.linearVelocity.y < -0.1f)
                return JumpFall;
        }
        else
        {
            // Just landed
            if (player.landedThisFrame)
            {
                m_isLanding = true;
                Invoke(nameof(ClearLandedFlag), m_landAnimTime);
                return JumpLand;
            }
            if (m_isLanding) return JumpLand;
            
            // Ground movement
            if (Mathf.Abs(player.moveInput) > 0.1f)
            {
                if (player.walkStartThisFrame)
                {
                    m_isStartingWalk = true;
                    Invoke(nameof(ClearWalkStartFlag), m_walkStartAnimTime);
                    return WalkStart;
                }
                return m_isStartingWalk ? WalkStart : Walk;
            }
            else
            {
                if (player.walkStopThisFrame)
                {
                    m_isStopping = true;
                    Invoke(nameof(ClearStopFlag), m_stopAnimTime);
                    return Stop;
                }
                return m_isStopping ? Stop : Idle;
            }
        }

        return Idle;
    }

    void PlayAnim(int anim)
    {
        animator.Play(anim);
        currentAnim = anim;
    }

    void ClearLandedFlag()
    {
        m_isLanding = false;
    }

    void ClearWalkStartFlag()
    {
        m_isStartingWalk = false;
    }

    void ClearStopFlag()
    {
        m_isStopping = false;
    }
    
    // Get animation clip length by name
    public float GetClipLength(string clipName)
    {
        foreach (AnimationClip clip in ac.animationClips)
        {
            if (clip.name == clipName)
            {
                return clip.length;
            }
        }
        Debug.LogWarning($"Clip {clipName} not found!");
        return 0f;
    }
}
