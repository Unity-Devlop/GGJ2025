namespace Game
{
    public interface IGameEntry
    {
        public bool initialized { get; }
        public void OnInit();
    }
    
    public interface IIndexable
    {
        public int index { get; }
    }

    public interface IJsonData
    {
        
    }
}