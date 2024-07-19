using System;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class AttackInfo
    {
        public int Damage;
        public int PushForce;

        [HideInInspector] public Vector2 AttackPosition;

        public AttackInfo SetAttackPosition(Vector2 attackPosition)
        {
            AttackPosition = attackPosition;
            return this;
        }

        public override string ToString() => $"Damage: {Damage}, push force: {PushForce}, attack position: {AttackPosition}";
    }

}