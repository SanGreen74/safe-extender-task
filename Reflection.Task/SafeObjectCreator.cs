using System;

namespace Reflection.Task
{
    public class SafeObjectCreator : ISafeObjectCreator
    {
        public T CreateSafe<T>()
            where T : class
        {
            throw new NotImplementedException();
        }
    }
}