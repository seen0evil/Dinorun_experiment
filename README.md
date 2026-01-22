# Dinorun_experiment — Modified DinoRun for Latency Adaptation (Experiment 2)

This repository contains a research-oriented modification of **JHeaon/dinorun** (Unity DinoRun) for **Experiment 2 (DinoRun)** in the paper:

> **Immediate Auditory Feedback Speeds Visuomotor Delay Adaptation in Video Games**  

The experiment tests whether **immediate auditory confirmation of input registration** helps players adapt more quickly to **visuomotor latency** (input-to-visual delay), compared to auditory feedback that is **delayed to coincide with the delayed visual event**.

## Upstream / Original Project Attribution

This project is a derivative work of:

- **Upstream repository:** https://github.com/JHeaon/dinorun

All credit for the baseline DinoRun implementation belongs to the upstream author(s). This repo focuses on **instrumentation and experimental manipulation** needed to run the DinoRun task as a controlled study.

### Asset Credits (from upstream)

The upstream repository credits the following third-party assets/resources (retained or adapted per your project configuration):

- Character Asset: https://assetstore.unity.com/packages/2d/characters/bolt-2d-dinorun-assets-pack-188721  
- Title Asset: https://sysrqmts.com/ko/prices/dino-run-dx  
- Background Music: https://bgmstore.net/  
- Sound Effect: https://www.youtube.com/shorts/gNpmzpJ4Ro0  

If you redistribute builds, please verify the licensing/terms for each asset above and ensure your usage complies.

## Research Context

Online/cloud gaming often involves latency between a player’s input and the resulting onscreen event. When that latency changes, players must adapt by pressing earlier. A key difficulty is that the **moment an input is registered** is not perfectly perceivable during a physical keypress—so the player’s estimate of “how much latency is in the system” can be noisy.

This experiment evaluates a low-cost UI/game-feel intervention:

- **Immediate Auditory Feedback:** play a short beep **at the moment the keypress is registered**
- **Delayed Auditory Feedback:** play the same beep **synchronized with the delayed visual response**

## Task Overview (Experiment 2: DinoRun)

This task is a timing-based variant of DinoRun deployed in-browser (Unity WebGL).

**Core mechanic:**
- Coins spawn at the right side of the screen at ~3s intervals (with jitter).
- Coins scroll leftward at the environment speed.
- The player presses a key to jump.
- Coins are placed such that optimal performance requires timing the jump so the avatar reaches **peak height when the coin passes**.

**Latency manipulation:**
- Practice + Baseline: no added visuomotor latency
- Latency phase: introduce input-to-visual latency of approximately **200 ms (± ~33 ms)**

**Auditory feedback manipulation during the latency phase:**
- Immediate vs. Delayed (between-subjects)
