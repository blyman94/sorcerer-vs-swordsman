using Game.Stats;
using UnityEngine;

namespace Game.Combat.Utilities
{
    public delegate void AttackStarted(string attackType);
    public delegate void AttackSuccessful(bool grounded);
    public delegate void DamageTaken();
    public delegate void Died();
    public delegate void DisableMove(float duration);
    public delegate void RequestDamagePopup(Vector3 origin, int damage, bool player);
}

