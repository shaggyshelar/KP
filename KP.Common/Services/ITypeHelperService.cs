namespace KP.Common.Services
{
    public interface ITypeHelperService
    {
          bool TypeHasProperties<T>(string fields);
    }
}