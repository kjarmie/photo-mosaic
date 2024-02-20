# Hyperboliq Assessments
The goal of the assignment is to use an existing input image that you can upload via an application / 
stack of your choice, break it up into a bunch of tiles (segments), and replace every single tile in the 
image with an image from our image dataset (you can store it in the assets folder). 

You will be left with a new image that looks more or less like the original but made up of a large number
of different smaller images.

## Functional requirements:

- Upload image
- Calculate avg RGB for each tile image from our assets folder (Avg R, Avg G, Avg B)
- Divide our input image in 20x20 parts. (You can change this however you like)
- Calculate the avg RGB for each of the 400 parts in our input image.
- Calculate the distance between every tile (AVG RGB) and every part of our image (AVG RGB):
- Choose the tiles with the smallest distance, resize them and replace that image part with the tile
- Display output image.

## Use the following resources:
Image data set [https://data.caltech.edu/records/mzrjq-6wc02]

## Limitations:
We don't want to use euclidian distance to calculate our distances between colours since this does not take human colour perception into account. Instead let's use "Delta E* CIE" and then use these transformations to go from RGB-> CIE-L*ab to do the calculation. http://www.easyrgb.com/en/math.php

## Upon completion:
Please add your work to a git repository with a descriptive README file.
Make repo public and send a link to the interviewers, you can also just keep the repo private and add the interviewers as contributors to the repo if you'd prefer that.