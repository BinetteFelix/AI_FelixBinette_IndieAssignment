Week 1 evidence

Keep this short. Bullet points, a small table and one diagram or code trace are enough. This is your checkpoint record, not an extra report. Replace the prompts as you work.

Name: Felix Binette
Exact Unity version: 6000.3.26f1
Checkpoint commit / repository: 
Checkpoint Commit name: “Forgot to save scene in last commit...”, 
Commit ID: ef9dcb55416be6f37da29d544b3ada928f88fd07, 
Repo link: https://github.com/BinetteFelix/AI_FelixBinette_IndieAssignment

Current stage and anything unfinished: Stage 5 and I have not entered the change to the correction I made or how it is related to the cause. Or a counter example / experiment.

Initial prediction

Three observations the rival needs, including one it might not have:

The rival needs a way to stop checking for scraps, if it doesn’t find one
for a whole lap through the area, it stops checking and moves back to the depot. It also needs a way to check if the scrap is reachable or not, meaning if the scrap is outside of the play area it can ignore that scrap. And lastly the rival needs to be able to be able to find the shortest path to the scrap, even when seeing a wall in front of itself, because now it looks like it just takes ‘A’ path instead of a short path.


Seeing a pickup across a gap proves / does not prove reachability because:

It seems like the rival wants every scrap to be reachable, since it never switches target scrap even if it can’t reach the current target. So in the current state, reachability is decided by whether the rival can see it.

One question about NPC behavior:

How do the bots in Rocket League decide when to shoot the ball and when to defend its net? 

One complete trace

Use one actual situation. Include the observation data, derived facts, selected task and reason, movement request/result, and how feedback affects a later decision. Identify the responsible code locations. A tiny state diagram can accompany this trace.

If the rival sees one scrap (8) and sets its current task to ‘Collect’ then it will never switch from that task unless the scrap is picked up by the player instead. If the rival sees another scrap (9) that is closer, the rival will ignore it and continue walking toward the scrap (8) it saw before. 

In the rival brain it checks for the next decision by taking a bunch of steps; first by observing using the pick-up sensor, which looks for scraps within the pick-up range. Then the brain uses the last observation to get the current facts by calling the DecisionPolicy.Infer(LastObservation), then gets the next decision by using DecisionPolicy.Choose(CurrentFacts). The rival messes up which scrap is closest by never checking which scrap is closest, it only  gets the first scrap that the rival can find a complete route to and can take the quickest/shortest path to get to said scrap. The rival therefore is kind of dumb in a way, because it doesn’t know which scrap is the closest, only which one it sees first. It may be dumb because of this but that may not be the purpose of the AI. Having the rival keep its target ensures that it  actually is able to grab that scrap, because the rival checks if the current route is complete, meaning it has found a route from the rival’s current position to the scrap and is in pursuit of said scrap. If the rival would see a new scrap around a wall, the new scrap may be unreachable, being across a gap or above some sort of ledge, this would cause the rival to do a check on the new scraps and it would first stand still because the new scraps were unreachable or unviable and then start exploring around and then eventually grabbing another scrap that is reachable. This whole process is skipped by making the rival retain its target once found a complete path to a scrap, thus making the rival’s path finding and scrap collecting more efficient and stable. 

Visible: 3, Travel: Moving, Task: Collect, Cargo: false, target: 8, blocked: 0. 

* Explore ? Collect when an eligible pickup is selected.
* Collect ? Deliver after successful collection gives cargo.
* Deliver ? Collect or Explore after delivery, depending on available knowledge.
* Collect ? another Collect target or Explore when the current target becomes unusable.


Scenario record

| Scenario | Predicted result | Observed result and short evidence / remaining issue |
|---|---|---|
| Normal loop and restart | The rival observes the surrounding area and collects scraps based on their distance from the rival, on restart the match restarts with restart attributes (scores, time, scraps are reset to their original states) | The rival collects everything within the visible area, when the rival has set its mind on collecting a scrap, the rival can’t change its mind, it keeps going toward the same scrap even if it sees a closer scrap and once it has delivered the scrap the counter for how many scraps the rival has delivered goes up. |
| Carrying cargo | The rival brings the already carried scrap to its depot and deposits it, and keeps collecting scrap until it wins | The rival carried the scrap picked up in start back to its depot and kept looking for  more scraps, just like in the first scenario |
| Hidden / remembered pickup | The rival ignores the blocked/hidden scrap and sets a visible but more distant scrap as the new target | before bugfix: the rival stood still until the blocked/hidden scrap was picked up by the player, then went and picked up the more distant but visible scrap. After bugfix: the rival completely ignores the blocked/hidden scrap and immediately recognizes the more distant scrap as the current target since it is the closest non-blocked/hidden scrap |
| Target taken | The rival recognizes the scrap as non-existent and looks for a different scrap closest to itself | the rival immediately turned around and targeted the closest scrap second to the stolen scrap |
| Unreachable target | the rival does the same as for a blocked/hidden scrap | Before bugfix: the rival stood still and didn’t want to go to any scraps at all, and since the player couldn’t reach the unreachable scrap either it could never be stolen, and so the rival stood still forever. After bugfix: the rival stands still for a second then turns and starts going toward the next survey point (exploring) and when reached the next survey point, started to look for more scraps and found a different scrap and collected it |
| No usable pickup | The rival looks around a bit before realizing that there are no scraps | The rival walks around the play area indefinitely, meaning it keeps patrolling around |

Supplied test result, with any added tests identified:
Live walkthrough checked by / recording link if used:
Recording link Is supplied in the Omniway submission.

One correction and one independent check

The original weakness and why it happened: 
Before, the rival got a little bit of “decision anxiety”, meaning when there was a scrap behind a wall and that scrap was the first scrap that the rival saw, it set that scrap as its target and “tried” to walk towards it and inevitably failed completely since it decided to stand completely still. Why this happened was because it never registered the scrap as functionally blocked or non-visible, causing it to break because if something is marked as blocked, the rival can’t walk towards it, but if it is visible, it can, meaning there was some kind of logic complication there. 
What I changed and why it addresses the cause:
My counterexample or experiment: prediction, observed result, and one limitation:

Collaboration and help

People I discussed or playtested with: Emil Rotmark, Casper Aaröe

Code, sources or substantive tools/AI assistance I used, and how I verified them: No AI or sources really, we only looked at the supplied scripts, like DecisionPolicy.cs, RivalBrain.cs, Navigator.cs, etc.

If unfinished: what works, exact reproduction steps, and the help I need:

The AI can decide if the current target scrap is the closest and considered reachable but isn’t and, meaning it will stand still and basically just look at the scrap for a few seconds then it will move on because it doesn’t seem to be able to reach it
