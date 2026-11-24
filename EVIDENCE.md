# Week 1 evidence

Keep this short. Bullet points, a small table and one diagram or code trace are enough. This is your checkpoint record, not an extra report. Replace the prompts as you work.

Name:
Exact Unity version:
Checkpoint commit / repository:
Current stage and anything unfinished:

## Initial prediction

Three observations the rival needs, including one it might not have:
The rival needs a way to stop checking for scraps, if it doesn’t find one
 for a whole lap through the area it stops checking and moves back to the depot. It also needs a way to check if the scrap is reachable or not, meaning if the scrap is outside of the play area it can ignore that scrap. And lastly the rival needs to be able to be able to find the shortest path to the scrap, even when seeing a wall infront of itself, because now it looks like it just takes ‘A’ path instead of a short path.

Seeing a pickup across a gap proves / does not prove reachability because: 

It seems like the rival wants every scrap to be reachable, since it never switches target scrap even if it can’t reach the current target. So in the current state, reachability is decided by whether or not the rival can see it. 

One question about NPC behaviour:

Does the rival check for the best path around a blocked area? Or how does I evaluate whether a path is shorter when there is a wall in front of the rival. 

## One complete trace

Use one actual situation. Include the observation data, derived facts, selected task and reason, movement request/result, and how feedback affects a later decision. Identify the responsible code locations. A tiny state diagram can accompany this trace.

## Scenario record

| Scenario | Predicted result | Observed result and short evidence / remaining issue |
|---|---|---|
| Normal loop and restart | | |
| Carrying cargo | | |
| Hidden / remembered pickup | | |
| Target taken | | |
| Unreachable target | | |
| No usable pickup | | |

Supplied test result, with any added tests identified:
Live walkthrough checked by / recording link if used:

## One correction and one independent check

The original weakness and why it happened:
What I changed and why it addresses the cause:
My counterexample or experiment: prediction, observed result, and one limitation:

## Collaboration and help

People I discussed or playtested with:
Code, sources or substantive tools/AI assistance I used, and how I verified them:
If unfinished: what works, exact reproduction steps, and the help I need:

