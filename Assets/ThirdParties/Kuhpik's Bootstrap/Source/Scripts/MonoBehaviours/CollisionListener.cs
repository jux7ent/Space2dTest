using System;
using UnityEngine;

namespace Kuhpik
{
    public class CollisionListener : MonoBehaviour
    {
        public event Action<Transform> CollisionEnterEvent, CollisionExitEvent;
        public event Action<Transform, Transform> TriggerEnterEvent, TriggerStayEvent, TriggerExitEvent; // owner, other

        private void OnCollisionEnter(Collision collision)
        {
            CollisionEnterEvent?.Invoke(collision.transform);
        }

        private void OnCollisionExit(Collision collision)
        {
            CollisionExitEvent?.Invoke(collision.transform);
        }

        private void OnTriggerEnter(Collider other)
        {
            TriggerEnterEvent?.Invoke(transform, other.transform);
        }

        private void OnTriggerStay(Collider other) {
            //TriggerStayEvent?.Invoke(other.transform);
        }

        private void OnTriggerExit(Collider other)
        {
            TriggerExitEvent?.Invoke(transform, other.transform);
        }
    }
}