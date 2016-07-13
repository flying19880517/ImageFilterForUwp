using HaoRan.ImageFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ImageFilterForUwp
{
    /// <summary>
    /// A generic effect item.
    /// </summary>
    public class EffectItem
    {
        public IImageFilter Effect { get; private set; }
        public string Name { get; private set; }
        public ImageSource Thumbnail { get; set; }

        public EffectItem(IImageFilter effect)
        {
            Effect = effect;
        }

        public EffectItem(IImageFilter effect, string thumbnailRelativeResourcePath)
           : this(effect)
        {
            // Load the thumbnail from the resource stream using the WriteableBitmapEx lib
            Thumbnail = new BitmapImage(new Uri("ms-appx://" + thumbnailRelativeResourcePath, System.UriKind.RelativeOrAbsolute));

            Name = effect.GetType().Name;
        }

        public EffectItem(IImageFilter effect, string thumbnailRelativeResourcePath, string name)
           : this(effect, thumbnailRelativeResourcePath)
        {
            Name = name;
        }
    }
}
