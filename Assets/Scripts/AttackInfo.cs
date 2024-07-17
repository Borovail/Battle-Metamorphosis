namespace Assets.Scripts
{
    public class AttackInfo
    {
        public int Damage;

        public AttackInfo(int damage)
        {
            Damage = damage;
        }

        public override string ToString()
        {
            return $"Damage:{Damage}";
        }
    }

}