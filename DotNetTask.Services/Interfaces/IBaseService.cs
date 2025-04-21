namespace DotNetTask.Services.Interfaces;
public interface IBaseService<T>
{
    public void Validate(T model);
}