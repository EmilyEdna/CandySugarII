﻿using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CandySugar.Library.TemplateEffects
{
    public unsafe class WaterEffect : ShaderEffect
    {
        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(void* d, void* s, int len);
        static void MemorySet(void* src, byte value, int size)
        {
            byte* p = (byte*)src;
            for (int i = 0; i < size; i++, p++)
                *p = value;
        }
        static readonly DependencyProperty DxProperty = DependencyProperty.Register("Dx", typeof(float), typeof(WaterEffect), new UIPropertyMetadata(0f, PixelShaderConstantCallback(0)));
        static readonly DependencyProperty DyProperty = DependencyProperty.Register("Dy", typeof(float), typeof(WaterEffect), new UIPropertyMetadata(0f, PixelShaderConstantCallback(1)));
        static readonly DependencyProperty InputProperty = RegisterPixelShaderSamplerProperty("Input", typeof(WaterEffect), 0);
        static readonly DependencyProperty HeightProperty = RegisterPixelShaderSamplerProperty("Height", typeof(WaterEffect), 1);

        DispatcherTimer Timer;
        DispatcherTimer TimerEach;
        Random random = new Random();//给出一个随机数
        readonly int size;
        public WaterEffect(int w, int h)
        {
            this.Width = w;
            this.Height = h;
            size = w * h * 4;
            this.data = (float*)Marshal.AllocHGlobal(size);
            this.buf1 = (float*)Marshal.AllocHGlobal(size);
            this.buf2 = (float*)Marshal.AllocHGlobal(size);
            MemorySet(buf1, 0, size);
            MemorySet(buf2, 0, size);

            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri("/CandySugar.Resource;component/Assets/water.ps", UriKind.Relative);
            this.PixelShader = pixelShader;

            this.SetValue(DxProperty, 1f / (Width - 1f));
            this.SetValue(DyProperty, 1f / (Height - 1f));

            Updata();

            this.UpdateShaderValue(InputProperty);
            this.UpdateShaderValue(DxProperty);
            this.UpdateShaderValue(DyProperty);
            this.UpdateShaderValue(HeightProperty);

            CompositionTarget.Rendering += delegate { Apply(); };

            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(1);
            Timer.Tick += delegate { Updata(); };
            Timer.Start();

            TimerEach = new DispatcherTimer();
            TimerEach.Interval = TimeSpan.FromSeconds(20);
            TimerEach.Tick += delegate
            {
                float x = random.Next(0, 10) / 10f;
                float y = random.Next(0, 10) / 10f;
                Drop(x, y);
            };

        }

        public readonly int Width;

        public readonly int Height;

        float* data, buf1, buf2;
        private void Updata()
        {
            Action<int> act = y =>
            {
                int n = y * Width;
                for (int x = 0; x < Width; x++, n++)
                {
                    float s = GetValue(buf1, x, y - 1) + GetValue(buf1, x, y + 1) + GetValue(buf1, x - 1, y) + GetValue(buf1, x + 1, y);
                    s = (s / 2 - buf2[n]);
                    s -= s / 128;
                    if (s > 2) s = 2;
                    if (s < -2) s = -2;
                    buf2[n] = s;
                }
            };
            Parallel.For(0, Height, act);
            var temp = buf1;
            buf1 = buf2;
            buf2 = temp;
        }

        private void Apply()
        {
            Action<int> act = y =>
            {
                int n = y * Width;
                for (int x = 0; x < Width; x++, n++)
                {
                    data[n] = (buf2[n] + 2) / 4;
                }
            };
            Parallel.For(0, Height, act);

            var map = new ImageBrush(BitmapImage.Create(Width, Height, 96, 96, PixelFormats.Gray32Float, null, new IntPtr(data), size, Width * 4));
            this.SetValue(HeightProperty, map);
        }

        private float GetValue(float* buf, int x, int y)
        {
            if (x < 0)
                x = 0;
            else if (x > Width - 1)
                x = Width - 1;

            if (y < 0)
                y = 0;
            else if (y > Height - 1)
                y = Height - 1;

            return buf[y * Width + x];
        }

        private void SetValue(float* buf, int x, int y, float value)
        {
            if (x < 0)
                x = 0;
            else if (x > Width - 1)
                x = Width - 1;

            if (y < 0)
                y = 0;
            else if (y > Height - 1)
                y = Height - 1;

            buf[y * Width + x] = value;
        }

        int r = 20;//貌似是幅度
        float h = -1f;
        public void Drop(float xi, float yi)
        {
            int px = (int)(xi * (Width - 1));
            int py = (int)(yi * (Height - 1));
            for (int j = py - r; j <= py + r; j++)
            {
                for (int i = px - r; i <= px + r; i++)
                {
                    float dx = i - px;
                    float dy = j - py;
                    float a = (float)(1 - (dx * dx + dy * dy) / (r * r));
                    if (a > 0 && a <= 1)
                    {
                        SetValue(buf1, i, j, a * h);
                    }
                }
            }
        }

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                DisposeCore();
                GC.SuppressFinalize(this);
            }
        }

        protected void DisposeCore()
        {
            IsDisposed = true;
            if ((IntPtr)buf1 != IntPtr.Zero) Marshal.FreeHGlobal((IntPtr)buf1);
            if ((IntPtr)buf2 != IntPtr.Zero) Marshal.FreeHGlobal((IntPtr)buf2);
        }

        public void StartTime()
        {
            TimerEach.Start();
        }

        public void StopTime()
        {
            TimerEach.Stop();
        }

        ~WaterEffect()
        {
            Dispose();
        }
    }
}
