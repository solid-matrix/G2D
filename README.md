# G2D

A lightweight, ultra-high-performance, cross-platform 2D game engine with Native AOT support and an easy-to-use API.

## Features

### Structure

- `G2D`: Core library providing window management, event handling, rendering, audio, and video functionality.
- `G2D.Ecs`: ECS architecture framework built on top of G2D.
- `G2D.GameObject`: Node-tree architecture framework (similar to Godot and Unity) based on G2D.

### Tech Stack

- SDL3: Windowing and event handling, utilizing the `ppy.SDL3-CS` binding library.

- Vulkan: Graphics API, utilizing the `Vortice.Vulkan` binding library.

### Target Device

Currently only supports modern devices (with Vulkan 1.3 support or above). Compatibility for legacy devices is not planned at this stage.

## Todos

- ~~Implement Vulkan descriptor Indexing and Texture rendering:~~

  ~~Enable texture rendering without updating descriptor sets.~~

- ~~Optimize `BufferSpanPool`:~~

  ~~Enable intelligent reuse the VkBuffer for vertices, indices and instance data.  So that 0 VkBuffers will be created or destroyed within a frame.~~

- Optimize graphics pipeline:

  Enable the default graphics pipeline to cover most common rendering tasks.

- Implement custom shader support:

  Provide users with programmable vertex and fragment shader, configurable graphics pipeline, customizable uniform data, and multiple texture and sampler slots.

- Optimize `Graphics` class:

  Cache user draw call and batch them into a single draw call, unless the graphics pipeline changed.

- Optimize GC:

  Strive for zero GC in runtime loops.
  
- Supports drawing basic primitives:

  triangles, polygons, circles, rectangles, and capsules.

- Implement engine -native multi-threading and asynchronization to maximize performance potential.



