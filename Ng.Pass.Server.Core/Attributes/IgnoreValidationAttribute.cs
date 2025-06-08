using System.ComponentModel.DataAnnotations;

namespace Ng.Pass.Server.Core.Attributes;

/// <summary>
/// When applied, this attribute will ignore API binding validation.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class IgnoreValidationAttribute : ValidationAttribute { }
