namespace Services.Persistent
{
    public interface IPersistentDataService : IService
    {
        void ResetProgress();

        int GetLevel();

        void SaveLevel(int level);
    }
}