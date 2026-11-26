using System;
using System.Collections.Generic;
using SalvageSprint.Core;
using UnityEngine;

namespace SalvageSprint
{
    public sealed class PickupSensor : MonoBehaviour
    {
        [SerializeField] private MatchController match;
        [SerializeField, Min(0.01f)] private float sightCheckDisplaySeconds = 0.4f;
        [SerializeField, Min(1)] private float radius = 10;
        public float Radius => radius;

        [Header("Perception debugging")]
        [SerializeField] private bool ShowPerceptionRange;
        [SerializeField] private bool showSightChecks;


        public void Configure(MatchController owner) 
        { 
            match = owner; 
        }
        public Observation Capture(RaceActor actor, int rememberedId, Func<int, bool> blocked)
        {
            var candidates = new List<Candidate>();

            foreach (SalvagePickup item in match.Pickups)
            {
                if (!match.IsAvailable(item.Id)) 
                    continue;

                float distance = MatchController.FlatDistanceSquared(transform.position, item.transform.position);
                bool remembered = item.Id == rememberedId;
                bool inRange = distance <= radius * radius;

                if (!inRange && !remembered) 
                    continue;

                bool visible = false;
                if (inRange)
                {
                    Vector3 eyePosition = transform.position + Vector3.up;
                    Vector3 pickupPosition = item.transform.position + Vector3.up * 0.6f;

                    bool sightObstructed = Physics.Linecast(eyePosition, pickupPosition, match.SightBlockers, QueryTriggerInteraction.Ignore);

                    visible = !sightObstructed;

                    if (showSightChecks)
                        Debug.DrawLine(eyePosition, pickupPosition, visible ? Color.green : Color.red, sightCheckDisplaySeconds, false);
                }
                bool isBlocked = blocked(item.Id);

                // Broad-phase candidates are data to process, not automatically known targets.
                candidates.Add(new Candidate(item.Id, distance, true, visible, remembered, isBlocked));
            }
            return new Observation(actor.Carrying, rememberedId, candidates.ToArray());
        }

        private void OnDrawGizmosSelected()
        {
            if (!ShowPerceptionRange) return;

            const int segments = 64;

            // The sensor measures horizontal distance, so draw an XZ circle
            // Raise it slightly to keep it above the floor.
            Vector3 centre = transform.position + Vector3.up * 0.05f;

            Color previousColour = Gizmos.color;
            Gizmos.color = Color.cyan;

            Vector3 previousPoint = centre + Vector3.right * radius;

            for (int i = 1; i <= segments; i++)
            {
                float angle = i * (2f * Mathf.PI / segments);

                Vector3 nextPoint = centre + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);

                Gizmos.DrawLine(previousPoint, nextPoint);
                previousPoint = nextPoint;
            }

            Gizmos.color = previousColour;
        }
    }
}
