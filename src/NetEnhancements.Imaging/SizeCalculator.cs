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

            if (originalWidth == originalHeight && desiredWidth == desiredHeight)
            {
                // Image and bounding box are square.
                // No need to calculate aspects, just downsize it with the bounding box.
                return new(desiredWidth, desiredHeight);
            }

            if (originalWidth == originalHeight)
            {
                // Image is square, bounding box isn't.
                // Get smallest side of bounding box.
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
                    // Image is wider and taller than bounding box.
                    // Figure out which bounding box dimension is the smallest and which original image dimension is the smallest, already know original image is larger than bounding box.
                    // Will downscale the original image by an aspect ratio to fit in the bounding box at the maximum size within aspect ratio.
                    return (float)Math.Min(desiredWidth, desiredHeight) / Math.Min(originalWidth, originalHeight);
                }

                // Image is wider than bounding box.
                // One dimension (width) so calculate the aspect ratio between the bounding box width and original image width.
                return (float)desiredWidth / originalWidth;
            }

            // Image is taller than bounding box.
            return (float)desiredHeight / originalHeight;
        }
    }
}
