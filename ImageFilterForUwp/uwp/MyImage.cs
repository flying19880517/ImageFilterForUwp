/* 
 * HaoRan ImageFilter Classes v0.1
 * Copyright (C) 2012 Zhenjun Dai
 *
 * This library is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as published by the
 * Free Software Foundation; either version 2.1 of the License, or (at your
 * option) any later version.
 *
 * This library is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU Lesser General Public License
 * for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this library; if not, write to the Free Software Foundation.
 */


using ImageFilterForUwp;
using System;
using System.Windows;
using Windows.UI.Xaml.Media.Imaging;

namespace HaoRan.ImageFilter
{
    public class MyImage
    {
        //original bitmap image
        public WriteableBitmap image;
        // public WriteableBitmap destImage;

        //format of image (jpg/png)
        private String formatName;
        //dimensions of image
        private int width, height;
        // RGB Array Color
        public int[] colorArray;

        public MyImage(WriteableBitmap img)
        {
            //this.image = img.Clone(); // 与 SilverLight 中的 WriteableBitmap类不同,这里使用下面自定义方法
            //InitImage(img);//初始化拷贝

            this.image = img;

            formatName = "jpg";
            width = img.PixelWidth;
            height = img.PixelHeight;

            updateColorArray();
        }

        /// <summary>
        /// 移植到 uwp 时，由于 Windows runtime 中的 WriteableBitmap类没有 Clone()方法，这里自定义拷贝方法
        /// </summary>
        /// <param name="bitmap"></param>
        async void InitImage(WriteableBitmap bitmap)
        {
            this.image = await ImageFilterForUwp.Utility.BitmapClone(bitmap);
        }

        /* 移植到 uwp 
        */
        public MyImage clone()
        {
            return new MyImage(this.image);
        }

        /**
         * Method to reset the image to a solid color
         * @param color - color to rest the entire image to
         */
        public void clearImage(int color)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    setPixelColor(x, y, color);
                }
            }
        }


        /**
         * Set color array for image - called on initialisation
         * by constructor
         * 
         * @param bitmap
         */
        private void updateColorArray()
        {
            /* 原 wp7 中代码，在 uwp 工程中进行适配
            colorArray = image.Pixels;
            int r, g, b;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;
                    r = (colorArray[index] >> 16) & 0xff;
                    g = (colorArray[index] >> 8) & 0xff;
                    b = colorArray[index] & 0xff;
                    colorArray[index] = (255 << 24) | (r << 16) | (g << 8) | b;
                }
            }
            */

            byte[] bytes = Utility.Get_WriteableBitmap_bytes(this.image);
            
            colorArray = Utility.ConvertToIntArray(bytes, height, width);
        }


        /**
         * Method to set the color of a specific pixel
         * 
         * @param x
         * @param y
         * @param color
         */
        public void setPixelColor(int x, int y, int color)
        {
            colorArray[((y * image.PixelWidth + x))] = color;
            //image.setPixel(x, y, color);
            //destImage.setPixel(x, y, colorArray[((y*image.getWidth()+x))]);
        }

        /**
         * Get the color for a specified pixel
         * 
         * @param x
         * @param y
         * @return color
         */
        public int getPixelColor(int x, int y)
        {
            return colorArray[y * width + x];
        }

        /**
         * Set the color of a specified pixel from an RGB combo
         * 
         * @param x
         * @param y
         * @param c0
         * @param c1
         * @param c2
         */
        public void setPixelColor(int x, int y, int c0, int c1, int c2)
        {
            int rgbcolor = (255 << 24) + (c0 << 16) + (c1 << 8) + c2;
            colorArray[((y * image.PixelWidth + x))] = rgbcolor;
            //int array = ((y*image.getWidth()+x));

            //vbb.order(ByteOrder.nativeOrder());
            //vertexBuffer = vbb.asFloatBuffer();
            //vertexBuffer.put(vertices);
            //vertexBuffer.position(0);

            //image.setPixel(x, y, colorArray[((y*image.getWidth()+x))]);
        }

        /* 移植到 uwp 时，该方法没用到
        public void copyPixelsFromBuffer()
        { //从缓冲区中copy数据以加快像素处理速度
           // this.destImage.Dispatcher.BeginInvoke(() =>
           // {
                //var result = new WriteableBitmap(width, height);
                Buffer.BlockCopy(colorArray, 0, destImage.Pixels, 0, colorArray.Length * 4);
                //return result;
           // });
        }
        */

        /**
         * Method to get the RED color for the specified 
         * pixel 
         * @param x
         * @param y
         * @return color of R
         */
        public int getRComponent(int x, int y)
        {
            return (getColorArray()[((y * width + x))] & 0x00FF0000) >> 16;
        }


        /**
         * Method to get the GREEN color for the specified 
         * pixel 
         * @param x
         * @param y
         * @return color of G
         */
        public int getGComponent(int x, int y)
        {
            return (getColorArray()[((y * width + x))] & 0x0000FF00) >> 8;
        }


        /**
         * Method to get the BLUE color for the specified 
         * pixel 
         * @param x
         * @param y
         * @return color of B
         */
        public int getBComponent(int x, int y)
        {
            return (getColorArray()[((y * width + x))] & 0x000000FF);
        }



        /* 移植到 uwp 时，该方法没用到
        *
         * @return the image
         
        public WriteableBitmap getImage()
        {
            //return image;
            return destImage;
        }
        */


        /**
         * @param image the image to set
         */
        public void setImage(WriteableBitmap image)
        {
            this.image = image;
        }


        /**
         * @return the formatName
         */
        public String getFormatName()
        {
            return formatName;
        }


        /**
         * @param formatName the formatName to set
         */
        public void setFormatName(String formatName)
        {
            this.formatName = formatName;
        }


        /* 
         * @return the width
        */
        public int getWidth()
        {
            return width;
        }


        /*
         * @param width the width to set
         */
        public void setWidth(int width)
        {
            this.width = width;
        }


        /* 
         * @return the height
         */
        public int getHeight()
        {
            return height;
        }


        /*
         * @param height the height to set
         */
        public void setHeight(int height)
        {
            this.height = height;
        }


        /*移植到 uwp 时，该方法没用到
         * @return the colorArray
         */
        public int[] getColorArray()
        {
            return colorArray;
        }


        /*移植到 uwp 时，该方法没用到
         * @param colorArray the colorArray to set
         
        public void setColorArray(int[] colorArray)
        {
            this.colorArray = colorArray;
        }*/

        public static int SAFECOLOR(int a)
        {
            if (a < 0)
                return 0;
            else if (a > 255)
                return 255;
            else
                return a;
        }

        public static int rgb(int r, int g, int b)
        {
            return (255 << 24) + (r << 16) + (g << 8) + b;
        }

        [Obsolete("移植到 uwp 时，该方法不适用")]
        public static MyImage LoadImage(string path)
        {
            /*
            Uri uri = new Uri(path, UriKind.Relative);
            StreamResourceInfo resourceInfo = Application.GetResourceStream(uri);
            BitmapImage bmp = new BitmapImage();
            bmp.SetSource(resourceInfo.Stream);
            return new Image(new WriteableBitmap(bmp));
            */

            return null;
        }

    }
}
