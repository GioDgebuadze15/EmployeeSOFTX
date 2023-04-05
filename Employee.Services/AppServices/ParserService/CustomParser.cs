namespace Employee.Services.AppServices.ParserService;

public abstract class CustomParser<T>
{
    public abstract T? Parse(string toParse);
}