# RoboViz - Robot Swarm Visualizer Tool
## Introduction

The RoboViz tool is a visualizer tool to represent a robot swarm in its task environment. The tool takes in a JSON file containing the definition (as specified by [Robogen](https://robogen.org/)) of a reference robot and visualizes a homogeneous or heterogeneous robot-swarm of a specified size. The task environment is a 3D bounded arena, and the task is a pre-specified collective gathering.

## Using the Tool
### Linux 

1. Clone the github repo & cd into the directory
```
git clone https://github.com/cyberCharl/Capstone-Roboviz
cd Capstone-Roboviz
```
2. Make the file executable
```
chmod +x RoboVizTooool.x86_64
```
3. Execute the file
```
./RoboVizTooool.x86_64
```

## Functional Requirements

- Read and parse the input JSON file
- Read and parse configuration text file
- Read and parse robot positions text file
- Render 3D scene with task environment and robots
- Navigate scene - zoom in and out of the rendered scene

## Input Files
### Robot Swarm File

This is a JSON file obtained from the RoboGen platform - robogen-evolver. It contains all the information required to construct a given robot, including body parts and its controller.

### Configuration File

Contains the required parameters:

- terrainLength (INT)
- terrainWidth (INT)
- swarmSize (INT)

### Robot Positions Configuration File

Contains the robot's position:

- x-coordinate y-coordinate z-coordinate

## The Team
Developed by: 
- Charl Botha
- George du Plooy 
- Bernard Strauss

UCT 3rd Year Compsci 2022
