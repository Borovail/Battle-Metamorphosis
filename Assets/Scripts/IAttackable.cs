namespace Assets.Scripts
{
    public interface IAttackable
    {
        public void TakeAttack(AttackInfo attackInfo)
        {
            ApplyDamage(attackInfo);
            ApplyKnockback(attackInfo);
        }
        protected void ApplyDamage(AttackInfo attackInfo);
        protected void ApplyKnockback(AttackInfo attackInfo);

    }
}