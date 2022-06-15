using NAudio.Dsp;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace CandySugar.Library.AudioTemplate
{
    public class AudioFactory : IDisposable
    {
        /// <summary>
        /// 输出设备
        /// </summary>
        private WaveOutEvent _WaveOut;
        /// <summary>
        /// 音频读取
        /// </summary>
        //private MediaFoundationReader _MediaReader;
        /// <summary>
        /// 音频读取
        /// </summary>
        private AudioFileReader _AudioReader;
        /// <summary>
        ///  每个单声道数据样本的位数，例如 16位，24位，32位
        /// </summary>
        private int _BitsPerSample;
        /// <summary>
        /// 采样率，例如 44.1Khz ，就是 44100
        /// </summary>
        private int _SampleRate;
        /// <summary>
        /// 通道数，例如 2
        /// </summary>
        private int _ChannelCount;
        /// <summary>
        /// 定时器
        /// </summary>
        private static Timer _Timer = new() { Interval = 100 };
        /// <summary>
        /// 实体类数据
        /// </summary>
        private AudioModel _Audio;
        private static AudioFactory _Instance;
        public static AudioFactory Instance 
        {
            get
            {
                if (_Instance == null)
                    _Instance = new AudioFactory();

                return _Instance;
            }
        }
        private Action<Tuple<List<double>, string, double>> _Action;
        /// <summary>
        /// 空检查
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        private void NullCheck()
        {
            if (_WaveOut == null && _AudioReader == null)
                throw new NullReferenceException("请先调用【InitConfig】方法初始化!");
        }
        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="action"></param>
        private void RegistEvent()
        {
            _WaveOut.PlaybackStopped += (sender, args)=> { };
            _Timer.Elapsed -= PlayEvent;
            _Timer.Elapsed += PlayEvent;
        }
        private async void PlayEvent(object sender, ElapsedEventArgs e)
        {
            if (_WaveOut != null)
            {
                if (_WaveOut.PlaybackState == PlaybackState.Playing)
                {
                    GetSampleArray();
                    _Action.Invoke(await FourierTransform());
                }
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="route"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public AudioFactory InitConfig(string route, Action<Tuple<List<double>, string, double>> action)
        {
            Dispose();

            _Action = action;
            SampleArray = new float[1024];
            _WaveOut = new WaveOutEvent();
            _AudioReader = new AudioFileReader(route);

            _BitsPerSample = _AudioReader.WaveFormat.BitsPerSample;
            _SampleRate = _AudioReader.WaveFormat.SampleRate;
            _ChannelCount = _AudioReader.WaveFormat.Channels;

            _Audio = new AudioModel();

            RegistEvent();
            return this;
        }
        /// <summary>
        /// 改变音量
        /// </summary>
        /// <param name="vol"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public AudioFactory Change(float vol)
        {
            NullCheck();
            _WaveOut.Volume = vol;
            return this;
        }
        /// <summary>
        /// 播放器
        /// </summary>
        /// <returns></returns>
        public WaveOutEvent PlayOut()
        {
            return _WaveOut;
        }
        /// <summary>
        /// 执行（1：播放 2：暂停 3：停止）
        /// </summary>
        /// <param name="action"></param>
        /// <param name="Module">1：播放 2：暂停 3：停止</param>
        /// <returns></returns>
        public AudioFactory Run(Action<AudioModel> action = null, int Module = 1)
        {
            NullCheck();

            if (Module < 3 && Module > 0)
            {
                _WaveOut.Init(_AudioReader);
                _Audio.Seconds = _AudioReader.TotalTime.TotalSeconds;
                _Audio.TimeSpan = _AudioReader.TotalTime.ToString().Split(".").FirstOrDefault();
                if (Module == 1)
                {
                    _WaveOut.Play();
                    _Timer.Start();
                }
                if (Module == 2) _WaveOut.Pause();


            }
            else  //Stop()函数会触发PlaybackStopped事件
                _WaveOut.Stop();

            action?.Invoke(_Audio);

            return this;
        }
        public void Dispose()
        {
            _Audio = null;
            if (_Timer != null)
            {
                _Timer.Elapsed -= PlayEvent;
                _Timer.Enabled = false;
                _Timer.Stop();
            }

            if (_WaveOut != null)
            {
                _WaveOut.Dispose();
                _WaveOut = null;
            }
            if (_AudioReader != null)
            {
                _AudioReader.Dispose();
                _AudioReader.Close();
                _AudioReader = null;
            }
        }
        #region 获取音频的其他信息
        /// <summary>
        /// 采样数据的对象锁，防止未分离左右通道就进入下一次采样
        /// </summary>
        private object _sampleLock = new object();
        /// <summary>
        /// 音频采样数据
        /// </summary>
        private float[] SampleArray { get; set; }
        private async void GetSampleArray()
        {
            await Task.Run(() => { _AudioReader.Read(SampleArray, 0, 1024); });
        }
        /// <summary>
        /// 傅里叶变换
        /// </summary>
        private async Task<Tuple<List<double>,string,double>> FourierTransform()
        {
           return await Task.Run(() =>
             {
                #region 分离左右通道

                //假设 SampleArray 中已经有数据
                 float[][] chanelSampleArray;
                 lock (this._sampleLock)//防止未分离完左右通道就进入下一次调用 SampleArray
                 {
                     chanelSampleArray = Enumerable
                         .Range(0, _ChannelCount)//分离通道
                         .Select(chanel => Enumerable//对每个通过的数据进行处理
                             .Range(0, this.SampleArray.Length / this._ChannelCount)//每个通道的数组长度
                             .Select(i => this.SampleArray[chanel + i * this._ChannelCount])//左右左右，这样读取
                             .ToArray())
                         .ToArray();
                 }

                #endregion

                #region 合并左右通道并取平均值

                 float[] chanelAverageSample = Enumerable
                     .Range(0, chanelSampleArray[0].Length)
                     .Select(index => Enumerable//每次读取一个左右数据合并、取平均值
                         .Range(0, this._ChannelCount)
                         .Select(chanel => chanelSampleArray[chanel][index])
                         .Average())
                     .ToArray();

                #endregion

                #region 傅里叶变换
                //NAudio 提供了快速傅里叶变换的方法, 通过傅里叶变换, 可以将时域数据转换为频域数据
                // 取对数并向上取整
                 int log = (int)Math.Ceiling(Math.Log(chanelAverageSample.Length, 2));
                //对于快速傅里叶变换算法, 需要数据长度为 2 的 n 次方
                 int length = (int)Math.Pow(2, log);
                 float[] filledSample = new float[length];
                //拷贝到新数组
                 Array.Copy(chanelAverageSample, filledSample, chanelAverageSample.Length);
                //将采样转化为复数
                 Complex[] complexArray = filledSample
                     .Select((value, index) => new Complex() { X = value })
                     .ToArray();
                //进行傅里叶变换
                 FastFourierTransform.FFT(false, log, complexArray);

                #endregion

                #region 提取需要的频域信息

                 Complex[] halfComeplexArray = complexArray
                     .Take(complexArray.Length / 2)//数据是左右对称的，所以只取一半
                     .ToArray();

                //这个已经是频域数据了
                 double[] resultArray = complexArray
                     .Select(value => Math.Sqrt(value.X * value.X + value.Y * value.Y))//复数取模
                     .ToArray();

                //我们取 最小频率 ~ 20000Hz
                //对于变换结果, 每两个数据之间所差的频率计算公式为 采样率/采样数, 那么我们要取的个数也可以由 20000 / (采样率 / 采样数) 来得出
                //当然，因为我这里并没有指定频率与幅值，所以顺便取几个数就行，若有需要可以再去细分各个频率的幅值
                 int count = 20000 / (this._SampleRate / length);
                 double[] finalData = resultArray.Take(count).ToArray();

                 #endregion

                 #region 获取实时播放时间和长度
                 var CurrentSpan = _AudioReader.CurrentTime.ToString().Split(".").FirstOrDefault();
                 var CurrentSeconds = _AudioReader.CurrentTime.TotalSeconds;
                 #endregion

                 #region 设置绑定数据
                 var LineData = finalData.Take(16).ToList();
                 #endregion

                 return new Tuple<List<double>, string, double>(LineData, CurrentSpan, CurrentSeconds);
             });
        }
        #endregion
    }
}
