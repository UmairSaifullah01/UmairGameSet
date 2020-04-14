using System;
using UnityEngine;
namespace UMGS
{
    public class UJCollision : MonoBehaviour
    {
        public Action<Collision> enter, stay, exit;
        private void OnCollisionEnter (Collision other)
        {
            if (enter != null)
                enter.Invoke (other);
        }
        private void OnCollisionStay (Collision other)
        {
            if (stay != null)
                stay.Invoke (other);
        }

        private void OnCollisionExit (Collision other)
        {
            if (exit != null)
                exit.Invoke (other);
        }
    }

    public static class UMCollision
    {
        public static void OnCollisionEnter (this Collider col, Action<Collision> trigger)
        {
            UJCollision trgr = col.GetComponent<UJCollision> ();
            if (trgr == null)
            {
                trgr = col.gameObject.AddComponent<UJCollision> ();
            }
            trgr.hideFlags = HideFlags.HideInInspector;
            trgr.enter = trigger;
        }
        public static void OnCollisionStay (this Collider col, Action<Collision> trigger)
        {
            UJCollision trgr = col.GetComponent<UJCollision> ();
            if (trgr == null)
            {
                trgr = col.gameObject.AddComponent<UJCollision> ();
            }
            trgr.hideFlags = HideFlags.HideInInspector;
            trgr.stay = trigger;
        }

        public static void OnCollisionExit (this Collider col, Action<Collision> trigger)
        {
            UJCollision trgr = col.GetComponent<UJCollision> ();
            if (trgr == null)
            {
                trgr = col.gameObject.AddComponent<UJCollision> ();
            }
            trgr.hideFlags = HideFlags.HideInInspector;
            trgr.exit = trigger;
        }
    }
}