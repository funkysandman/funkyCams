```NETFx``` - C# Image Manipulation Library

Copyright (C) 2012 Asad Memon

## MIT Licence


Permission is hereby granted, free of charge, to any person 
obtaining a copy of this software and associated documentation 
files (the "Software"), to deal in the Software without restriction, 
including without limitation the rights to use, copy, modify, 
merge, publish, distribute, sublicense, and/or sell copies of 
the Software, and to permit persons to whom the Software is 
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included 
in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR 
ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

NETFx
=====

NetFx is a C# image manipulation library. This library makes it possible to clone Instagram effect in your own Apps.
The library provides basic building blocks for advanced effects. Think of it as Instagram filters for .NET Developers.
The library is heavily inspired by [filtrr2](https://github.com/alexmic/filtrr) javascript library by Alex Michael.

Following effects have been implemented:
- Brighten
- Saturate
- Gamma
- Adjust Channels
- Expose
- Curves
- Sharpen
- Blur
- Fill
- Subtract
- Sepia
- Contrast
- Posterize
- Invert
- Alpha
- Vignette
- Noise

The library also supports photoshop like layer blending modes:
- Multiply
- Screen
- Overlay
- Soft Light
- Addition
- Exclusion
- Difference

=====================================
##Using NETFx

**Set Up**

- You need to add five classes (effects.cs,layers.cs,Util.cs,Bitmap.cs,LockBitmap.cs) to your project in order to use NETFx. These classes are based on .NET 4.0 and are independant of any third party library.

- After that, just declare an object of both effects 'class' and 'layers' class:

```c#
effects effects = new effects(); //contains all the basic effects 
layers layers = new layers(); //photoshop like layer implementation
```

- You need to use my wrapper around the .net Bitmap when applying effects, you can use your old bitmap(if you really have to) like this:

```c#
Bitmap oldBMP = new Bitmap("megan.jpg"); //this is .NET Bitmap
BitmapW BetterBitmap = new BitmapW(oldBMP); //copy the old bitmap into this one.
```

Otherwise you can do the following too:

```c#
BitmapW BetterBitmap = new BitmapW("megan.jpg"); //load from file
```

**Effects and Layers**

- After you have a bitmap and some objects ready to use, try some effects:

```c#
BitmapW a = new BitmapW("D:\megan.jpg"); //load an image
a = effects.sepia(a); //apply the effect
pictureBox1.Image = a.GetBitmap(); //show the resulting image
```

- You can also merge two bitmaps with some blending options(like in photoshop):

```c#
BitmapW a = new BitmapW("D:\megan.jpg"); //load an image
BitmapW a = new BitmapW("D:\glow.jpg"); //load another image
a = layers.merge("softLight",a,b); //merge 'b' onto 'a' as soft light
pictureBox1.Image = a.GetBitmap(); //show the resulting image
```



**Sample code in C#**

```c#
effects effects = new effects(); //contains all the basic effects 
layers layers = new layers(); //photoshop like layer implementation

BitmapW a = new BitmapW("D:\megan.jpg"); //load an image
a = effects.sepia(a); //apply the effect
pictureBox1.Image = a.GetBitmap(); //show the resulting image
```

a more complex sample:

```c#
BitmapW a = new BitmapW(openFileDialog1.FileName); //load a bitmap

BitmapW layer1 = a.Clone(), layer2 = a.Clone(), layer3 = a.Clone(); //make three copies of the image l1,l2,l3
BitmapW res = a.Clone(); //this will hold the final result

layer3 = effects.fill(layer3, 167, 118, 12); //layer 3 is a solid fill of this rgb color

layer2 = effects.blur(layer2, "gaussian"); //layer2 is a blurred version of original image
layer1 = effects.saturate(layer1, -50); //layer1 is a saturated version


res = layers.merge("overlay", res, layer1); //merge layer1 onto original with "overlay" layer blending
res = layers.merge("softLight", res, layer2); //now merge layer2 onto this, with "softlight" layer blending
res = layers.merge("softLight", res, layer3); //now merge layer3 onto this, with "softlight" layer blending
res = effects.saturate(res,-40); //apply -40 saturate effect on this now
res = effects.contrast(res, 10); //apply 10 contrast 

pictureBox2.Image = res.GetBitmap(); //show the resulting image
```

=====================================
