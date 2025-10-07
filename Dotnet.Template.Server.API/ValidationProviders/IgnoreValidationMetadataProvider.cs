using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Dotnet.Template.Server.Core.Attributes;

public class IgnoreValidationMetadataProvider : IValidationMetadataProvider
{
    public void CreateValidationMetadata(ValidationMetadataProviderContext context)
    {
        var attributes = context.Attributes.OfType<IgnoreValidationAttribute>();
        if (attributes.Any())
        {
            context.ValidationMetadata.IsRequired = false;
            context.ValidationMetadata.ValidatorMetadata.Clear();
        }
    }
}
