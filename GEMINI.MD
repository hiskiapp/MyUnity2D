# Unity 2D Desktop Pet Listing Application

## Overview
Create a cross-platform desktop application using Unity 2D that displays pet data from a REST API. The application should be buildable for both Windows and macOS platforms.

## Requirements

### Technical Specifications
- **Unity Version**: 6000.1.14f1
- **Target Platforms**: Windows and macOS desktop

### Core Functionality
- Fetch and display pet data from REST API endpoint
- Simple, clean UI for listing pets
- Error handling for network requests
- Responsive design suitable for desktop usage

### API Integration
- **Endpoint**: `/pets`
- **Default Base URL**: `http://localhost:5018` (configurable)
- **HTTP Method**: GET
- **Expected Response Format**:
  ```json
  [
    {
      "id": 1,
      "name": "Rin",
      "createdAt": "2025-08-02T11:20:07.648841Z",
      "updatedAt": "2025-08-02T11:20:07.648841Z"
    },
    {
      "id": 2,
      "name": "My",
      "createdAt": "2025-08-02T11:20:07.648841Z",
      "updatedAt": "2025-08-02T11:20:07.648841Z"
    },
    {
      "id": 3,
      "name": "Tom",
      "createdAt": "2025-08-02T13:03:17.413045Z",
      "updatedAt": "2025-08-02T13:03:17.413045Z"
    }
  ]
  ```

### Configuration Requirements
- API base URL should be configurable via environment variables
- Support for different environments (development, staging, production)
- Easy testing configuration for CI/CD pipelines (GitHub Actions)
