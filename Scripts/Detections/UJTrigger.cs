using System;
using UnityEngine;
namespace UMGS
{
    public class UJTrigger : MonoBehaviour
    {
        public Action<Collider> enter, stay, exit;
        private void OnTriggerEnter (Collider other)
        {
            if (enter != null)
                enter.Invoke (other);
        }
        private void OnTriggerStay (Collider other)
        {
            if (stay != null)
                stay.Invoke (other);
        }

        private void OnTriggerExit (Collider other)
        {
            if (exit != null)
                exit.Invoke (other);
        }
    }

    public static class UMTrigger
    {
        public static void OnTriggerEnter (this Collider col, Action<Collider> trigger)
        {
            UJTrigger trgr = col.GetComponent<UJTrigger> ();
            if (trgr == null)
            {
                trgr = col.gameObject.AddComponent<UJTrigger> ();
            }
            trgr.hideFlags = HideFlags.HideInInspector;
            trgr.enter = trigger;
        }
        public static void OnTriggerStay (this Collider col, Action<Collider> trigger)
        {
            UJTrigger trgr = col.GetComponent<UJTrigger> ();
            if (trgr == null)
            {
                trgr = col.gameObject.AddComponent<UJTrigger> ();
            }
            trgr.hideFlags = HideFlags.HideInInspector;
            trgr.stay = trigger;
        }

        public static void OnTriggerExit (this Collider col, Action<Collider> trigger)
        {
            UJTrigger trgr = col.GetComponent<UJTrigger> ();
            if (trgr == null)
            {
                trgr = col.gameObject.AddComponent<UJTrigger> ();
            }
            trgr.hideFlags = HideFlags.HideInInspector;
            trgr.exit = trigger;
        }
    }
}