# Project File Structure

This document outlines the organization of the file structure for the Unity AR smart room project.

## Folders

- **Assets**
  - **ExampleAssets**: Contains sample assets provided by Unity or for testing purposes.
  - **Plugins**
    - **Lumin**: Lumin SDK specific plugins.
  - **Prefabs**
    - **AROverlays**: Prefabs used for creating dynamic AR overlays.
  - **Scenes**
    - **BlankAR**: The main AR scene for the project.
  - **Scripts**
    - **AROverlay**
      - `QRCodeDetector.cs`: Handles QR code detection within the AR environment.
      - `OverlayManager.cs`: Manages the lifecycle and content of AR overlays.
      - `DynamicOverlay.cs`: Script attached to AR overlay prefabs to dynamically display data and interact with the user.
    - **DataCommunication**
      - `APICommunicator.cs`: Manages API calls and data transfer with the smart room's system.
    - **UI**
      - `DeviceControlUI.cs`: UI script for interacting with smart devices.

- **XR**
  - Settings and resources for XR plugins and management.

## Important Scripts

- `QRCodeDetector.cs`: Detects QR codes and retrieves the corresponding UID.
- `OverlayManager.cs`: Creates and updates AR overlays based on data linked to UIDs.
- `DynamicOverlay.cs`: Dynamically updates the content of AR overlays based on the smart room data.
- `APICommunicator.cs`: Handles all communications with the smart room's web-based API, including fetching device data and sending commands.

## Prefabs

- **AROverlays**: Prefabs designed for displaying information and controls in AR, dynamically updated based on smart room data.

## Builds

- The Builds folder is intended for storing the compiled versions of the app. It's excluded from version control.

## Coding standards

- Make sure to update the changes in the structure here and work according to the [Single Responsibility Principle](https://www.geeksforgeeks.org/single-responsibility-in-solid-design-principle/)!
- Make use of PascalCase and camelCase
