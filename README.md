AR Auto Showroom — Project Summary
Project Overview
Unity 6 AR application for ESLSCA DMVR606 final project. Place and interact with 3D vehicles using markerless plane detection and marker-based image tracking.
Team

Kareem (Resha) — Markerless AR, plane detection, gestures (drag rotate, pinch scale), vehicle placement
Joe — Marker-based AR, AR Tracked Image Manager, marker spawning
Abdelrahman (Abdo) — UI Canvas, audio system, car animations (doors, hood), home screen design

Tech Stack

Unity 6 (6000.2.2f1)
AR Foundation
XR Interaction Toolkit
Google ARCore (Android)
Universal Render Pipeline (URP)
Scripting Backend: IL2CPP
Target: Android ARM64, Min API 26

Repository
GitHub: https://github.com/Reshoozzz/AR-Auto-Showroom
Working branch: CONFLICT_SPACE_BACKUP_3
Scenes (in Build Order)

Abdo.unity (index 0) — Home Screen with two buttons: "Markerless Experience" and "Marker-Based Experience"
Resha.unity (index 1) — Markerless AR scene (plane detection + tap to place)
Joe.unity (index 2) — Marker-based AR scene (image tracking via MER/BMW logos)

Main Scripts

ARVehiclePlacement.cs — handles plane detection, raycasting, vehicle spawn, rotation, scale, reset
MarkerVehicleSpawner.cs — handles AR Tracked Image detection and car positioning on marker
HomePageManager.cs — scene navigation (LoadMarkerless / LoadMarkerBased)
CarUIPanelManager.cs — toggles Sportage/Honda UI panels
CarController.cs — door/hood animations, color changes, wheel changes, engine sound
UImanager.cs, AudioManager.cs, BackgroundMusicManager.cs — Abdo's UI/audio managers

Vehicles

Car_Sportage (Kia Sportage)
Honda Civic
(FINAL_MODEL_GT existed but excluded from project)

Markers (XR Reference Image Library: CarMarkers)

MER (Mercedes logo) → spawns Car_Sportage
BMW (BMW logo) → spawns Honda Civic
Physical size configured: 0.2m × 0.2m

Current Status
Working

Home Screen with scene navigation ✅
Markerless plane detection + tap to place vehicle
Drag-to-rotate gesture
Pinch-to-scale gesture
Vehicle reset logic
Hide planes after placement
Marker-based image tracking (camera opens, detection runs)
UI buttons in scenes (Color, Wheels, Engine, Doors, Hood, Reset)
Sportage/Honda UI panel switching
Audio system with background music + engine sound
Door + Hood animations
APK builds and installs on Android device
Camera feed displays correctly on device

Known Issues / Incomplete

Marker detection works but car positioning needs refinement (cars need to spawn flat on floor regardless of marker orientation)
Sportage prefab disabled state behavior — needs verification on device
UI buttons in Joe.unity may throw NullReferenceException if no car spawned yet
Voiceover system not implemented
Virtual Turntable (auto-rotation toggle) not implemented
Two-finger rotation gesture not implemented
Performance Report not yet written
GDD not yet written
50-second demo video not yet recorded

Build Settings

Platform: Android
Min API: 26 (Android 8.0)
Architecture: ARM64
Scripting Backend: IL2CPP
Package: com.eslsca.arshowroom
