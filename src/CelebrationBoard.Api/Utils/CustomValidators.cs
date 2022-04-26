using CelebrationBoard.Domain.Common;
using CSharpFunctionalExtensions;
using FluentValidation;

namespace CelebrationBoard.Api.Celebrations;

public static class CustomValidators
{
  public static IRuleBuilderOptions<T, TProperty> NotEmpty<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
  {
    return DefaultValidatorExtensions.NotEmpty(ruleBuilder)
        .WithMessage(Errors.General.ValueIsRequired().Serialise());
  }

  public static IRuleBuilderOptions<T, int> GreaterThan<T>(this IRuleBuilder<T, int> ruleBuilder, int greaterThanValue)
  {
    return DefaultValidatorExtensions.GreaterThan(ruleBuilder, greaterThanValue)
        .WithMessage(Errors.General.GreaterThan(greaterThanValue).Serialise());
  }

  public static IRuleBuilderOptions<T, string> MustBeValueObject<T, TValueObject>(
            this IRuleBuilder<T, string> ruleBuilder,
            Func<string, Result<TValueObject, Error>> factoryMethod)
            where TValueObject : ValueObject
  {
    return (IRuleBuilderOptions<T, string>)ruleBuilder.Custom((value, context) =>
    {
      Result<TValueObject, Error> result = factoryMethod(value);

      if (result.IsFailure)
      {
        context.AddFailure(result.Error.Serialise());
      }
    });
  }
}