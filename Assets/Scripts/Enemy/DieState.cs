using UnityEngine;

public class DieState : IState
{
    private readonly Enemy enemy;
    
    public DieState(Enemy enemy)
    {
        this.enemy = enemy;
    }
    
    public void Enter()
    {
        enemy.Die();
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
 
    }

    public void FixedUpdate()
    {
        
    }
}
