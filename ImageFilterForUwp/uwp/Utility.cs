using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace ImageFilterForUwp
{
    class Utility
    {
        public static byte[] ConvertToByteArray(int[] intArray)
        {
            if (intArray.Length > 0)
            {
                byte[] result = new byte[intArray.Length * 4];

                int j = 0;
                for (int i = 0; i < intArray.Length; i++)
                {
                    // b g r a   vs  argb
                    result[j++] = (byte)(intArray[i]); // b
                    result[j++] = (byte)(intArray[i] >> 8); // g
                    result[j++] = (byte)(intArray[i] >> 16); // r
                    result[j++] = (byte)(intArray[i] >> 24); // a
                }

                return result;
            }

            return null;
        }

        public static int[] ConvertToIntArray(byte[] byteArray,int height, int width)
        {
            if (byteArray.Length > 0)
            {
                int[] result = new int[byteArray.Length / 4];
                
                int a, r, g, b;
                int i = 0;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int index = y * width + x;
                        b = byteArray[i++];
                        g = byteArray[i++];
                        r = byteArray[i++];
                        a = byteArray[i++];
                        result[index] = (a << 24) | (r << 16) | (g << 8) | b;
                    }
                }

                return result;
            }
            return null;
        }

        public static async Task<WriteableBitmap> BitmapClone(WriteableBitmap bitmap)
        {
            WriteableBitmap result = new WriteableBitmap(bitmap.PixelWidth, bitmap.PixelHeight);

            byte[] sourcePixels = Get_WriteableBitmap_bytes(bitmap);

            using (Stream resultStream = bitmap.PixelBuffer.AsStream())
            {
                await resultStream.WriteAsync(sourcePixels, 0, sourcePixels.Length);
            }

            return result;
        }

        public static byte[] Get_WriteableBitmap_bytes(WriteableBitmap bitmap)
        {

            IBuffer bitmapBuffer = bitmap.PixelBuffer;

            //byte[] sourcePixels = new byte[bitmapBuffer.Length];

            //Windows.Security.Cryptography.CryptographicBuffer.CopyToByteArray(bitmapBuffer, out sourcePixels);

            //bitmapBuffer.CopyTo(sourcePixels);

            using (var dataReader = DataReader.FromBuffer(bitmapBuffer))
            {
                var bytes = new byte[bitmapBuffer.Capacity];
                dataReader.ReadBytes(bytes);

                return bytes;
            }
        }
    }
}
