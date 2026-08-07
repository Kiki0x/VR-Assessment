# VR Puzzle Interaction Assessment

## Overview
This project is a VR puzzle game developed in Unity for the Meta Quest headset. The core objective is to place specific geometric shapes (Cube, Sphere, Cylinder) into their matching sockets. It features a fully functional world-space UI dashboard to track time, remaining objects, and manage game states.

## Key Features & Mechanics
*   **Physical Object Interaction:** Players use virtual hands to grab, manipulate, and place physics-based objects using the Meta XR Interaction SDK.
*   **Tag-Based Validation:** Sockets use Unity Tags and Trigger Colliders to verify if the correct object is placed.
*   **World-Space UI Dashboard:** A floating canvas tracks the active timer and the remaining objects required to complete the puzzle.
*   **Tactile UI Buttons:** Instead of relying on laser pointers, the UI buttons are physical triggers that the player must "poke" with their virtual hands, increasing VR immersion.
*   **Advanced State Management:** Players can Reset the entire scene or Restart the specific task, teleporting pieces back to their origin without triggering a scene-load black screen.

## Core Systems Architecture

### Object Snapping & Kinematic Locking
When a shape enters its designated socket, the system executes a clean physics freeze. Instead of destroying the Rigidbody (which breaks dependencies within Meta's internal SDK architecture), the object's Rigidbody is set to kinematic. The custom script then iterates through the object to disable the `Grabbable` and `HandGrabInteractable` components. This successfully locks the object into place error-free while allowing it to be easily reactivated during a game reset.

### Physics-Driven UI Configuration
To avoid input conflicts between the Meta XR Simulator and standard hand-tracking raycasts, the UI relies entirely on Unity's native physics engine. TextMeshPro button elements are equipped with Box Colliders and driven by a custom `PhysicalButton` script linked via UnityEvents. This creates a highly stable, tactile interface where players physically push buttons rather than aiming laser pointers.

### State Reset & Transform Flushing
Resetting object positions in VR requires bypassing the physics engine's cached states. If an object is simply teleported away from a socket, the trigger zone often re-grabs it instantly. To handle this, the reset logic implements a strict execution order: the object is temporarily deactivated (`SetActive(false)`) to force the SDK to drop all grab states. The transform is then moved to the origin, velocity and angular velocity are zeroed, and the object is reactivated. This ensures a flawless teleport without physics clipping.

## Setup & Build Instructions

### Prerequisites
*   **Unity Editor:** 2022.3.62f3 (with Android Build Support installed).
*   **Hardware:** Meta Quest headset with Developer Mode enabled.

### Project Setup
1. Clone this repository to your local machine.
2. Open **Unity Hub** and click **Add > Add project from disk**, then select the cloned folder.
3. Allow Unity to resolve the Meta XR packages and project settings. 
4. In the Project window, navigate to your Scenes folder and open the main puzzle scene.

### Building for Meta Quest
1. Navigate to **File > Build Settings**.
2. Verify the platform is set to **Android**.
3. Ensure **Texture Compression** is set to **ASTC**.
4. In **Player Settings > Other Settings**, verify the Color Space is **Linear** and Scripting Backend is **IL2CPP**.
5. Click **Build**, save the `.apk` file, and sideload it onto your Meta Quest using SideQuest or the Meta Quest Developer Hub.
