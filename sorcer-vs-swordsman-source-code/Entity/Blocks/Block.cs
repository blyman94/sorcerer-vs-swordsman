using Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entity
{
    public enum EntityType { Sorcerer, Swordsman, None, Default }
    public enum Effect { AttackPowerUp, AttackPowerDown, SpeedUp, SpeedDown, Fire, None, Default }
    public class Block : MonoBehaviour
    {

        public bool isEntity;
        public EntityType EntityType;
        public bool isDebuff;
        public Effect Effect;
        public List<LineRenderer> connections;

        public bool isConnectionStarted;

        public LineRenderer activeConnection = null;
        public int activeConnectionIndex = 0;

        public List<Block> connectedBlocks;

        private void Start()
        {
            connectedBlocks = new List<Block>();
            for (int i = 0; i < connections.Count; i++)
            {
                connectedBlocks.Add(null);
            }
        }

        private void Update()
        {
            if (isConnectionStarted)
            {
                activeConnection.SetPosition(1, PlayerPosition.Current);
            }
        }

        public bool StartConnection(bool sorcerer)
        {
            if (isEntity)
            {
                LineRenderer newConnection;
                int connectionIndex;
                GetConnection(out newConnection, out connectionIndex);

                if (newConnection == null)
                {
                    Debug.Log("Failed to create connection: Max connection count reached.");
                    return false;
                }

                isConnectionStarted = true;

                newConnection.positionCount = 2;
                newConnection.SetPosition(0, transform.position);

                if (!sorcerer)
                {
                    newConnection.SetPosition(1, PlayerPosition.Current);
                }

                activeConnection = newConnection;
                activeConnectionIndex = connectionIndex;

                activeConnection.startColor = Color.white;
                activeConnection.endColor = Color.white;

                return true;
            }
            else
            {
                return false;
            };
        }

        public void SeverConnection(LineRenderer connection)
        {
            int connectionIndex = connections.IndexOf(connection);
            Destroy(connection.GetComponent<PolygonCollider2D>());
            EffectManager.Instance.EndEffect(this,
                connectedBlocks[connectionIndex]);
            connection.positionCount = 0;
            connectedBlocks[connectionIndex] = null;
        }

        public void SeverConnection(Block effectBlock)
        {
            int blockIndex = connectedBlocks.IndexOf(effectBlock);
            SeverConnection(connections[blockIndex]);
        }

        public void Connect(Block effectBlock)
        {
            bool connectionSuccessful = StartConnection(true);
            if (connectionSuccessful)
            {
                effectBlock.CloseConnection(this);
            }
        }

        public bool CloseConnection(Block startingBlock)
        {
            if (startingBlock.connectedBlocks.Contains(this))
            {
                Debug.Log("Already connected to this block!");
                return false;
            }
            if (!isEntity)
            {
                startingBlock.activeConnection.SetPosition(1, transform.position);

                PolygonCollider2D polygonCollider2D = startingBlock.activeConnection.gameObject.AddComponent<PolygonCollider2D>();
                polygonCollider2D.pathCount = 1;

                float halfWidth = startingBlock.activeConnection.startWidth * 0.5f;

                Vector2 dir = (startingBlock.transform.localPosition - transform.localPosition).normalized;
                Vector2 perp = new Vector2(dir.y, -dir.x);

                Vector2 point1 = new Vector2(startingBlock.transform.localPosition.x, startingBlock.transform.localPosition.y) + (perp * halfWidth);
                Vector2 point2 = new Vector2(transform.localPosition.x, transform.localPosition.y) + (perp * halfWidth);
                Vector2 point3 = new Vector2(transform.localPosition.x, transform.localPosition.y) - (perp * halfWidth);
                Vector2 point4 = new Vector2(startingBlock.transform.localPosition.x, startingBlock.transform.localPosition.y) - (perp * halfWidth);

                Vector2[] path = new Vector2[5] { point1, point2, point3, point4, point1 };
                polygonCollider2D.SetPath(0, path);
                polygonCollider2D.offset = new Vector2(-startingBlock.transform.localPosition.x, -startingBlock.transform.localPosition.y);

                startingBlock.connectedBlocks[startingBlock.activeConnectionIndex] = this;

                if ((startingBlock.EntityType == EntityType.Sorcerer && this.isDebuff) ||
                    (startingBlock.EntityType == EntityType.Swordsman && !this.isDebuff))
                {
                    startingBlock.activeConnection.startColor = Color.cyan;
                    startingBlock.activeConnection.endColor = Color.cyan;
                }

                if ((startingBlock.EntityType == EntityType.Sorcerer && !this.isDebuff) ||
                    (startingBlock.EntityType == EntityType.Swordsman && this.isDebuff))
                {
                    startingBlock.activeConnection.startColor = Color.yellow;
                    startingBlock.activeConnection.endColor = Color.yellow;
                }

                startingBlock.activeConnection = null;
                startingBlock.activeConnectionIndex = -1;
                startingBlock.isConnectionStarted = false;

                EffectManager.Instance.StartEffect(startingBlock, this);
                return true;
            }
            else
            {
                Debug.Log("Cant end connection on an entity block");
                return false;
            }
        }

        private void GetConnection(out LineRenderer connection, out int index)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                if (connections[i].positionCount == 0)
                {
                    connection = connections[i];
                    index = i;
                    return;
                }
            }
            connection = null;
            index = -1;
        }
    }
}

