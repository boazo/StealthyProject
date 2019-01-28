# StealthyProject

Stealthy is a Unity3d project that is used thoughtout the תעשידע project.


# How to setup the enviornment

## Installing unity3d

Unity3d is a free to use software. However, registeration is required. 
Follow the installation instructions in this [link](https://docs.unity3d.com/Manual/InstallingUnity.html).

## Installing git

Download git installation executable from https://git-scm.com/downloads and start the installation by running the executable. 
Just click Next, Next, Next, ... until the installation completes.

## Cloning the project on your Desktop / Laptop

To clone this git repository on your PC. Open a Windows\MacOS cmd\terminal and type the following command:
```bash
git clone https://github.com/boazo/StealthyProject.git
```

# How to execute the demo scene

The repository includes a sample scene. To run Open a Windows\MacOS cmd\terminal and type the following command:
```bash
<project root folder>\Assets\Scenes\SampleScene.unity
```
This command will open the sample scene in unity3d. Be patient it might take a minute to load the first time opening the scene. 

From inside Unity press the Play button and execute the sample scene.

# Understanding the Project Folder Structure

The interesting project folder is **{project root folder}\Assets\Scripts**. This is where all of the game C# scripts are located.
This is how the Scripts folder is structured in this project:

```
<project root folder>\Assets\Scripts\Interface
```
This folder contains IMaze.cs that defines the __IMaze__ interface for painting the generated maze in unity.
Printing the generated maze is performed via this interface. Please go over this code as it should be used by your maze generator!

Enjoy :--)

```
<project root folder>\Assets\Scripts\Algorithms
```
This folder will contain the implementation of the different Maze generators developed by the different teams.
The initial version, includes a sample BorderMazeGenerator.cs class that creates a maze that paints a room surrounded by walls, hence, the name of the class BorderMazeGenerators. Please review this class as it demonstrates how to the IMaze interface.
```
<project root folder>\Assets\Scripts\Maze
```
  The implementation of the IMaze interface and more. It interacts with unity3d and is generally not in the scope of the project.
  Those of you who are interested are more then invited to explore the code in this folder as well :-)
  
# Getting started with Unity

It is highly recommended that you follow this unity tutorial: https://unity3d.com/learn/tutorials/s/interactive-tutorials.
It will help you ramp on unity3d. Keep exploring the Unity3d website as it contains many additional tutorials.

Additional external tutorials can be found on the web. Specifically, [Brackeys](https://www.youtube.com/user/Brackeys) is an awesome youtuber that goes over unity3d and has great unity3d beginner tutorials - you are welcome to explore his channel as well.
