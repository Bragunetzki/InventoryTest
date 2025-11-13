namespace Core.Utils
{
    public struct Result<T>
    {
        public T Object { get; private set; }
        public bool Exists { get; private set; }
        
        public Result(T result, bool exists)
        {
            Object = result;
            Exists = exists;
        }
    }
}