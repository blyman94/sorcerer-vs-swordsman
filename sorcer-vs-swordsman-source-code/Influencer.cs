using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Entity;

namespace Game
{
    public class Influencer : MonoBehaviour
    {
        public Block SorcererBlock;
        public Block SwordsmanBlock;
        public Block[] GoodEffectsBlocks;
        public Block[] BadEffectsBlocks;

        private Block tempBlock;

        public ParticleSystem InfluenceSystem;

        private void Influence()
        {
            int randomInt = Random.Range(0, 3);
            switch (randomInt)
            {
                case 0:
                    ConnectSorcererGoodConnection(false);
                    break;
                case 1:
                    ConnectSwordsmanBadConnection(false);
                    break;
                case 2:
                    SeverSorcererBadConnection(false);
                    break;
                case 3:
                    SeverSwordsmanGoodConnection(false);
                    break;
            }
        }

        private void PlayInfluenceParticle()
        {
            InfluenceSystem.Play();
        }

        private void Shuffle(Block[] blockArray)
        {
            for (int i = 0; i < blockArray.Length - 1; i++)
            {
                int rnd = Random.Range(i, blockArray.Length);
                tempBlock = blockArray[rnd];
                blockArray[rnd] = blockArray[i];
                blockArray[i] = tempBlock;
            }
        }

        private void SeverSwordsmanGoodConnection(bool all)
        {
            Shuffle(GoodEffectsBlocks);
            foreach (Block effectBlock in GoodEffectsBlocks)
            {
                if (SwordsmanBlock.connectedBlocks.Contains(effectBlock))
                {
                    if (SwordsmanBlock.activeConnection == null)
                    {
                        SwordsmanBlock.SeverConnection(effectBlock);
                        PlayInfluenceParticle();
                        if (!all)
                        {
                            return;
                        }
                    }
                }
            }
        }

        private void ConnectSorcererGoodConnection(bool all)
        {
            Shuffle(GoodEffectsBlocks);
            foreach (Block effectBlock in GoodEffectsBlocks)
            {
                if (!SorcererBlock.connectedBlocks.Contains(effectBlock))
                {
                    if (SorcererBlock.activeConnection == null)
                    {
                        SorcererBlock.Connect(effectBlock);
                        PlayInfluenceParticle();
                        if (!all)
                        {
                            return;
                        }
                    }
                }
            }
        }

        private void SeverSorcererBadConnection(bool all)
        {
            Shuffle(BadEffectsBlocks);
            foreach (Block effectBlock in BadEffectsBlocks)
            {
                if (SorcererBlock.connectedBlocks.Contains(effectBlock))
                {
                    if (SorcererBlock.activeConnection == null)
                    {
                        SorcererBlock.SeverConnection(effectBlock);
                        PlayInfluenceParticle();
                        if (!all)
                        {
                            return;
                        }
                    }
                }
            }
        }

        private void ConnectSwordsmanBadConnection(bool all)
        {
            Shuffle(BadEffectsBlocks);
            foreach (Block effectBlock in BadEffectsBlocks)
            {
                if (!SwordsmanBlock.connectedBlocks.Contains(effectBlock))
                {
                    if (SwordsmanBlock.activeConnection == null)
                    {
                        SwordsmanBlock.Connect(effectBlock);
                        PlayInfluenceParticle();
                        if (!all)
                        {
                            return;
                        }
                    }
                }
            }
        }
    }
}

