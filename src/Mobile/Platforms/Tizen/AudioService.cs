

namespace Microsoft.NetConf2021.Maui.Platforms.Tizen;

    public class AudioService : IAudioService
    {
        //MainActivity instance;

        //private MediaPlayer mediaPlayer => null;

        public bool IsPlaying => false;

        public double CurrentPosition => 0;

        public Task InitializeAsync(string audioURI)
        {
            //if (this.instance == null)
            //{
            //    this.instance = MainActivity.instance;
            //}
            //else
            //{
            //    await this.instance.binder.GetMediaPlayerService().Stop();
            //}

            //this.instance.binder.GetMediaPlayerService().AudioUrl = audioURI;
            return Task.CompletedTask;
        }

        public Task PauseAsync()
        {
            //if (IsPlaying)
            //{
            //    return this.instance.binder.GetMediaPlayerService().Pause();
            //}

            return Task.CompletedTask;
        }

        public Task PlayAsync(double position = 0)
        {
            //await this.instance.binder.GetMediaPlayerService().Play();
            //await this.instance.binder.GetMediaPlayerService().Seek((int)position * 1000);
            return Task.CompletedTask;
        }
    }
