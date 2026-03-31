# 🎮 Duet Cats – Playable Ads Case Study

A minimal playable version of *Duet Cats*, built for the Playable Ads Developer case study.

## 🔗 Playable Build
👉 [Play here]([YOUR_WEB_LINK](https://thuonggamedev.itch.io/duet-cats))

## 📦 Repository
👉 [GitHub Repo]([YOUR_WEB_LINK](https://github.com/ThuongDev1203/DuetCat.git))

---

## 🧩 Overview

This project recreates the core gameplay loop of Duet Cats in a short and interactive format:

- Notes spawn based on JSON timing data
- Player controls 2 cats (left/right)
- Hit notes to gain score
- Miss → lose life
- Win when all notes are cleared

The focus is on **clarity, responsiveness, and quick onboarding**, suitable for playable ads.

---

## 🎮 Core Features

- ✅ JSON-based note spawning
- ✅ Touch & mouse input
- ✅ Hit / Miss detection
- ✅ Score & lives system
- ✅ Basic UI (portrait & landscape)

---

## 🏗️ Project Structure
Scripts/
├── Core (GameManager, Input, UI, Audio)
├── Gameplay (Note, Spawner)
└── Data (JSON Loader)

---

## ⚙️ How It Works

### Note Spawning
- Notes are loaded from JSON and sorted by time
- Spawned early based on `travelTime` to sync with audio

### Input
- Screen divided into left/right
- Each side controls one cat

### Hit Detection
- Uses simple collision (OverlapCircle)
- Hit → score
- Miss → lose life

---

## 🧠 Key Decisions

- Singleton pattern for main managers
- Data-driven spawning (JSON)
- Simple architecture for fast iteration

---

## ⚖️ Trade-offs

- No object pooling (using Instantiate/Destroy)
- Basic UI and VFX
- No Perfect/Good timing system
- Limited optimization

---

## 🚀 Improvements (Future Work)

- Object pooling for performance
- Better hit feedback (VFX, sound)
- Timing accuracy (Perfect/Good)
- Combo system
- UI polish & animation

---

## 🛠️ Tech Stack

- Unity
- C#
- UniTask

---

## 📌 Notes

This project prioritizes **core gameplay clarity** over full feature completeness due to time constraints (3–5 days scope).
