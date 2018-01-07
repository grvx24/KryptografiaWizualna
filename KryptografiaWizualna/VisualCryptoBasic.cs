using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KryptografiaWizualna
{
    class VisualCryptoBasic
    {
        public static int[,] pixelsMatrix1 = new int[,] { { 0,1,0,1 },{ 1,0,1,0 },{0,0,1,1 },
                                                    {1,1,0,0 },{0,1,1,0 },{1,0,0,1 } };

        public static int[,] pixelsMatrix2 = new int[,] { { 1,0,1,0 },{ 0,1,0,1 },{1,1,0,0 },
                                                    {0,0,1,1 },{1,0,0,1 },{0,1,1,0 } };


        //public static int[,] pixelsMatrix1_3components = new int[,] { { 0,0,0,1,1,1,0,1,1 },
        //    { 0,0,1,1,0,1,0,1,1 },{0,0,1,0,0,1,1,1,1},
        //    {0,0,1,0,1,1,0,1,1 },{0,0,1,1,0,0,1,1,1},{0,0,0,0,1,1,1,1,1},
        //{0,0,0,1,1,0,1,1,1 },{0,0,1,1,1,0,1,0,1 },{0,0,1,1,0,1,1,0,1 } };

        //public static int[,] pixelsMatrix2_3components = new int[,] { { 0,0,0,0,1,1,1,1,1 },
        //    { 1,1,1,1,0,1,0,0,0 },{1,0,0,1,1,1,0,1,0 },
        //    {0,1,0,0,1,0,1,1,1 },{1,0,1,0,1,1,0,1,0},{1,0,1,0 } };

        //public static int[,] pixelsMatrix3_3components = new int[,] { { 0,0,1,1 },{ 1,1,0,0 },{1,0,0,1 },
        //                                            {0,1,0,1 },{1,0,0,1 },{1,0,1,0 } };


        private Bitmap bmp;
        public bool ImageLoaded = false;

        private Color MapToColor(int value, bool whiteAsTransparent = false)
        {
            if (value == 0)
            {
                return Color.Black;
            }
            else
            {
                if (whiteAsTransparent)
                    return Color.Transparent;
                else
                    return Color.White;
            }
        }

        private Color MapToColorReversed(int value, bool whiteAsTransparent = false)
        {
            if (value == 1)
            {
                return Color.Black;
            }
            else
            {
                if (whiteAsTransparent)
                    return Color.Transparent;
                else
                    return Color.White;
            }
        }

        public Bitmap AssembleComponents(Bitmap[] components,int numOfSubpixels,int numOfComponents, bool removeNoise = false)
        {

            if (components[0].Size != components[1].Size)
            {
                throw new ArgumentException("Obrazy muszą być tej samej wielkości!");
            }

            if(numOfSubpixels == 2)
            {
                int width = components[0].Width;
                int height = components[0].Height;
                Console.WriteLine(width + " " + height);

                Bitmap result = new Bitmap(width, height);
                
                for (int i = 0; i < height; i ++)
                {
                    for (int j = 0; j < width; j += 2)
                    {
                        bool pixelBlockIsEqual = true;

                        Color[] c1 = new Color[2];
                        c1[0] = components[0].GetPixel(j, i);
                        c1[1] = components[0].GetPixel(j + 1, i);

                        Color[] c2 = new Color[2];
                        c2[0] = components[1].GetPixel(j, i);
                        c2[1] = components[1].GetPixel(j + 1, i);

                        for (int k = 0; k < c1.Length; k++)
                        {
                            if (c1[k] != c2[k])
                            {
                                pixelBlockIsEqual = false;
                            }
                        }

                        if (pixelBlockIsEqual)
                        {
                            if (!removeNoise)
                            {
                                result.SetPixel(j, i, c1[0]);
                                result.SetPixel(j + 1, i, c1[1]);
                            }
                            else
                            {
                                result.SetPixel(j, i, Color.White);
                                result.SetPixel(j + 1, i, Color.White);
                            }

                        }
                        else
                        {
                            result.SetPixel(j, i, Color.Black);
                            result.SetPixel(j + 1, i, Color.Black);
                        }
                    }
                }

                return result;
            }
            else 
            {
                int width = components[0].Width;
                int height = components[0].Height;
                Console.WriteLine(width + " " + height);

                Bitmap result = new Bitmap(width, height);

                for (int i = 0; i < height; i += 2)
                {
                    for (int j = 0; j < width; j += 2)
                    {
                        bool pixelBlockIsEqual = true;

                        Color[] c1 = new Color[4];
                        c1[0] = components[0].GetPixel(j, i);
                        c1[1] = components[0].GetPixel(j + 1, i);
                        c1[2] = components[0].GetPixel(j, i + 1);
                        c1[3] = components[0].GetPixel(j + 1, i + 1);

                        Color[] c2 = new Color[4];
                        c2[0] = components[1].GetPixel(j, i);
                        c2[1] = components[1].GetPixel(j + 1, i);
                        c2[2] = components[1].GetPixel(j, i + 1);
                        c2[3] = components[1].GetPixel(j + 1, i + 1);

                        for (int k = 0; k < c1.Length; k++)
                        {
                            if (c1[k] != c2[k])
                            {
                                pixelBlockIsEqual = false;
                            }
                        }

                        if (pixelBlockIsEqual)
                        {
                            if (!removeNoise)
                            {
                                result.SetPixel(j, i, c1[0]);
                                result.SetPixel(j + 1, i, c1[1]);
                                result.SetPixel(j, i + 1, c1[2]);
                                result.SetPixel(j + 1, i + 1, c1[3]);
                            }
                            else
                            {
                                result.SetPixel(j, i, Color.White);
                                result.SetPixel(j + 1, i, Color.White);
                                result.SetPixel(j, i + 1, Color.White);
                                result.SetPixel(j + 1, i + 1, Color.White);
                            }


                        }
                        else
                        {
                            result.SetPixel(j, i, Color.Black);
                            result.SetPixel(j + 1, i, Color.Black);
                            result.SetPixel(j, i + 1, Color.Black);
                            result.SetPixel(j + 1, i + 1, Color.Black);
                        }


                    }
                }

                return result;
            }

        }

        public Bitmap[] Encrypt2PixVersion(int thresholdR = 127, int thresholdG = 127, int thresholdB = 127, bool whiteAsTransparent = false)
        {
            if (thresholdR < 0 || thresholdR > 255)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (thresholdG < 0 || thresholdG > 255)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (thresholdB < 0 || thresholdB > 255)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (bmp == null)
            {
                throw new NullReferenceException("Nie wczytano obrazu!");
            }

            int width = bmp.Width;
            int height = bmp.Height;

            Bitmap[] components = new Bitmap[] { new Bitmap(width * 2, height), new Bitmap(width * 2, height) };

            Random rng = new Random();

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Color c = bmp.GetPixel(j, i);

                    if (c.R > thresholdR && c.G > thresholdG && c.B > thresholdB)
                    {
                        int randomIndex = rng.Next(0, 2);

                        components[0].SetPixel(j * 2, i, MapToColor(randomIndex, whiteAsTransparent));
                        components[0].SetPixel(j * 2 + 1, i, MapToColor(~randomIndex&1, whiteAsTransparent));
      
                        components[1].SetPixel(j * 2, i, MapToColor(randomIndex, whiteAsTransparent));
                        components[1].SetPixel(j * 2 + 1, i, MapToColor(~randomIndex & 1, whiteAsTransparent));
                    }
                    else
                    {
                        int randomIndex = rng.Next(0, 2);
                        components[0].SetPixel(j * 2, i , MapToColor(randomIndex, whiteAsTransparent));
                        components[0].SetPixel(j * 2 + 1, i , MapToColor(~randomIndex & 1, whiteAsTransparent));

                        components[1].SetPixel(j * 2, i, MapToColor(~randomIndex & 1, whiteAsTransparent));
                        components[1].SetPixel(j * 2 + 1, i, MapToColor(randomIndex, whiteAsTransparent));

                    }
                }
            }


            return components;
        }


        public Bitmap[] Encrypt(int thresholdR = 127, int thresholdG = 127, int thresholdB = 127, bool whiteAsTransparent = false)
        {
            if (thresholdR < 0 || thresholdR > 255)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (thresholdG < 0 || thresholdG > 255)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (thresholdB < 0 || thresholdB > 255)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (bmp == null)
            {
                throw new NullReferenceException("Nie wczytano obrazu!");
            }

            int width = bmp.Width;
            int height = bmp.Height;

            Bitmap[] components = new Bitmap[] { new Bitmap(width * 2, height * 2), new Bitmap(width * 2, height * 2) };

            Random rng = new Random();

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Color c = bmp.GetPixel(j, i);

                    if (c.R > thresholdR && c.G > thresholdG && c.B > thresholdB)
                    {
                        int randomIndex = rng.Next(0, 6);
                        components[0].SetPixel(j * 2, i * 2, MapToColor(pixelsMatrix1[randomIndex, 0], whiteAsTransparent));
                        components[0].SetPixel(j * 2 + 1, i * 2, MapToColor(pixelsMatrix1[randomIndex, 1], whiteAsTransparent));
                        components[0].SetPixel(j * 2, i * 2 + 1, MapToColor(pixelsMatrix1[randomIndex, 2], whiteAsTransparent));
                        components[0].SetPixel(j * 2 + 1, i * 2 + 1, MapToColor(pixelsMatrix1[randomIndex, 3], whiteAsTransparent));

                        components[1].SetPixel(j * 2, i * 2, MapToColor(pixelsMatrix1[randomIndex, 0], whiteAsTransparent));
                        components[1].SetPixel(j * 2 + 1, i * 2, MapToColor(pixelsMatrix1[randomIndex, 1], whiteAsTransparent));
                        components[1].SetPixel(j * 2, i * 2 + 1, MapToColor(pixelsMatrix1[randomIndex, 2], whiteAsTransparent));
                        components[1].SetPixel(j * 2 + 1, i * 2 + 1, MapToColor(pixelsMatrix1[randomIndex, 3], whiteAsTransparent));

                    }
                    else
                    {
                        int randomIndex = rng.Next(0, 6);
                        components[0].SetPixel(j * 2, i * 2, MapToColor(pixelsMatrix1[randomIndex, 0], whiteAsTransparent));
                        components[0].SetPixel(j * 2 + 1, i * 2, MapToColor(pixelsMatrix1[randomIndex, 1], whiteAsTransparent));
                        components[0].SetPixel(j * 2, i * 2 + 1, MapToColor(pixelsMatrix1[randomIndex, 2], whiteAsTransparent));
                        components[0].SetPixel(j * 2 + 1, i * 2 + 1, MapToColor(pixelsMatrix1[randomIndex, 3], whiteAsTransparent));

                        components[1].SetPixel(j * 2, i * 2, MapToColor(pixelsMatrix2[randomIndex, 0], whiteAsTransparent));
                        components[1].SetPixel(j * 2 + 1, i * 2, MapToColor(pixelsMatrix2[randomIndex, 1], whiteAsTransparent));
                        components[1].SetPixel(j * 2, i * 2 + 1, MapToColor(pixelsMatrix2[randomIndex, 2], whiteAsTransparent));
                        components[1].SetPixel(j * 2 + 1, i * 2 + 1, MapToColor(pixelsMatrix2[randomIndex, 3], whiteAsTransparent));

                    }
                }
            }


            return components;

        }

        public void LoadImage(string path)
        {
            bmp = new Bitmap(Image.FromFile(path));
            ImageLoaded = true;
        }


    }
}
