# Simulatica 
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
![Simulatica](https://img.shields.io/badge/Simulatica-0.2.7-orange)
![CalcEngine](https://img.shields.io/badge/CalcEngine-1.0.2-success)
![Visualizer](https://img.shields.io/badge/Visualizer-1.0.2-success)
![MonoGame](https://img.shields.io/badge/MonoGame-v3.8-red)
![DOTNET](https://img.shields.io/badge/.NETcore-v3.1-blue)
![csharp](https://img.shields.io/badge/.CSharp-v8.0-blueviolet)

### About

Universal & low-level simulation framework designed for mainly academic purposes. It consists of three project: 
- **CalcEngine** - Central part of application, responsible for actual calculation. It's bases on runtime compilation and `reflection`.
- **Visualizer** - Design to visualize the data recived from **CalcEngine**. Supports both 2D and 3D animations. Current frames rendering is performed via `MonoGame`. Support for Blender is planned in the future also. 
- **Simulatica** - User-friendly desktop dashboard, for creating simulations and handling all simulatica features and related cases. Currently suspended.

## Project entry

To begin simulation you must first provide proper simulation file `.conig` which must contain all of the necessary configuration in `json` format. Separate project [Simulatica Dashboard](https://github.com/BigFipil/SimulaticaDashboard/) can be helpfull with creating such configuration.   

### Demo simulations

![Solar scope simulation](https://github.com/BigFipil/Simulatica/blob/master/SolarScope1.gif)
![force field simulation](https://github.com/BigFipil/Simulatica/blob/master/sim.gif)

## Possible vulnerabilities
Project currently uses `Newtonsoft.Json` which may cause some potential security vulnerabilities.
