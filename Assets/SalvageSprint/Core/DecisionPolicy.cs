namespace SalvageSprint.Core
{
    // Week 1 assignment starter. Work through README stages in this same project.
    // STAGE 2: distinguish visible candidates from unknown hidden objects.
    // STAGE 3: retain a usable remembered target. STAGE 4: use failed-route feedback.
    // Delivery is supplied so the unmodified game has a complete scoring loop.
    public static class DecisionPolicy
    {
        public const string Label = "Part 1 starter: nearest candidate";

        private static bool IsEligible(Candidate candidate)
        {
            return candidate.Available &&
                (candidate.Visible || candidate.Remembered) &&
                !candidate.Blocked;
        }
        public static AgentFacts Infer(Observation observation)
        {
            if (observation.Carrying)
                return new AgentFacts(true, -1, false);

            foreach (Candidate candidate in observation.Candidates)
            {
                // Deliberately naive: ignores Visible, Remembered and Blocked.
                if (candidate.Id == observation.CurrentTargetId &&
                    IsEligible(candidate))
                    return new AgentFacts(false, candidate.Id, true);
            }

            int nearest = -1;
            float distance = float.PositiveInfinity;

            foreach (Candidate candidate in observation.Candidates)
            {
                // Deliberately naive: ignores Visible, Remembered and Blocked.
                if (IsEligible(candidate) && candidate.DistanceSquared < distance)
                { 
                    nearest = candidate.Id; 
                    distance = candidate.DistanceSquared; 
                }
            }
            return new AgentFacts(false, nearest, false);
        }

        public static Decision Choose(Observation observation) => Choose(Infer(observation));

        public static Decision Choose(AgentFacts facts)
        {
            if (facts.NeedsDelivery)
                return new Decision(TaskKind.Deliver, -1, "Cargo held: return to depot");

            if (!facts.HasUsableTarget)
            {
                return new Decision(TaskKind.Explore, -1, "No known pickup, visit next survey point");
            }

            string reason = facts.RetainedTarget
                ? "Retain current eligible target"
                : "Nearest eligible known pickup";

            return new Decision(TaskKind.Collect, facts.SelectedTargetId, reason);
        }
    }
}
