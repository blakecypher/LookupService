using System.Text;
using System.Text.Json;

namespace backend.Helpers;

public class CamelCaseToSnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return name;
        }

        // Convert camelCase to snake_case
        var snakeCaseName = "";
        for (int i = 0; i < name.Length; i++)
        {
            if (char.IsUpper(name[i]))
            {
                snakeCaseName += (i > 0 ? "_" : "") + char.ToLower(name[i]);
            }
            else
            {
                snakeCaseName += name[i];
            }
        }

        return snakeCaseName;
    }
}