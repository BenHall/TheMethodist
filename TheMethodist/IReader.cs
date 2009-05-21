namespace TheMethodist
{
    public class Event
    {
        public delegate void ReadLineEventHandler(string line);
    }
    public interface IReader
    {
        event TheMethodist.Event.ReadLineEventHandler LineRead;
    }
}
