## Filters

Filters are used to build WPF or Silverlight effects through EffectBuilder objects. See [Tutorial](Tutorial). It's very important to understand that you can **combine filters** to get more powerfull effects ! Filters generate the HLSL code from intrinsic algorithms and .NET properties.
* [Contraster and Brighter](#Contraster_Brighter)
* [#Saturator](#Saturator)
* [#GrayScaler](#GrayScaler)
* [#Clipper](#Clipper)
* [#ColorToner](#ColorToner)
* [#Inverter](#Inverter)
* [#Mixer](#Mixer)
* [#Subtractor](#Subtractor)
* [#Solarizer](#Solarizer)
* [#ThresholdFilter](#ThresholdFilter)
* [#WaveWarper](#WaveWarper)
* [#Convolution3X3Filter](#Convolution3X3Filter)
	* [Blur](#Convolution3X3Filter_Blur)
	* [Sharp](#Convolution3X3Filter_Sharp)
	* [Edge](#Convolution3X3Filter_Edge)
	* [Emboss](#Convolution3X3Filter_Emboss)
{anchor:Contraster_Brighter}
### Contraster and Brighter
Filters to change contrast and brightness.
The default level (1.0) for contrast or brightness doesn't affect the image.
A null or negative brighness level gives a black image.

Definition :
{{
<pfx:EffectBuilder
     EffectName="ContrastBrightnessEffect">
    <pfx:Contraster FilterName="Contrast"/>
    <pfx:Brighter FilterName="Brightness"/>
</pfx:EffectBuilder>
}}
Usage :
{{
<Image 
      Source="http:.../MyPicture.jpg"
      Stretch="None">
          <Image.Effect>
              <fx:ContrastBrightnessEffect ContrastLevel="3" BrightnessLevel="1.25"/>
          </Image.Effect>
</Image>
}}
| ![](Filters_http://www.odewit.net/Images/perspectivefx/RomeTrevi.w350.JPG) | ![](Filters_http://www.odewit.net/Images/perspectivefx/RomeTrevi.w600.ContrastBrightness.jpg) |
{anchor:Saturator}
### Saturator
A filter to change the saturation of an image. The default level (1.0) doesn't affect the image. A level of 0.0 gives a grayscale image. A negative level gives a negative image.

Definition :

{{
<pfx:EffectBuilder
    EffectName="SaturatorEffect">
    <pfx:Saturator FilterName="Saturation"/>
</pfx:EffectBuilder>
}}
Usage :

{{
<Image 
    Source="http://...myImage.jpg"
    Stretch="None">
    <Image.Effect>
        <fx:SaturatorEffect SaturationLevel="2.5"/>
    </Image.Effect>                
</Image>
}}
| ![](Filters_http://www.odewit.net/Images/perspectivefx/Holywood.w350.jpg) | ![](Filters_http://www.odewit.net/Images/perspectivefx/Holywood.w600.Saturator.jpg) |
{anchor:GrayScaler}
### GrayScaler
A Black-and-white filter.

Definition :

{{
<pfx:EffectBuilder
    EffectName="GrayScalerEffect">
    <pfx:GrayScaler FilterName="GrayScaler"/>
</pfx:EffectBuilder>
}}
Usage :

{{
<Image 
    Source="http://...myImage.jpg"
    Stretch="None">
    <Image.Effect>
        <fx:GrayScalerEffect />
    </Image.Effect>                
</Image>
}}
| ![](Filters_http://www.odewit.net/Images/perspectivefx/Loco.w350.jpg) | ![](Filters_http://www.odewit.net/Images/perspectivefx/Loco.GrayScaler.jpg) |
{anchor:Clipper}
### Clipper
Clips the input using an opacity mask brush.

Definition :

{{
<pfx:EffectBuilder
    EffectName="ClipperEffect">
    <pfx:Clipper FilterName="Clipper"/>
</pfx:EffectBuilder>
}}
Usage :

{{
<Window.Resources>
    <RadialGradientBrush 
        x:Key="radialGradient">
        <GradientStop Color="Red" Offset="0" />
        <GradientStop Color="Red" Offset="0.8" />
        <GradientStop Color="Transparent" Offset="1" />
    </RadialGradientBrush>
    <Rectangle 
        x:Key="radialGradientRect" 
        Width="100"
        Height="100"
        Fill="{StaticResource radialGradient}"/>
    <VisualBrush 
        x:Key="radialGradientBrush" 
        Visual="{StaticResource radialGradientRect}"
        Stretch="Uniform"/>
...
</Window.Resources>
...
<Image 
    Source="http://...myImage.jpg"
    Stretch="None">
    <Image.Effect>
        <fx:ClipperEffect
            ClipperMask="{StaticResource radialGradientBrush}" />
    </Image.Effect>                
</Image>
}}
| ![](Filters_http://www.odewit.net/Images/perspectivefx/LocoWheel.w350.jpg) | ![](Filters_http://www.odewit.net/Images/perspectivefx/LocoWheel.Clipper.jpg) |
{anchor:ColorToner}
### ColorToner
A toner filter for grayscale or colour images. Default properties values give a sepia tone to a grayscale picture.

Definition :

{{
<pfx:EffectBuilder
    EffectName="ColorTonerEffect">
    <pfx:ColorToner FilterName="ColorToner"/>
</pfx:EffectBuilder>
}}
Usage :

{{
<Image 
    Source="http://...myImage.jpg"
    Stretch="None">
    <Image.Effect>
        <fx:ColorTonerEffect
            ColorTonerRedRatio="0.9"
            ColorTonerGreenRatio="0.82"
            ColorTonerBlueRatio="0.19"/>
    </Image.Effect>                
</Image>
}}
| ![](Filters_http://www.odewit.net/Images/perspectivefx/Lupin.w350.jpg) | ![](Filters_http://www.odewit.net/Images/perspectivefx/Lupin.ColorToner.jpg) |
{anchor:Inverter}
### Inverter
An inversion filter, which creates a negative image. 

Definition :

{{
<pfx:EffectBuilder
    EffectName="InverterEffect">
    <pfx:Inverter FilterName="Inverter"/>
</pfx:EffectBuilder>
}}
Usage :

{{
<Image 
    Source="http://...myImage.jpg"
    Stretch="None">
    <Image.Effect>
        <fx:InverterEffect />
    </Image.Effect>                
</Image>
}}
| ![](Filters_http://www.odewit.net/Images/perspectivefx/LocoWheel2.w350.jpg) | ![](Filters_http://www.odewit.net/Images/perspectivefx/LocoWheel2.Inverter.jpg) |
{anchor:Mixer}
### Mixer
A filter to mix 2 inputs.

Definition :

{{
<pfx:EffectBuilder
    EffectName="MixerEffect">
    <pfx:Mixer FilterName="Mixer"/>
</pfx:EffectBuilder>
}}
Usage :

{{
<Window.Resources>
    <ImageBrush 
        x:Key="imageBrush" 
        ImageSource="http://...myImageB.jpg" />
...
</Window.Resources>
...
<Image 
    Source="http://...myImageA.jpg"
    Stretch="None">
    <Image.Effect>
        <fx:MixerEffect 
            MixerGain1="0.55"
            MixerGain2="0.97"
            MixerInput2="{StaticResource imageBrush}" />
    </Image.Effect>                
</Image>
}}
| ![](Filters_http://www.odewit.net/Images/perspectivefx/RomeA.w275.jpg) | ![](Filters_http://www.odewit.net/Images/perspectivefx/RomeB.w275.jpg) |
![](Filters_http://www.odewit.net/Images/perspectivefx/Rome.Mixer.jpg)
{anchor:Subtractor}
### Subtractor
A filter to mix 2 inputs, using color subtraction. Usefull to build graduated effects. The xxInput2 brush intensity can be set through the xxGain2 property.

Definition :

{{
<pfx:EffectBuilder
    EffectName="SubtractorEffect">
    <pfx:Subtractor FilterName="Subtractor"/>
</pfx:EffectBuilder>
}}
Usage :

{{
<Window.Resources>
    <LinearGradientBrush 
        x:Key="linearGradient2"
        StartPoint="0.5,0"
        EndPoint="0.5,1">
        <GradientStop Color="Orange" Offset="0" />
        <GradientStop Color="White" Offset="0.8" />
        <GradientStop Color="White" Offset="1" />
    </LinearGradientBrush>
    <Rectangle 
        x:Key="linearGradientRect" 
        Width="100"
        Height="100"
        Fill="{StaticResource linearGradient2}"/>
    <VisualBrush 
        x:Key="linearGradientBrush" 
        Visual="{StaticResource linearGradientRect}"
        Stretch="UniformToFill"/>
</Window.Resources>
...
<Image 
    Source="http://...myImage.jpg"
    Stretch="None">
    <Image.Effect>
        <fx:SubtractorEffect
            SubtractorInput2="{StaticResource linearGradientBrush}"/>
    </Image.Effect>                
</Image>
}}
| ![](Filters_http://www.odewit.net/Images/perspectivefx/Horizon.w350.jpg) | ![](Filters_http://www.odewit.net/Images/perspectivefx/Horizon.Subtractor.jpg) |
{anchor:Solarizer}
### Solarizer
A solarization filter.

Definition :

{{
<pfx:EffectBuilder
    EffectName="SolarizerEffect">
    <pfx:Solarizer FilterName="Solarizer"/>
</pfx:EffectBuilder>
}}
Usage :

{{
<Image 
    Source="http://...myImage.jpg"
    Stretch="None">
    <Image.Effect>
        <fx:SolarizerEffect 
            SolarizerThreshold="0.2"/>
    </Image.Effect>                
</Image>
}}
| ![](Filters_http://www.odewit.net/Images/perspectivefx/Loco.w350.jpg) | ![](Filters_http://www.odewit.net/Images/perspectivefx/Loco.Solarizer.0.2.jpg) |
{anchor:ThresholdFilter}
### ThresholdFilter
A filter that produces high-contrast rendering (2 levels by color).

Definition :

{{
<pfx:EffectBuilder
    EffectName="ThresholdEffect">
    <pfx:ThresholdFilter FilterName="ThresholdFilter"/>
</pfx:EffectBuilder>
}}
Usage :

{{
<Image 
    Source="http://...myImage.jpg"
    Stretch="None">
    <Image.Effect>
        <fx:ThresholdEffect
            ThresholdFilterThreshold="0.34" />
    </Image.Effect>                
</Image>
}}
| ![](Filters_http://www.odewit.net/Images/perspectivefx/WesternPacific.w350.jpg) | ![](Filters_http://www.odewit.net/Images/perspectivefx/WesternPacific.Threshold.034.jpg) |
{anchor:WaveWarper}
### WaveWarper
A filter to warp the input.

Definition :

{{
    <pfx:EffectBuilder
        EffectName="WaveWarperEffect">
        <pfx:WaveWarper FilterName="WaveWarper"/>
    </pfx:EffectBuilder>
}}
Usage :

{{
<Image 
    Source="http://...myImage.jpg"
    Stretch="None">
    <Image.Effect>
        <fx:WaveWarperEffect 
            WaveWarperX1="-4.92"
            WaveWarperX2="-0.08"
            WaveWarperY1="3.62"
            WaveWarperY2="-0.07"/>
    </Image.Effect>                
</Image>
}}

_This effect must be specified in first position in the EffectBuilder filter collection. It can't be used with Convolution3X3Filter._

| ![](Filters_http://www.odewit.net/Images/perspectivefx/Loco.w350.jpg) | ![](Filters_http://www.odewit.net/Images/perspectivefx/LocoColor.WaveWarper.jpg) |
{anchor:Convolution3X3Filter}
### Convolution3X3Filter
Convolution3X3Filter uses a 3X3 matrix to determine the rgb value of the current pixel from a sampling of the 9 adjacent pixels, by cumulating their rgb ponderated value. For each adjacent pixel, the ponderation factor is read from the corresponding xxMnn matrix property, divided by the xxDivisor property. Typically, the xxDivisor property should be the sum of all the matrix properties. A zero value is considered to be 1.

The size of the image should be set in the xxHorizontalPixelCount and xxVerticalPixelCount properties. The default values are respectively 40 and 30. These properties determine the distance between the current pixel and the pixels to sample.

A Convolution3X3Filter lets you apply the following effects :
* Blur effect.
* Sharpening effect.
* Emboss effect.
* Edge effect.
Definition :

{{
<pfx:EffectBuilder
    EffectName="ConvolutionEffect">
    <pfx:Convolution3X3Filter FilterName="Convolution"/>
</pfx:EffectBuilder>
}}

_This effect must be specified in first position in the EffectBuilder filter collection. It can't be used with WaveWarper._

{anchor:Convolution3X3Filter_Blur}
#### Blur effect

A blur effect can be optained from a matrix with corners to 1 :

| 1 | 0 | 1 |
| 0 | 0 | 0 |
| 1 | 0 | 1 |

_This effect has a better rendering on little images._

Usage :

{{
<Image 
    Source="http://...myImage.jpg"
    Stretch="None">
    <Image.Effect>
        <fx:ConvolutionEffect
            ConvolutionHorizontalPixelCount="600"
            ConvolutionVerticalPixelCount="398"
            ConvolutionDivisor="4"
            ConvolutionM00="1"
            ConvolutionM02="1"
            ConvolutionM20="1"
            ConvolutionM22="1" />
    </Image.Effect>                
</Image>
}}
| ![](Filters_http://www.odewit.net/Images/perspectivefx/Lupin.w350.JPG) | ![](Filters_http://www.odewit.net/Images/perspectivefx/Lupin.w600.Blur.jpg) |
{anchor:Convolution3X3Filter_Sharp}
#### Sharpening effect

A sharpening effect can be optained from a matrix that increases the color of the current pixel and reduce the color of the adjacent pixels :

| 0 | -1 | 0 |
| -1 | 5 | -1 |
| 0 | -1 | 0 |

Usage :

{{
<Image 
    Source="http://...myImage.jpg"
    Stretch="None">
    <Image.Effect>
        <fx:ConvolutionEffect
            ConvolutionHorizontalPixelCount="600"
            ConvolutionVerticalPixelCount="398"
            ConvolutionM01="-1"
            ConvolutionM10="-1"
            ConvolutionM11="5"
            ConvolutionM12="-1"
            ConvolutionM21="-1" />
    </Image.Effect>                
</Image>
}}
| ![](Filters_http://www.odewit.net/Images/perspectivefx/Lupin.w350.JPG) | ![](Filters_http://www.odewit.net/Images/perspectivefx/Lupin.w600.Sharp.jpg) |
{anchor:Convolution3X3Filter_Edge}
#### Edge effect

An edge effect can be optained from a matrix with the same negative value for each adjacent pixels, and a current pixel value equal to the opposite sum of the adjacent pixels values :

| -2 | -2 | -2 |
| -2 | 16 | -2 |
| -2 | -2 | -2 |

Usage :

{{
<Image 
    Source="http://...myImage.jpg"
    Stretch="None">
    <Image.Effect>
        <fx:ConvolutionEffect
            ConvolutionHorizontalPixelCount="600"
            ConvolutionVerticalPixelCount="398"
                ConvolutionM00="-2"
                ConvolutionM01="-2"
                ConvolutionM02="-2"
                ConvolutionM10="-2"
                ConvolutionM11="16"
                ConvolutionM12="-2"
                ConvolutionM20="-2"
                ConvolutionM21="-2"
                ConvolutionM22="-2" />
    </Image.Effect>                
</Image>
}}
| ![](Filters_http://www.odewit.net/Images/perspectivefx/Lupin.w350.JPG) | ![](Filters_http://www.odewit.net/Images/perspectivefx/Lupin.w600.Edge.jpg) |
{anchor:Convolution3X3Filter_Emboss}
#### Emboss effect

An emboss effect can be optained from a matrix that reduces the color of a corner and increases the color of the opposite corner. The light seems to emanate from the region with the highest value difference with the current pixel (here the top-left corner).

| -2 | 0 | 0 |
| 0 | 1 | 0 |
| 0 | 0 | 2 |

Usage :

{{
<Image 
    Source="http://...myImage.jpg"
    Stretch="None">
    <Image.Effect>
        <fx:ConvolutionEffect
            ConvolutionHorizontalPixelCount="600"
            ConvolutionVerticalPixelCount="398"
            ConvolutionM00="-2"
            ConvolutionM11="0.8"
            ConvolutionM22="2" />
    </Image.Effect>                
</Image>
}}
| ![](Filters_http://www.odewit.net/Images/perspectivefx/RomeTrevi.w350.JPG) | ![](Filters_http://www.odewit.net/Images/perspectivefx/RomeTrevi.w600.Emboss.jpg) |