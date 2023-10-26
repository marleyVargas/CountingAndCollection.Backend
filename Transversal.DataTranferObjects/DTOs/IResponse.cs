
namespace Transversal.DTOs
{
    public interface IResponse<out T>
    {
        T Result { get; }
    }
}