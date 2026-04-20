using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Gotcha.Core.Services.ValidationServices
{
    public static class ImageValidationService
    {
        internal static bool IsAllowedImageUrl(string? url)
        {
            // Null or empty is allowed — the image is optional
            if (string.IsNullOrEmpty(url))
            {
                return true;
            }

            /* Uri.TryCreate tries to parse the string into a Uri object
             * UriKind.Absolute means it must be a full URL (like "https://example.com/photo.jpg")
             *   — relative paths like "/images/photo.jpg" would fail here
             * "out var uri" stores the parsed result so we can inspect it below
             * If parsing fails (invalid URL), we reject it */
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
                return false;

            /* uri.Scheme is the protocol part of the URL (the bit before "://")
             * For example: "https://example.com" → scheme is "https"
             *              "ftp://files.com"     → scheme is "ftp"
             * Uri.UriSchemeHttp = "http", Uri.UriSchemeHttps = "https"
             * We only allow http and https — this blocks dangerous schemes like:
             *   "javascript:" (XSS attacks), "file:" (local file access), "ftp:", etc. */
            if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
            {
                return false;
            }

            return true;
        }

        // --- Profile Image File Validation ---

        // 8 MB max file size
        public const long MaxImageSizeInBytes = 8 * 1024 * 1024;

        // Dimension limits for uploaded images
        public const int MinImageDimension = 200;
        public const int MaxImageDimension = 5000;
        public const int ProfileImageSize = 1500;

        private static readonly HashSet<string> AllowedImageExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp"
        };

        private static readonly HashSet<string> AllowedImageMimeTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "image/jpeg",
            "image/png",
            "image/webp"
        };

        /// <summary>
        /// Checks if the file extension is one of the allowed image types (.jpg, .jpeg, .png, .webp).
        /// </summary>
        public static bool IsAllowedFileExtension(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return false;

            string extension = Path.GetExtension(fileName);
            return AllowedImageExtensions.Contains(extension);
        }

        /// <summary>
        /// Checks if the MIME type is one of the allowed image types (image/jpeg, image/png, image/webp).
        /// </summary>
        public static bool IsAllowedMimeType(string contentType)
        {
            if (string.IsNullOrWhiteSpace(contentType))
                return false;

            return AllowedImageMimeTypes.Contains(contentType.Trim());
        }

        /// <summary>
        /// Checks if the file size is within the allowed limit (5 MB).
        /// </summary>
        public static bool IsAllowedFileSize(long fileSize)
        {
            return fileSize > 0 && fileSize <= MaxImageSizeInBytes;
        }

        /// <summary>
        /// Checks that the file's magic bytes match the expected format based on the extension.
        /// This prevents someone from renaming a .exe to .jpg and uploading it.
        ///
        /// Magic bytes (file signatures):
        ///   JPEG: starts with FF D8 FF
        ///   PNG:  starts with 89 50 4E 47
        ///   WEBP: starts with 52 49 46 46 (RIFF), then bytes 8-11 are 57 45 42 50 (WEBP)
        /// </summary>
        public static bool HasValidImageSignature(byte[] fileHeader, string fileName)
        {
            if (fileHeader == null || fileHeader.Length < 12)
                return false;

            if (string.IsNullOrWhiteSpace(fileName))
                return false;

            string extension = Path.GetExtension(fileName).ToLowerInvariant();

            if (extension == ".jpg" || extension == ".jpeg")
            {
                // JPEG magic bytes: FF D8 FF
                return fileHeader[0] == 0xFF
                    && fileHeader[1] == 0xD8
                    && fileHeader[2] == 0xFF;
            }

            if (extension == ".png")
            {
                // PNG magic bytes: 89 50 4E 47
                return fileHeader[0] == 0x89
                    && fileHeader[1] == 0x50
                    && fileHeader[2] == 0x4E
                    && fileHeader[3] == 0x47;
            }

            if (extension == ".webp")
            {
                // WEBP: starts with RIFF (52 49 46 46), bytes 8-11 are WEBP (57 45 42 50)
                bool isRiff = fileHeader[0] == 0x52
                           && fileHeader[1] == 0x49
                           && fileHeader[2] == 0x46
                           && fileHeader[3] == 0x46;

                bool isWebp = fileHeader[8] == 0x57
                           && fileHeader[9] == 0x45
                           && fileHeader[10] == 0x42
                           && fileHeader[11] == 0x50;

                return isRiff && isWebp;
            }

            return false;
        }

        /// <summary>
        /// Checks if the image dimensions are within the allowed range.
        /// Returns (isValid, width, height) so the caller can use the dimensions without loading the image again.
        /// </summary>
        public static (bool IsValid, int Width, int Height) ValidateImageDimensions(Stream inputStream)
        {
            // Identify reads only the image header — much cheaper than loading the full image
            ImageInfo? imageInfo = Image.Identify(inputStream);

            if (imageInfo == null)
            {
                return (false, 0, 0);
            }

            int width = imageInfo.Width;
            int height = imageInfo.Height;

            bool isTooSmall = width < MinImageDimension || height < MinImageDimension;
            bool isTooLarge = width > MaxImageDimension || height > MaxImageDimension;

            return (!isTooSmall && !isTooLarge, width, height);
        }

        /// <summary>
        /// Re-encodes the uploaded image as JPEG using ImageSharp.
        /// This is the final safety net: it strips all EXIF/GPS metadata and proves
        /// the file is a real image (ImageSharp will throw if it can't decode it).
        /// Returns a clean MemoryStream with the re-encoded JPEG.
        /// </summary>
        internal static MemoryStream ReEncodeImage(Stream inputStream)
        {
            var outputStream = new MemoryStream();

            // Load will throw if the stream is not a valid image
            using (Image image = Image.Load(inputStream))
            {
                // Re-save as JPEG — this strips all metadata (EXIF, GPS, etc.)
                var encoder = new JpegEncoder
                {
                    Quality = 85
                };

                image.Save(outputStream, encoder);
            }

            // Reset position so the caller can read from the start
            outputStream.Position = 0;
            return outputStream;
        }

        /// <summary>
        /// Resizes the image to 1500x1500 and re-encodes as JPEG.
        /// The image should already be cropped to a square by the frontend.
        /// If not perfectly square, it resizes to fit 1500x1500 (may stretch slightly).
        /// Strips all metadata (EXIF, GPS, etc.) as a side effect of re-encoding.
        /// </summary>
        public static MemoryStream ResizeAndReEncodeImage(Stream inputStream)
        {
            var outputStream = new MemoryStream();

            using (Image image = Image.Load(inputStream))
            {
                image.Mutate(x => x.Resize(ProfileImageSize, ProfileImageSize));

                var encoder = new JpegEncoder
                {
                    Quality = 85
                };

                image.Save(outputStream, encoder);
            }

            outputStream.Position = 0;
            return outputStream;
        }
    }
}
