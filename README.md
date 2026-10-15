# Space Defense

## Description
You are DEF-01, a robot created by the last united civilizations of the cosmos. Designed to think faster than any being and adapt to any battlefield, your sole directive is simple: protect the universe at all costs.

Alien factions from distant galaxies have begun invading star systems, consuming planets and erasing entire species. As the final line of defense, you are deployed to eliminate these threats before they spread beyond control.

## Controls
- WASD / Arrow Keys – Move
- Space – Attack

## VFX / SFX (First Pass)
- A reusable `SimpleEffectsController` component now drives gameplay/UI audio and particles.
- It is used by `Player`, `Enemy`, `UIManager`, and `GameUIManager` for:
  - Player attack + movement cues
  - Enemy hit + death/explosion cues
  - Main-menu and back-button UI click feedback
- If you do **not** assign clips or particle prefabs, gameplay still runs safely:
  - Missing `AudioClip` values are skipped
  - Missing particle prefabs fall back to a small runtime procedural burst effect

### Unity setup steps
1. Select Player/Enemy/UI manager objects in scenes/prefabs.
2. Assign optional clips (`attackClip`, `moveClip`, `hitClip`, `deathClip`, `buttonClickClip`, etc.).
3. Assign optional particle prefabs (`attackVfxPrefab`, `moveVfxPrefab`, `hitVfxPrefab`, `deathVfxPrefab`).
4. (Optional) Create an `AudioMixer` with an SFX group and assign it to `SimpleEffectsController.outputMixerGroup`.
5. Tune per-object audio behavior with:
   - `masterVolume`
   - `randomPitchRange`
   - `spatialBlend` (2D vs 3D)
6. Tune movement pacing with `Player.moveFxInterval`.

## OOP Implementation
- Encapsulation: Player stats are private with public getters/setters
- Inheritance: Character → Player, Enemy
- Polymorphism: Attack() overridden in enemy

## How to Run
1. Open the project in Unity 6000.x.x
2. Open MainScene
3. Press Play
