using Microsoft.EntityFrameworkCore;

namespace Apex.Infrastructure.DAL;

internal static class ModelBuilderExtensions
{
    public static ModelBuilder ConvertObjectNamesToSnakeCase(this ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetTableName(ToSnakeCase(entity.GetTableName()!));
            foreach (var property in entity.GetProperties())
                property.SetColumnName(ToSnakeCase(property.GetColumnName()));
            foreach (var key in entity.GetKeys())
                key.SetName(ToSnakeCase(key.GetName()!));
            foreach (var fk in entity.GetForeignKeys())
                fk.SetConstraintName(ToSnakeCase(fk.GetConstraintName()!));
            foreach (var index in entity.GetIndexes())
                index.SetDatabaseName(ToSnakeCase(index.GetDatabaseName()!));
        }
        return modelBuilder;
    }
    
    private static string ToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;
        
        var result = new System.Text.StringBuilder();
        result.Append(char.ToLowerInvariant(input[0]));
        
        for (int i = 1; i < input.Length; i++)
        {
            char c = input[i];
            if (char.IsUpper(c))
            {
                if (char.IsLower(input[i - 1]) || (i < input.Length - 1 && char.IsLower(input[i + 1])))
                    result.Append('_');
                result.Append(char.ToLowerInvariant(c));
            }
            else
            {
                result.Append(c);
            }
        }            
        return result.ToString();
    }
}