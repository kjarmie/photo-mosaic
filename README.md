# Overview
This little library can generate photo mosaic images from a source image. There is a library that performs the actual 
creation, and an simple console app to demonstrate the usage. 

The mosaic generation makes use of the Delta E color difference to match parts of the original image to iamges in our data set and then replaces those parts of the image with the image from the dataset with the lowest Delta E between the average RGB of the image part and the dataset.

I have also included a webapp in ASP.NET to showcase the project. It is not currently functional, there are some issues with content type headers, but I have included the example files. 
![image](https://github.com/kjarmie/photo-mosaic/assets/5638804/24e235d9-88b6-4588-b8b8-53624ffe3ab4)
![image](https://github.com/kjarmie/photo-mosaic/assets/5638804/daec8b02-1588-4a2e-bf26-6e2d79b15ed9)
![image](https://github.com/kjarmie/photo-mosaic/assets/5638804/61962ea9-cc3c-466e-bd35-15acb0565418)

---
