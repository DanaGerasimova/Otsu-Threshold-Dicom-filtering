using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cartesian_spherical
{
    static class WBit
    {
        public static Bitmap Bitmap(this List<ushort> pixels, int width, int height)
        {
            int id = 0;
            Bitmap bitmap = new Bitmap(width, height);
            int max = pixels.Max();
            int min = pixels.Min();
            int M = max - min;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int ncol = (int)(((double)pixels[id] / M) * 255);
                    if (ncol < 0) ncol = 0;
                    if (ncol > 255) ncol = 255;

                    bitmap.SetPixel(i, j, Color.FromArgb(ncol, ncol, ncol));
                    id++;
                }
            }
            return bitmap;
        }

        public static Bitmap otsu(this Bitmap bitmap)
        {
            Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            int min = bitmap.GetPixel(0, 0).R;
            int max = bitmap.GetPixel(0, 0).R;

            //наибольший и наименьший полутон
            for (int i = 0; i < bitmap.Width; i++)
                for (int j = 0; j < bitmap.Height; j++)
                {
                    int z = bitmap.GetPixel(i, j).R;
                    if (z < min) { min = z; }
                    if (z > max) { max = z; }
                }

            int M = max - min;
            int[] barchart = new int[M + 1];

            for (int i = 0; i <= M; i++)
            {
                barchart[i] = 0;
            }

            int total = bitmap.Width * bitmap.Height;
            //высота столбцов гистограммы
            for (int i = 0; i < bitmap.Width; i++)
                for (int j = 0; j < bitmap.Height; j++)
                {
                    barchart[bitmap.GetPixel(i, j).R - min]++;
                }

            int m = 0;
            int n = 0;

            float sum = 0; // Total
            for (int i = 1; i < M; i++)
            {
                sum += i * barchart[i];
            }

            int rift = 0; //порог
            float sumB = 0; // сумма
            int wB = 0; // фон
            int wF; // полезные
            float mD; // среднее 
            float currentMax, totalMax = 1;
            for (int i = 0; i < M; i++)
            {
                // фон
                wB += barchart[i];
                if (wB == 0) continue;

                // полезные
                wF = total - wB;
                if (wF == 0) break;

                sumB += i * barchart[i];

                // средние
                mD = sumB / wB - (sum - sumB) / wF;
                // дисперсия
                currentMax = wB * wF * mD * mD;

                
                if (currentMax > totalMax)
                {
                    totalMax = currentMax;
                    rift = i; 
                }
            }
            //сумма высот всех столбцов
           /* for (int t = 0; t <= M; t++)
            {
                m += t * barchart[t];
                n += barchart[t];
            }

            float maxDisp = -1; // Максимальное значение межклассовой дисперсии
            

            int high1 = 0; //Сумма высот всех столбцов для класса 1
            int midhigh1 = 0; //Сумма высот всех столбцов для класса 1, домноженных на положение их середины

            for (int r = 0; r < M; r++)
            {
                high1 += r * barchart[r];
                midhigh1 += barchart[r];

                float p1 = (float)midhigh1 / n; //вероятность класса 1
                // a = a1 - a2, где a1, a2 - средние арифметические для классов 1 и 2
                float a = (float)high1 / midhigh1 - (float)(m - high1) / (n - midhigh1);
                float disp = p1 * (1 - p1) * a * a;

                if (disp > maxDisp)
                {
                    maxDisp = disp;
                    rift = r;
                }
            }*/

            // порог отсчитывался от min
            rift += min;

            for (int i = 0; i < bitmap.Width; i++)
                for (int j = 0; j < bitmap.Height; j++)
                {
                    int ncol = bitmap.GetPixel(i, j).R;
                    if (ncol < rift) ncol = 0;
                    if (ncol >= rift) ncol = 255;
                    
                    newBitmap.SetPixel(i, j, Color.FromArgb(ncol, ncol, ncol));
                }
            return newBitmap;
        }
    }

}
