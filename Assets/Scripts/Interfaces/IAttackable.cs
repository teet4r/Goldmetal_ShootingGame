public interface IAttackable
{
    public bool IsAttacking { get; }
    public void StartAttack();
    public void StopAttack();
}
