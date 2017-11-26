## Writing custom effects for WPF or Silverlight using Perspective FX

You can write custom effects with the Perspective FX tools, without any HLSL programming (HLSL is generated and compiled for you). The process is very simple : you generate an effect library using a command-line tool and a configuration file, and then you use your library in a WPF or Silverlight project :
* [Prerequisites](#Prerequisites)
* [Create your effect library using FxGeneratorCmd](#FxGeneratorCmd)
* [Use your effect library in a WPF or Silverlight project](#UseYourLib)
* [Add several effects in your library](#AddSeverlaEffects)
* [Get more powerfull effects by combining filters](#CombiningFilters)
* [Image Warping](#ImageWarping)

{anchor:Prerequisites}
### Prerequisites
Perspective FX 2.1 requires : 
* Visual Studio 2010 or [Windows SDK for Windows 7 and .NET Framework 4](http://www.microsoft.com/downloads/en/details.aspx?FamilyID=6B6C21D2-2006-4AFA-9702-529FA782D63B).
* [The DirectX End-User Runtime](http://www.microsoft.com/downloads/en/details.aspx?familyid=2DA43D38-DB71-4C1B-BC6A-9B6652CD92A3).
* Silverlight Tools for Visual Studio 2010 (if you want to build Silverlight effect assemblies). 

Notes : 
* Although WPF 4.0 supports Pixel Shader 3.0, Perspective FX 2.0 currently generates only Pixel Shader 2.0 effects. If you need Pixel Shader 3.0 effects, you can get the generated source code, compile it with fxc.exe (DirectX SDK), and then include it in a WPF 4.0 project as Resource. Remember that Pixel Shader 3.0 doesn't render in software mode.

{anchor:FxGeneratorCmd}
### Create your effect library using FxGeneratorCmd

1) Build or get FxGeneratorCmd.exe and its dependencies (all in the same directory) :
	* Perspective.PixelShader.dll
	* Perspective.PixelShader.Build.dll
	* Perspective.PixelShader.BuildTask.dll
	* Perspective.PixelShader.Compiler.dll
2) In the same directory, create and edit a new file called i.e. FxDemo.xaml :

{{
<pfx:EffectBuilderCollection
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:pfx="http://www.codeplex.com/perspectivefx"
    EffectDirectory="sources"
    EffectNamespace="FxDemo.Effects"    
    >
    <pfx:EffectBuilder
        EffectName="SolarizerEffect">
        <pfx:Solarizer FilterName="Solarizer"/>
    </pfx:EffectBuilder>
</pfx:EffectBuilderCollection>
}}
This file defines a collection of effect specifications (EffectBuilderCollection), each one being specified by an EffectBuilder element. Each EffectBuilder defines the effect class name by the EffectName property, and the effect algorithm by one or several filter(s), here a Solarizer.

3) Execute the following command (or in the same directory, create and edit a new file called i.e. FxGen.bat, and execute it) :

{{ FxGeneratorCmd FxDemo.xaml }}

A directory called _source_ is created. It contains :
* SolarizerEffect.fx : the HLSL generated code.
* SolarizerEffect.ps : the compiled HLSL code.
* SolarizerEffect.cs : the .NET WRAPPER for WPF or Silverlight.
* _bin_ : a directory in which you can find your effect library for WPF : FxDemo.Effects.dll

* Use the following command to generate an assembly for debugging :
{{ FxGeneratorCmd FxDemo.xaml /debug }}

* Use the following command to generate a Silverlight assembly, and not a WPF one :
{{ FxGeneratorCmd FxDemo.xaml /sl:1 /wpf:0 }}

_Note : the Silverlight Tools for Visual Studio should have been installed previously._

{anchor:UseYourLib}
### Use your effect library in a WPF or Silverlight project
In WPF or Silverlight, you can apply you effect to any UIElement, and even to a video. Here is how to apply it to an image :
* In the project, add a reference to your library.
* In a XAML file, add an Image element in the container of your choice (here a StackPanel) :
{{
<StackPanel Orientation="Horizontal">
    <Image 
        Source="http://...myImage.jpg"
        Stretch="None">
    </Image>
</StackPanel>
}}
![](Tutorial_http://www.odewit.net/Images/perspectivefx/Loco.jpg)
* In the heading container of the XAML file (here a WPF Window), add a XAML namespace referencing your library :
{{
<Window x:Class="FxDemo.Window1"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:fx="clr-namespace:FxDemo.Effects;assembly=FxDemo.Effects"
    Title="Window1" WindowState="Maximized">
...
</Window>
}}
* Assign your effect to the Effect property of the Image element :
{{
<Image 
    Source="http://...myImage.jpg"
    Stretch="None">
    <Image.Effect>
        <fx:SolarizerEffect />
    </Image.Effect>
</Image>
}}
_The effect class name comes from the EffectBuilder.EffectName value in the FxDemo.xaml file._

![](Tutorial_http://www.odewit.net/Images/perspectivefx/Loco.Solarizer.0.7.jpg)

The SolarizerEffect class has a SolarizerThreshold property, which has a default value of 0.7. This property indicates the threshold over which the color is inverted. 
_This property name comes from the Solarizer.FilterName (FilterBase.FilterName) value in the FxDemo.xaml file._
Here is the rendering with a value of 0.2 :

![](Tutorial_http://www.odewit.net/Images/perspectivefx/Loco.Solarizer.0.2.jpg)

{anchor:AddSeverlaEffects}
### Add several effects in your library

You can embed several effects in your library. Just add a new EffectBuilder entry for each effect in the FxDemo.xaml file :

{{
<pfx:EffectBuilderCollection ... >
    <pfx:EffectBuilder
        EffectName="SolarizerEffect">
        <pfx:Solarizer FilterName="Solarizer"/>
    </pfx:EffectBuilder>
    <pfx:EffectBuilder
        EffectName="GrayScalerEffect">
        <pfx:GrayScaler FilterName="GrayScaler"/>
    </pfx:EffectBuilder>
    ...
</pfx:EffectBuilderCollection>
}}
Here is the rendering of GrayScalerEffect :

![](Tutorial_http://www.odewit.net/Images/perspectivefx/Loco.GrayScaler.jpg)

_The gray scale is applied though 3 ratio, one for each rgb component, that you can change through the RedRatio, GreenRatio and BlueRatio properties._

{anchor:CombiningFilters}
### Get more powerfull effects by combining filters

An effect class can be built from several [filters](filters). Just specify them in the EffectBuilder element :

{{
<pfx:EffectBuilder
    EffectName="OldPaperEffect">
    <pfx:GrayScaler FilterName="GrayScaler"/>
    <pfx:ColorToner FilterName="ColorToner"/>
</pfx:EffectBuilder>
}}
Here we use first a GrayScaler filter, and then a ColorToner filter, which gives a good old sepia tone to the image :

![](Tutorial_http://www.odewit.net/Images/perspectivefx/Loco.OldPaper.jpg)

{anchor:ImageWarping}
### Image Warping

By default, the Input1 Brush (the main input sampler) is sampled at the current pixel coordinates.

But you can customize the sampling to produce a warping effect :
* Set the EffectBuilder.DefaultSampling to false.
* Insert _at the first position_ in the EffectBuilder children a warping filter, i.e. WaveWarper.
{{
<pfx:EffectBuilder
    EffectName="OldPaperEffect"
    DefaultSampling="False">
    <pfx:WaveWarper FilterName="WaveWarper"/>
    <pfx:GrayScaler FilterName="GrayScaler"/>
    <pfx:ColorToner FilterName="ColorToner"/>
</pfx:EffectBuilder>
}}
You can adjust the effect by using the WaveWarperX1, WaveWarperX2, WaveWarperY1 and WaveWarperY2 properties (i.e. by using bound sliders).

{{
<Image 
    Source="http://...myImage.jpg"
    Stretch="None">
    <Image.Effect>
        <fx:OldPaperEffect 
            x:Name="oldPaperEffect"
            WaveWarperX1="-4.92"
            WaveWarperX2="-0.08"
            WaveWarperY1="3.62"
            WaveWarperY2="-0.07"/>
    </Image.Effect>                
</Image>
}}

![](Tutorial_http://www.odewit.net/Images/perspectivefx/Loco.WaveWarper.jpg)