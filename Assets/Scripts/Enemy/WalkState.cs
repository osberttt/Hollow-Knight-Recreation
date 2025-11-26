using UnityEngine;

public class WalkState : IState
{
    private readonly Enemy enemy;
    
    public WalkState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        if (Physics2D.OverlapCircle(enemy.wallCheck.position, enemy.wallCheckRadius, enemy.wallLayer))
        {
            enemy.ChangeDirection();
        }
        
        enemy.Move();
    }

    public void FixedUpdate()
    {
        
    }
}
