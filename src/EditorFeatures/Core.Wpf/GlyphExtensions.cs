using StarkPlatform.CodeAnalysis.Editor.Shared.Extensions;
using Microsoft.VisualStudio.Imaging.Interop;

namespace StarkPlatform.CodeAnalysis.Editor.Wpf
{
    internal static class GlyphExtensions
    {
        public static ImageMoniker GetImageMoniker(this Glyph glyph)
        {
            var imageId = glyph.GetImageId();
            return new ImageMoniker()
            {
                Guid = imageId.Guid,
                Id = imageId.Id
            };
        }
    }
}
