# fileview
 View a file... as an image!

## Sources
(as of inital release)
### FileView.exe in RGB
![](https://i.imgur.com/OiuBSMO.png)

### FileView.exe in RGBA
![](https://i.imgur.com/pWzZ6NS.png)

## Command Line Arguments
1. input file to be turned into an image
2. output file for the image to be placed in

### Optional
Optional arguments must be put after mandatory arguments
	
* `--alpha` uses the alpha channel in the image.
* `--decode` decodes an image back into its original data. This cannot be used with `--alpha` for technical reasons.