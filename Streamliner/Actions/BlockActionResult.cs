namespace Streamliner.Actions
{
    public class BlockActionResult
    {
        public static BlockActionResult<T> Success<T>(T model) => new BlockActionResult<T> { Continue = true, Model = model };
        public static BlockActionResult<T> Fail<T>(T model) => new BlockActionResult<T> { Continue = false, Model = model };
    }
    
    public class BlockActionResult<T>
    {
        public bool Continue { get; set; }
        public T Model { get; set; }
    }
}