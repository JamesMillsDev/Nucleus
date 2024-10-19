namespace Nucleus.Structures
{
    public class Either<T>
    {
        private readonly T? failure;
        private readonly T? success;
        
        public Either(T failure, T success)
        {
            this.failure = failure;
            this.success = success;
        }

        public T? Evaluate(Func<bool> predicate) => predicate() ? success : failure;
    }
}