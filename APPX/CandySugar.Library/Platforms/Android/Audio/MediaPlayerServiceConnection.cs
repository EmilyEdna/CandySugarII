using Android.Content;
using Android.OS;

namespace CandySugar.Library.Platforms.Android.Audio
{
    public class MediaPlayerServiceConnection : Java.Lang.Object, IServiceConnection
    {
        readonly IAudioActivity instance;

        public MediaPlayerServiceConnection(IAudioActivity mediaPlayer)
        {
            this.instance = mediaPlayer;
        }

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            if (service is MediaPlayerServiceBinder binder)
            {
                instance.Binder = binder;
            }
        }

        public void OnServiceDisconnected(ComponentName name)
        {
        }
    }
}
