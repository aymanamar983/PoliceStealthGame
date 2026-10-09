# Police Stealth Game

A 3D stealth and retrieval game prototype developed in Unity using C#. The project focuses on custom player movement, guard detection, state-based enemy AI, projectile combat, and mission progression.

## Game Overview

Play as a police operative infiltrating a guarded area. Avoid guard line of sight, retrieve the required mission item, and reach the extraction zone to complete the mission.

Guards patrol designated points, investigate the player's last known position after losing sight of the player, and shoot when the player is detected.

## Mission Objectives

1. Start the mission from the start screen.
2. Navigate the environment and avoid guard detection.
3. Retrieve the required item.
4. Reach the extraction zone with the item.
5. Avoid enemy bullets. A hit triggers the game-over screen.
6. Reach extraction successfully to display the mission-complete screen.

## Controls

| Input | Action |
|---|---|
| W / A / S / D | Move the player |
| Mouse | Aim, if enabled in the current build |
| Start button | Begin the mission |

Confirm these controls against the actual playable build before release.

## Guard AI

The guard uses three states:

### Patrol
- Moves between configured patrol points.
- Waits at each point for a configurable duration.
- Uses idle and walking animations.

### Suspicious / Investigate
- Stores the player's last known position after line of sight is lost.
- Moves to that position.
- Rotates to search the area for a configurable duration.
- Returns to patrol if the player is not detected again.

### Shoot
- Enters the shooting state when the player is visible.
- Rotates toward the player.
- Fires projectiles at a configurable rate.
- Can show a shooting indicator and play gunshot audio when configured.

## Line-of-Sight Detection

The custom vision system uses:
- `Vector3.Dot` to check the player's angle relative to the guard's forward direction.
- Distance checks to enforce the detection range.
- Raycasts to detect walls or other configured obstacles blocking visibility.
- A visual vision cone for debugging and tuning the field of view.

## Mission and UI Systems

The project includes UI systems for:
- Start screen
- Item/key-required notification at extraction
- Mission-complete screen
- Game-over screen
- Guard suspicious and shooting indicators

The mission can only be completed when the required item has been collected and the player enters the extraction zone.

## Technical Details

- **Engine:** Unity
- **Language:** C#
- **Player movement:** Rigidbody-based movement (no `CharacterController`)
- **Camera:** Top-down follow camera
- **Guard navigation:** Unity `NavMeshAgent`
- **Enemy AI:** Custom state-based logic
- **Detection:** Dot product, distance checks, and raycast occlusion
- **Combat:** Pooled projectiles
- **Animation:** Unity Animator
- **Target platform:** WebGL

## How to Open the Project

1. Clone or download this repository.
2. Open Unity Hub.
3. Select **Add project from disk**.
4. Select the `PoliceStealthGame` folder.
5. Open it with the Unity Editor version appropriate for the project.
6. Open the main gameplay scene from the `Assets` folder.
7. Press **Play** to test the game.

## Build for WebGL

1. Open the project in Unity.
2. Open **Build Profiles** or **Build Settings**, depending on your Unity version.
3. Select **WebGL** as the target platform and switch platforms if needed.
4. Add the gameplay scene to the build scene list.
5. Build the project.
6. Host the resulting WebGL build on a compatible static hosting service and test it in a browser.

## Repository Structure

```text
PoliceStealthGame/
├── Assets/           # Scenes, scripts, models, materials, audio, UI
├── Packages/         # Unity package manifest and dependencies
├── ProjectSettings/  # Unity project configuration
├── .gitignore
└── README.md
```

## Optimization Notes

The project uses a custom projectile pool. Before final submission, profile the WebGL build and verify memory usage and frame rate. Pool muzzle-flash and impact effects as well if they are still instantiated and destroyed during gameplay. Confirm the performance target on the intended browser and hardware.

## Future Improvements

- Pool all gameplay effects.
- Profile and reduce WebGL memory usage and garbage collection.
- Avoid unnecessary repeated navigation updates.
- Improve guard search and combat behaviour.
- Polish animations, audio, lighting, and UI feedback.
- Add a playable WebGL link and screenshots to this README.

## Credits and Licenses

List any third-party models, animations, audio, fonts, or other assets used in the project and follow their respective licenses.

## Developer

Unity gameplay programming project demonstrating movement, stealth detection, guard AI, projectile combat, and mission progression.
