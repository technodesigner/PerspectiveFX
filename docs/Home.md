**New ! [Perspective FX 2.2 for WPF 4 and Silverlight 5](http://perspectivefx.codeplex.com/releases/view/89604)**
* [Principles : effects and filters](#Principles)
* [Tutorial](Tutorial)
* [Filters](Filters)
* [The team](#Team)
{anchor:Principles}
## Principles : effects and filters
Usually, writing custom graphical effects classes for WPF or Silverlight requires some HLSL programming skills. For a .NET programmer, learning this language may be sometimes long and tricky. The Perspective FX framework aims to help you to build custom effects classes without HLSL coding. It generates the HLSL code and the .NET wrapper for WPF or Silverlight. It produces an assembly that you can use in a WPF or Silverlight application. 

You build an effect by combining graphical filters, like a [filter system for cameras](http://www.cokin.com/ico1-p1.html). Each filter acts as a primitive effect. The resulting effect depends of the nature of the combined filters and of their order. Perspective FX gives you some classical and experimental filters : solarizer, sepia toner, image warper, graduated filter, emboss filter, etc. 

![](Home_http://www.odewit.net/Images/perspectivefx/Loco.w350.jpg)
![](Home_http://www.odewit.net/Images/perspectivefx/Loco.w350.Solarizer.0.2.jpg)
![](Home_http://www.odewit.net/Images/perspectivefx/Loco.w350.OldPaper.jpg)
![](Home_http://www.odewit.net/Images/perspectivefx/LocoColor.w350.WaveWarper.jpg)

Of course, you can also write your own custom filters.
One effect may use one or several filters. One assembly may contain one or several effects.

The main parts of the Perspective FX framework are :
* FxGeneratorCmd.exe : a command-line tool to generate the code and the assembly from a XAML description file (which contains the filters combinations).
* Perspective.PixelShader.Build.dll : the XAML reading and MSBuild driving code, used by FxGeneratorCmd.exe.
* Perspective.PixelShader.BuildTask.dll : a custom MSBuild task.
* Perspective.PixelShader.Compiler.dll : a custom pixel shader compiler, dedicated to WPF and Silverlight effects.
* Perspective.PixelShader.dll : the heart of Perspective FX :
	* The filter classes (which generate HLSL code).
	* The classes that produce .NET wrapper and the main HLSL program.
Perspective FX requires : 
* Visual Studio 2010.
* [The DirectX End-User Runtime](http://www.microsoft.com/downloads/en/details.aspx?familyid=2DA43D38-DB71-4C1B-BC6A-9B6652CD92A3).
* Silverlight Tools for Visual Studio (if you want to build Silverlight effect assemblies).
Note : 
* Although WPF 4.0 supports Pixel Shader 3.0, Perspective FX currently generates only Pixel Shader 2.0 effects. If you need Pixel Shader 3.0 effects, you can get the generated source code, compile it with fxc.exe, and then include it in a WPF 4.0 project as Resource. Remember that Pixel Shader 3.0 doesn't render in software mode.
**[Tutorial](Tutorial)**
**[Filters](Filters)**

Olivier Dewit
[technodesigner.fr](technodesigner.fr)
[@technodesigner](twitter.com/technodesigner)