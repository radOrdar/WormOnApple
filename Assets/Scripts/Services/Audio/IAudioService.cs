namespace Services.Audio
{
    public interface IAudioService : IService
    {
        void PlayFinish();
        void PlayLost();
        void PlayPickup();
        void PlayMusic();
    }
}