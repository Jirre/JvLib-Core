# JvLib-Core

 * [Description](#description)
 * [Gizmo Color Guide](#Gizmos)

## Description

This library provides the core functionality for the JvLib systems. Implementing many base features as well as providing required scripts shared amongst many of the different libraries provided.

<a name="Gizmos"/>

## Gizmo Color Guide
Within the Libraries, gizmos are used to easily denote and signal certain script types, these gizmos are differentiated by color and icon. For the colors a certain styleguide has been designed to easily denote a script's purpose. 

| Color           | Base        | Invert     | Use Cases |
|:------          |:-----       |:------    |:------ |
| Black | <div align="center"><img src="/Editor%20Resources/Gizmos/Entities/Gizmo_Entities_Black.png?raw=true"></div> | <div align="center"><img src="/Editor%20Resources/Gizmos/Entities/Gizmo_Entities_Black_Invert.png?raw=true"></div> | &bull; Static Classes |
| Blue  | <div align="center"><img src="/Editor%20Resources/Gizmos/Entities/Gizmo_Entities_Blue.png?raw=true"></div> | <div align="center"><img src="/Editor%20Resources/Gizmos/Entities/Gizmo_Entities_Blue_Invert.png?raw=true"></div> |  &bull; Data Classes <br>  &bull; Structs |
| Green | <div align="center"><img src="/Editor%20Resources/Gizmos/Entities/Gizmo_Entities_Green.png?raw=true"></div> | <div align="center"><img src="/Editor%20Resources/Gizmos/Entities/Gizmo_Entities_Green_Invert.png?raw=true"></div> |  &bull; Abstract </br>  &bull; Interface </br>  &bull; Attribute |
| Orange | <div align="center"><img src="/Editor%20Resources/Gizmos/Entities/Gizmo_Entities_Orange.png?raw=true"></div> | <div align="center"><img src="/Editor%20Resources/Gizmos/Entities/Gizmo_Entities_Orange_Invert.png?raw=true"></div> | (Unused)
| Purple | <div align="center"><img src="/Editor%20Resources/Gizmos/Entities/Gizmo_Entities_Purple.png?raw=true"></div> | <div align="center"><img src="/Editor%20Resources/Gizmos/Entities/Gizmo_Entities_Purple_Invert.png?raw=true"></div> |  &bull; Scriptable Objects |
| Red | <div align="center"><img src="/Editor%20Resources/Gizmos/Entities/Gizmo_Entities_Red.png?raw=true"></div> | <div align="center"><img src="/Editor%20Resources/Gizmos/Entities/Gizmo_Entities_Red_Invert.png?raw=true"></div> |  &bull; Behaviours </br>  &bull; ECS Systems |
  
#### Inverted Colors
Finally, we have inverted gizmos, these are used for each of the before mentioned scripts but only for the following cases:
- Non-Root Partial Classes
- Editor Scripts
