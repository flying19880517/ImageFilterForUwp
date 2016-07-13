using HaoRan.ImageFilter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace ImageFilterForUwp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class EffectPage : Page
    {
        public EffectPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            EffectItem effectItem = e.Parameter as EffectItem;

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///uwp/Sample.jpg"));

            var wb = new WriteableBitmap(1000, 1000);
            img.Source = wb;

            // Set the source of the WriteableBitmap to the image stream
            using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
            {
                try
                {
                    await wb.SetSourceAsync(fileStream);
                }
                catch (TaskCanceledException)
                {
                    // The async action to set the WriteableBitmap's source may be canceled if the user clicks the button repeatedly
                }
            }

            MyImage myImage = new MyImage(wb);
            //await myImage.InitImage(wb);

            //GaussianBlurFilter filter = new GaussianBlurFilter();
            //filter.Sigma = 40;
            //filter.process(myImage);
            //int[] array = myImage.colorArray;

            //await Task.Delay(1000);

            effectItem.Effect.process(myImage);

            byte[] result = Utility.ConvertToByteArray(myImage.colorArray);

            // Open a stream to copy the image contents to the WriteableBitmap's pixel buffer 
            using (Stream stream = wb.PixelBuffer.AsStream())
            {
                await stream.WriteAsync(result, 0, result.Length);
            }

            // 用像素缓冲区的数据绘制图片
            wb.Invalidate();

            base.OnNavigatedTo(e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
