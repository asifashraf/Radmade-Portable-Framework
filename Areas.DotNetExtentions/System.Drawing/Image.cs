using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

	public static class ImageFactory
	{
		/*
		 * Saving a Jpeg
The first thing to do here is set up the method signature with the input parameters. 
These are the save file path (string), the Image to save (System.Drawing.Bitmap), and a quality setting (long).
private void saveJpeg(string path, Bitamp img, long quality)
The next few things to do are setting up encoder information for saving the file. 
This includes setting an EncoderParameter for the quality of the Jpeg. 
The next thing is to get the codec information from your computer for jpegs. 
I do this by having a function to loop through the available ones on the computer and making sure jpeg is there. 
The line under that makes sure that the jpeg codec was found on the computer. 
If not it just returns out of the method.
The last thing to do is save the bitmap using the codec and the encoder infomation.
		 */
		public static void SaveJpeg(Bitmap img, string path,  long quality)
		{
			// Encoder parameter for image quality
			EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

			// Jpeg image codec
			ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");

			if (jpegCodec == null)
				return;

			EncoderParameters encoderParams = new EncoderParameters(1);
			encoderParams.Param[0] = qualityParam;

			img.Save(path, jpegCodec, encoderParams);
		}
		public static ImageCodecInfo GetEncoderInfo(string mimeType)
		{
			// Get image codecs for all image formats
			ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

			// Find the correct image codec
			for (int i = 0; i < codecs.Length; i++)
				if (codecs[i].MimeType == mimeType)
					return codecs[i];
			return null;
		}
		//The method takes two objects - the image to crop (System.Drawing.Image) and the rectangle to
		//crop out (System.Drawing.Rectangle). 
		//The next thing done is to create a Bitmap (System.Drawing.Bitmap) of the image. 
		// The only thing left is to crop the image. 
		//This is done by cloning the original image but only taking a rectangle of the original.
		public static Image CropImage(Image img, Rectangle cropArea)
		{
			Bitmap bmpImage = new Bitmap(img);
			Bitmap bmpCrop = bmpImage.Clone(cropArea,
			bmpImage.PixelFormat);
			return (Image)(bmpCrop);
		}
		//This next set of code is a slightly longer and more complex. 
		//The main reason this code is longer is because this resize function will keep the height and width proportional.
		//To start with we see that the input parameters are the image to resize (System.Drawing.Image) and the 
		//size (System.Drawing.Size). Also in this set of code are a few variables we use. 
		//The first two are the source height and width which is used later. 
		//And there are 3 other variables to calculate the proportion information.
		//The next step is to actually figure out what the size of the resized image should be. 
		//The first step is to calculate the percentages of the new size compared to the original. 
		//Next we need to decide which percentage is smaller because 
		//this is the percent of the original image we will use for both height and width. 
		//And now we calculate the number of height and width pixels for the destination image.
		//The final thing to do is create the bitmap (System.Drawing.Bitmap) which we will draw the resized 
		//image on using a Graphics (System.Drawing.Graphics) object. 
		//I also set the interpolation mode, which is the algorithm used to resize the image. 
		//I prefer HighQualityBicubic, which from my testing seems to return the highest quality results. 
		//And just to clean up a little I dispose the Graphics object.
		public static Image ResizeImage(Image imgToResize, Size size)
		{
			int sourceWidth = imgToResize.Width;
			int sourceHeight = imgToResize.Height;

			float nPercent = 0;
			float nPercentW = 0;
			float nPercentH = 0;

			nPercentW = ((float)size.Width / (float)sourceWidth);
			nPercentH = ((float)size.Height / (float)sourceHeight);

			if (nPercentH < nPercentW)
				nPercent = nPercentH;
			else
				nPercent = nPercentW;

			int destWidth = (int)(sourceWidth * nPercent);
			int destHeight = (int)(sourceHeight * nPercent);

			Bitmap b = new Bitmap(destWidth, destHeight);
			Graphics g = Graphics.FromImage((Image)b);
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;

			g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
			g.Dispose();

			return (Image)b;
		}
		public static void ResizeImage(string OriginalFile, string NewFile, int NewWidth, int MaxHeight, bool OnlyResizeIfWider)
		{
			System.Drawing.Image FullsizeImage = System.Drawing.Image.FromFile(OriginalFile);

			// Prevent using images internal thumbnail
			FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
			FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

			if (OnlyResizeIfWider)
			{
				if (FullsizeImage.Width <= NewWidth)
				{
					NewWidth = FullsizeImage.Width;
				}
			}

			int NewHeight = FullsizeImage.Height * NewWidth / FullsizeImage.Width;
			if (NewHeight > MaxHeight)
			{
				// Resize with height instead
				NewWidth = FullsizeImage.Width * MaxHeight / FullsizeImage.Height;
				NewHeight = MaxHeight;
			}

			System.Drawing.Image NewImage = FullsizeImage.GetThumbnailImage(NewWidth, NewHeight, null, IntPtr.Zero);

			// Clear handle to original file so that we can overwrite it if necessary
			FullsizeImage.Dispose();

			// Save resized picture
			NewImage.Save(NewFile);
		}
	}

