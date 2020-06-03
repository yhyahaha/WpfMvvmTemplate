namespace InterFaces
{
    public interface IObjectMapping
    {
        TDestination Map<TSource,TDestination>(TSource source);
    }
}
