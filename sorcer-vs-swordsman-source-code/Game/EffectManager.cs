using Game.Entity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
    public class EffectManager : Singleton<EffectManager>
    {
        [Header("Entities")]
        public Player player;
        public Sorcerer sorcerer;

        [Header("Stats Display Texts")]
        public TextMeshProUGUI PlayerDamageDisplayText;
        public TextMeshProUGUI SorcererDamageDisplayText;
        public TextMeshProUGUI PlayerSpeedDisplayText;
        public TextMeshProUGUI SorcererSpeedDisplayText;

        [Header("Effect Group Parameters")]
        public CanvasGroup DescribeEffectGroup;
        public TextMeshProUGUI EffectDescriptionText;
        public float ShowTime;
        public float FadeTime;

        [Header("Audio")]
        public AudioSource BuffAudio;
        public AudioClip PlayerBuffClip;
        public AudioClip SorcBuffClip;
        public AudioClip PlayerDebuffClip;
        public AudioClip SorcDebuffClip;

        public void Start()
        {
            DescribeEffectGroup.alpha = 0;
            DescribeEffectGroup.interactable = false;
            DescribeEffectGroup.blocksRaycasts = false;
        }

        public void PlayAudio(bool isPlayer, bool isBuff)
        {
            if (isPlayer && isBuff)
            {
                BuffAudio.PlayOneShot(PlayerBuffClip, 0.75f);
            }
            else if (isPlayer && !isBuff)
            {
                BuffAudio.PlayOneShot(PlayerDebuffClip, 0.75f);
            }
            else if (!isPlayer && isBuff)
            {
                BuffAudio.PlayOneShot(SorcBuffClip, 0.75f);
            }
            else
            {
                BuffAudio.PlayOneShot(SorcDebuffClip, 0.75f);
            }
        }

        public void StartEffect(Block entityBlock, Block effectBlock)
        {
            if (entityBlock.EntityType == EntityType.Swordsman)
            {
                ApplyEffectToPlayer(effectBlock.Effect);
            }
            if (entityBlock.EntityType == EntityType.Sorcerer)
            {
                ApplyEffectToSorcerer(effectBlock.Effect);
            }
        }

        public void EndEffect(Block entityBlock, Block effectBlock)
        {
            if (entityBlock.EntityType == EntityType.Swordsman)
            {
                RemoveEffectFromPlayer(effectBlock.Effect);
            }
            if (entityBlock.EntityType == EntityType.Sorcerer)
            {
                RemoveEffectFromSorcerer(effectBlock.Effect);
            }
        }

        private void ApplyEffectToPlayer(Effect effect)
        {
            switch (effect)
            {
                case Effect.AttackPowerUp:
                    player.PlayerStats.Damage += 5;
                    EffectDescriptionText.color = Color.cyan;
                    ShowDescribeEffectGroup("Swordsman Damage +5!");
                    PlayAudio(true, true);
                    break;
                case Effect.AttackPowerDown:
                    player.PlayerStats.Damage -= 5;
                    EffectDescriptionText.color = Color.yellow;
                    ShowDescribeEffectGroup("Swordsman Damage -5!");
                    PlayAudio(true, false);
                    break;
                case Effect.SpeedUp:
                    player.PlayerStats.MoveSpeed += 5;
                    EffectDescriptionText.color = Color.cyan;
                    ShowDescribeEffectGroup("Swordsman Speed +5!");
                    PlayAudio(true, true);
                    break;
                case Effect.SpeedDown:
                    player.PlayerStats.MoveSpeed -= 5;
                    EffectDescriptionText.color = Color.yellow;
                    ShowDescribeEffectGroup("Swordsman Speed -5!");
                    PlayAudio(true, false);
                    break;
                case Effect.Fire:
                    player.Ignite();
                    EffectDescriptionText.color = Color.yellow;
                    ShowDescribeEffectGroup("Swordsman On Fire!");
                    PlayAudio(true, false);
                    break;
                default:
                    Debug.Log("{{EffectManager.cs}} Unrecognized " +
                    "Effect passed to ApplyEffectToPlayer.");
                    break;
            }
            player.UpdateMoveSpeed();
            PlayerDamageDisplayText.text = player.PlayerStats.Damage.ToString();
            PlayerSpeedDisplayText.text = player.PlayerStats.MoveSpeed.ToString();
        }

        private void RemoveEffectFromPlayer(Effect effect)
        {
            switch (effect)
            {
                case Effect.AttackPowerUp:
                    player.PlayerStats.Damage -= 5;
                    EffectDescriptionText.color = Color.yellow;
                    ShowDescribeEffectGroup("Swordsman Damage -5!");
                    PlayAudio(true, false);
                    break;
                case Effect.AttackPowerDown:
                    player.PlayerStats.Damage += 5;
                    EffectDescriptionText.color = Color.cyan;
                    ShowDescribeEffectGroup("Swordsman Damage +5!");
                    PlayAudio(true, true);
                    break;
                case Effect.SpeedUp:
                    player.PlayerStats.MoveSpeed -= 5;
                    EffectDescriptionText.color = Color.yellow;
                    ShowDescribeEffectGroup("Swordsman Speed -5!");
                    PlayAudio(true, false);
                    break;
                case Effect.SpeedDown:
                    player.PlayerStats.MoveSpeed += 5;
                    EffectDescriptionText.color = Color.cyan;
                    ShowDescribeEffectGroup("Swordsman Speed +5!");
                    PlayAudio(true, true);
                    break;
                case Effect.Fire:
                    player.Snuff();
                    EffectDescriptionText.color = Color.cyan;
                    ShowDescribeEffectGroup("Swordsman Not On Fire!");
                    PlayAudio(true, false);
                    break;
                default:
                    Debug.Log("{{EffectManager.cs}} Unrecognized " +
                    "Effect passed to ApplyEffectToPlayer.");
                    break;
            }
            player.UpdateMoveSpeed();
            PlayerDamageDisplayText.text = player.PlayerStats.Damage.ToString();
            PlayerSpeedDisplayText.text = player.PlayerStats.MoveSpeed.ToString();
        }

        private void ApplyEffectToSorcerer(Effect effect)
        {
            switch (effect)
            {
                case Effect.AttackPowerUp:
                    sorcerer.SorcererStats.Damage += 5;
                    EffectDescriptionText.color = Color.yellow;
                    ShowDescribeEffectGroup("Sorcerer Damage +5!");
                    PlayAudio(false, true);
                    break;
                case Effect.AttackPowerDown:
                    sorcerer.SorcererStats.Damage -= 5;
                    EffectDescriptionText.color = Color.cyan;
                    ShowDescribeEffectGroup("Sorcerer Damage -5!");
                    PlayAudio(false, false);
                    break;
                case Effect.SpeedUp:
                    sorcerer.SorcererStats.MoveSpeed += 5;
                    EffectDescriptionText.color = Color.yellow;
                    ShowDescribeEffectGroup("Sorcerer Speed +5!");
                    PlayAudio(false, true);
                    break;
                case Effect.SpeedDown:
                    sorcerer.SorcererStats.MoveSpeed -= 5;
                    EffectDescriptionText.color = Color.cyan;
                    ShowDescribeEffectGroup("Sorcerer Speed -5!");
                    PlayAudio(false, false);
                    break;
                case Effect.Fire:
                    sorcerer.Ignite();
                    EffectDescriptionText.color = Color.cyan;
                    ShowDescribeEffectGroup("Sorcerer On Fire!");
                    PlayAudio(false, false);
                    break;
                default:
                    Debug.Log("{{EffectManager.cs}} Unrecognized " +
                    "Effect passed to ApplyEffectToSorcerer.");
                    break;
            }
            sorcerer.UpdateMoveSpeed();
            SorcererDamageDisplayText.text = sorcerer.SorcererStats.Damage.ToString();
            SorcererSpeedDisplayText.text = sorcerer.SorcererStats.MoveSpeed.ToString();
        }
        private void RemoveEffectFromSorcerer(Effect effect)
        {
            switch (effect)
            {
                case Effect.AttackPowerUp:
                    sorcerer.SorcererStats.Damage -= 5;
                    EffectDescriptionText.color = Color.cyan;
                    ShowDescribeEffectGroup("Sorcerer Damage -5!");
                    PlayAudio(false, false);
                    break;
                case Effect.AttackPowerDown:
                    sorcerer.SorcererStats.Damage += 5;
                    EffectDescriptionText.color = Color.yellow;
                    ShowDescribeEffectGroup("Sorcerer Damage +5!");
                    PlayAudio(false, true);
                    break;
                case Effect.SpeedUp:
                    sorcerer.SorcererStats.MoveSpeed -= 5;
                    EffectDescriptionText.color = Color.cyan;
                    ShowDescribeEffectGroup("Sorcerer Speed -5!");
                    PlayAudio(false, false);
                    break;
                case Effect.SpeedDown:
                    sorcerer.SorcererStats.MoveSpeed += 5;
                    EffectDescriptionText.color = Color.yellow;
                    ShowDescribeEffectGroup("Sorcerer Speed  +5!");
                    PlayAudio(false, true);
                    break;
                case Effect.Fire:
                    sorcerer.Snuff();
                    EffectDescriptionText.color = Color.yellow;
                    ShowDescribeEffectGroup("Sorcerer Not On Fire!");
                    PlayAudio(false, true);
                    break;
                default:
                    Debug.Log("{{EffectManager.cs}} Unrecognized " +
                    "Effect passed to RemoveEffectFromSorcerer.");
                    break;
            }
            sorcerer.UpdateMoveSpeed();
            SorcererDamageDisplayText.text = sorcerer.SorcererStats.Damage.ToString();
            SorcererSpeedDisplayText.text = sorcerer.SorcererStats.MoveSpeed.ToString();
        }

        public void ShowDescribeEffectGroup(string text)
        {
            StopAllCoroutines();
            UpdateDescription(text);
            DescribeEffectGroup.alpha = 1f;
            StartCoroutine(HideDescribeEffectGroup());
        }

        public IEnumerator HideDescribeEffectGroup()
        {
            yield return new WaitForSeconds(ShowTime);

            float elapsedTime = 0.0f;
            while (elapsedTime < FadeTime)
            {
                float alpha = Mathf.Lerp(1, 0, elapsedTime / FadeTime);
                DescribeEffectGroup.alpha = alpha;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            DescribeEffectGroup.alpha = 0;
        }

        public void UpdateDescription(string text)
        {
            EffectDescriptionText.text = text;
        }
    }
}

