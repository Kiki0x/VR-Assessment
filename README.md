# VR Puzzle Interaction Assessment

## Overview
This project is a VR puzzle game developed in Unity for the Meta Quest headset. The core objective is to place specific geometric shapes (Cube, Sphere, Cylinder) into their matching sockets. It features a fully functional world-space UI dashboard to track time, remaining objects, and manage game states.

## Key Features & Mechanics

*   **Physical Object Interaction:** Players use virtual hands to grab, manipulate, and place physics-based objects using the Meta XR Interaction SDK.
*   **Tag-Based Validation:** Sockets use Unity Tags and Trigger Colliders to verify if the correct object is placed.
*   **World-Space UI Dashboard:** A floating canvas tracks the active timer and the remaining objects required to complete the puzzle.
*   **Tactile UI Buttons:** Instead of relying on laser pointers, the UI buttons are physical triggers that the player must "poke" with their virtual hands, increasing VR immersion.
*   **Advanced State Management:** Players can Reset the entire scene or Restart the specific task, teleporting pieces back to their origin without triggering a scene-load black screen.

## Technical Implementation (The "How" and "Why")

### 1. Object Snapping & Freezing
**The Challenge:** When an object enters the correct socket, it needs to freeze perfectly in place. However, simply deleting the Rigidbody caused console errors because Meta's hidden `RigidbodyKinematicLocker` script depends on it. 
**The Solution:** Instead of destroying the physics body, the script sets `rb.isKinematic = true` to turn off gravity. It then iterates through the object's components and safely disables the `Grabbable` and `HandGrabInteractable` scripts. This perfectly locks the object in place without throwing dependency errors, and allows the scripts to be re-enabled later.

### 2. Tactile UI Over Ray Interactors
**The Challenge:** Integrating standard Pointable Canvas laser interactors caused background conflicts with the Meta XR Simulator and the standard hand-tracking grab mechanics.
**The Solution:** I bypassed the complex Ray Interaction building blocks entirely and utilized physical triggers. By adding Box Colliders to the UI TextMeshPro buttons and writing a custom `PhysicalButton` script with UnityEvents, the UI is driven entirely by Unity's native physics engine. This proved significantly more stable and provided a better, more tactile user experience.

### 3. The "Hard Reset" Teleportation
**The Challenge:** When pressing "Restart Task," the blocks needed to teleport back to the table. However, Meta's grabbing scripts and Unity's physics engine cache object states. Teleporting the object while the physics engine was "awake" caused the sockets to instantly re-grab the objects before they could escape the trigger zones.
**The Solution:** I implemented a "Hard Reset" sequence. Before teleporting, the object's `gameObject.SetActive(false)` is triggered. This forces Unity and the Meta SDK to completely drop the object and flush its cached physics state. The transform is then safely moved to the spawn point, the physics variables (velocity, angular velocity) are zeroed out, the grab scripts are re-enabled, and the object is woken back up. This guarantees a clean, conflict-free teleport.

## Build Information
*   **Engine:** Unity 2022.3.62f3
*   **SDK:** Meta XR Interaction SDK
*   **Platform:** Android (Meta Quest)
*   **Rendering:** IL2CPP, ASTC Compression, Linear Color Space
