namespace NetEnhancements.Imaging
{
    /// <summary>
    /// Calculates image sizes.
    /// </summary>
    public static class SizeCalculator
    {
        /// <summary>
        /// Returns the optimal new resolution given an <paramref name="originalResolution"/> and a <paramref name="desiredResolution"/>.
        /// </summary>
        public static Resolution GetRelativeSize(Resolution originalResolution, Resolution desiredResolution)
        {
            originalResolution.Deconstruct(out int originalWidth, out int originalHeight);
            desiredResolution.Deconstruct(out int desiredWidth, out int desiredHeight);

            if (desiredWidth < 2 || desiredHeight < 2)
            {
                throw new InvalidOperationException("Target size must be 2x2 pixels or larger.");
            }

            if (originalWidth < desiredWidth && originalHeight < desiredHeight)
            {
                //image fits in bounding box, keep size
                //If we made it bigger it would stretch the image resulting in loss of quality.
                return new(originalWidth, originalHeight);
            }

            if (originalWidth == originalHeight && desiredWidth == desiredHeight)
            {
                //image and bounding box are square, no need to calculate aspects, just downsize it with the bounding box
                return new(desiredWidth, desiredHeight);
            }

            if (originalWidth == originalHeight)
            {
                // Image is square, bounding box isn't.
                // Get smallest side of bounding box and resize to a square of that center the image vertically and horizontally with Css there will be space on one side.
                int smallSide = Math.Min(desiredWidth, desiredHeight);

                return new(smallSide, smallSide);
            }

            // Non-squares: figure out resizing within aspect ratios
            float ratio = GetRatio(originalWidth, originalHeight, desiredWidth, desiredHeight);

            return new((int)Math.Ceiling(originalWidth * ratio), (int)Math.Ceiling(originalHeight * ratio));
        }

        private static float GetRatio(int originalWidth, int originalHeight, int desiredWidth, int desiredHeight)
        {
            if (originalWidth > desiredWidth)
            {
                if (originalHeight > desiredHeight)
                {
                    //image is wider and taller than bounding box
                    //two dimensions so figure out which bounding box dimension is the smallest and which original image dimension is the smallest, already know original image is larger than bounding box
                    //will downscale the original image by an aspect ratio to fit in the bounding box at the maximum size within aspect ratio.
                    return (float)Math.Min(desiredWidth, desiredHeight) / Math.Min(originalWidth, originalHeight);
                }

                //image is wider than bounding box
                //one dimension (width) so calculate the aspect ratio between the bounding box width and original image width
                //downscale image by r to fit in the bounding box...
                return (float)desiredWidth / originalWidth;
            }

            //original image is taller than bounding box
            return (float)desiredHeight / originalHeight;
        }
    }
}
