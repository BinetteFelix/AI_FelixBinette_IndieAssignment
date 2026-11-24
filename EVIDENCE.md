# Week 1 evidence

Keep this short. Bullet points, a small table and one diagram or code trace are enough. This is your checkpoint record, not an extra report. Replace the prompts as you work.

Name: Felix Binette
Exact Unity version: 6000.3.26f1
Checkpoint commit / repository:
Current stage and anything unfinished:

## Initial prediction

Three observations the rival needs, including one it might not have:

The rival needs a way to stop checking for scraps, if it doesn’t find one
for a whole lap through the area it stops checking and moves back to the depot. It also needs a way to check if the scrap is reachable or not, meaning if the scrap is outside of the play area it can ignore that scrap. And lastly the rival needs to be able to be able to find the shortest path to the scrap, even when seeing a wall in front of itself, because now it looks like it just takes ?A? path instead of a short path.


Seeing a pickup across a gap proves / does not prove reachability because:

It seems like the rival wants every scrap to be reachable, since it never switches target scrap even if it can’t reach the current target. So in the current state, reachability is decided by whether the rival can see it.

One question about NPC behavior:

How do the bots in Rocket League decide when to shoot the ball and when to defend its net? 

## One complete trace

Use one actual situation. Include the observation data, derived facts, selected task and reason, movement request/result, and how feedback affects a later decision. Identify the responsible code locations. A tiny state diagram can accompany this trace.

If the rival picks up scrap ‘7, 10, 11, 12’, and the player picks up scrap ‘8’ and then picks up scrap ‘9’ when the rival has started to target that scrap, the rival then stands still in the middle of survey point 0 and 5. 

Visible: 0, Travel: failed, Task: Collect, Cargo: false, target: 5, blocked: 1. 

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
