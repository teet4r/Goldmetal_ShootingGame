public interface IDamageable
{
    public bool IsDead { get; }
    public int MaxHp { get; }
    public void GetDamage(int amount);
}
