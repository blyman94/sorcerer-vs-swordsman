using Game.Entity;
using UnityEngine;

namespace Game.Animation
{
    public class PlayerAnimEvents : MonoBehaviour
    {
        [SerializeField]
        private Player player;

        public void AE_PrimaryStrike()
        {
            player.MeleeFighter.MeleeWeapon.PrimaryStrike();
        }

        public void AE_SecondaryStrike()
        {
            player.MeleeFighter.MeleeWeapon.SecondaryStrike();
        }

        public void AE_UpwardStrike()
        {
            player.MeleeFighter.MeleeWeapon.UpwardStrike();
        }

        public void AE_SlamStrike()
        {
            player.MeleeFighter.MeleeWeapon.StartSlamStrike();
        }

        public void BT_EndSlamStrike()
        {
            player.MeleeFighter.MeleeWeapon.EndSlamStrike();
        }
    }
}
