namespace XEmuera.Forms
{
    public interface IPlayer
    {
        bool Looping { get; set; }

        bool IsPlaying { get; }

        float Volume { get; set; }


        void Load(string filepath);

        void Play();
        void Stop();
    }
}