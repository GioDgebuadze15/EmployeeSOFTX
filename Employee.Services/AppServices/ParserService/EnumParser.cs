namespace Employee.Services.AppServices.ParserService;

public class EnumParser<T> : CustomParser<T?> where T : struct, Enum
{
    public override T? Parse(string toParse)
    {
        if (Enum.TryParse(toParse, out T parsedEnum))
        {
            return parsedEnum;
        }

        return null;
    }
}