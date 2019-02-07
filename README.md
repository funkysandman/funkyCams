# funkyCams

This is a collection of VB/C# .net programs that I use to control various machine vision type cameras.  The cameras involved are USB, GIGE, 1394 FireWire variety.  Cameras touched include SVS Vistek, Photonics HQ2, Allied Vision (AVT), Foculus, Point Grey, Basler models.

These cameras are primarily used outdoors for taking snapshots of the night sky.

I've implemented a [TensorFlow](https://www.tensorflow.org/) object detection model to detect meteors.  The meteor detection program runs as a web service and analyzes pictures coming in from the cameras.

Also, I've written some simple ASCOM drivers to connect a few of the cameras to astrophotography imaging software. 

## To do:
* clean up projects
* rename suitably
* document tensorflow machine learning project

