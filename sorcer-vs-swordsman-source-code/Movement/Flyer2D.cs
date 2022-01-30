using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Movement
{
    public class Flyer2D : MonoBehaviour
    {
        // Movement Fields
        private Vector2 velocity;
		public Rigidbody2D rb2d;
		public float moveSpeed = 400;
        public float accelAir = 0.1f;
        private float velocityXSmoothing;
        private float velocityYSmoothing;

        public void AddForce(Vector2 force)
        {
            rb2d.AddForce(force * moveSpeed);
        }
    }
}

