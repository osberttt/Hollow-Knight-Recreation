using UnityEngine;

public class KnockBackState : IState
{
    private readonly Enemy enemy;
    private float timer;
    public KnockBackState(Enemy enemy)
    {
        this.enemy = enemy;
    }
    
    public void Enter()
    {
        timer = enemy.knockbackTime;
        enemy.ChangeSpriteColor(Color.red);
    }

    public void Exit()
    {
        enemy.ChangeSpriteColor(Color.white);
    }

    public void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f) enemy.StateMachine.ChangeState(enemy.WalkState);
        else enemy.KnockBack();
    }

    public void FixedUpdate()
    {
        
    }
}
