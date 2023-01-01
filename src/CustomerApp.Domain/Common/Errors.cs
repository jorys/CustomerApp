using ErrorOr;

namespace CustomerApp.Domain.Common;

public static class Errors
{
    internal static Error IsRequired(string fieldName) => 
        Error.Validation(
            $"{fieldName}.{nameof(IsRequired)}",
            $"{fieldName} is required.");

    internal static Error InvalidFormat(string fieldName) =>
        Error.Validation(
            $"{fieldName}.{nameof(InvalidFormat)}",
            $"{fieldName} has not a valid format.");

    internal static Error InvalidTemporaryEmail(string fieldName) =>
        Error.Validation(
            $"{fieldName}.{nameof(InvalidTemporaryEmail)}",
            $"{fieldName} must not be a temporary email.");

    internal static Error InvalidLength(string fieldName, int length) => 
        Error.Validation(
            $"{fieldName}.{nameof(InvalidLength)}",
            $"{fieldName} must have a length of {length}.");

    internal static Error MinLength(string fieldName, int length) =>
        Error.Validation(
            $"{fieldName}.{nameof(MinLength)}",
            $"{fieldName} must be at least {length} characters length.");

    internal static Error InvalidValue(string fieldName, string[]? validValues = null)
    {
        var description = $"{fieldName} has not a valid value";
        description += validValues is not null
            ? $" (accepted values: {string.Join(", ", validValues)})."
            : ".";

        return Error.Validation(
            $"{fieldName}.{nameof(InvalidValue)}",
            description);
    }

    internal static Error MustContainNumber(string fieldName) =>
        Error.Validation(
            $"{fieldName}.{nameof(MustContainNumber)}",
            $"{fieldName} must contain a number.");

    internal static Error MustContainSpecialCharacter(string fieldName) =>
        Error.Validation(
            $"{fieldName}.{nameof(MustContainSpecialCharacter)}",
            $"{fieldName} must contain a special character.");

    public const string MaximumLoginAttemptErrorCode = "Login.MaximumAttempt";
    internal static Error MaximumLoginAttempt() =>
        Error.Failure(
            MaximumLoginAttemptErrorCode,
            "Maximum attempt of login reached.");

    internal static Error DateShouldBeInFuture(string fieldName) =>
        Error.Validation(
            $"{fieldName}.{nameof(DateShouldBeInFuture)}",
            $"{fieldName} should be in future.");

    public static Error CannotBeNegative(string fieldName) =>
        Error.Validation(
            $"{fieldName}.{nameof(CannotBeNegative)}",
            $"{fieldName} cannot be negative.");

    public static Error NotFound(string fieldName) =>
        Error.Validation(
            $"{fieldName}.{nameof(NotFound)}",
            $"{fieldName} was not found.");

    public static Error MaxConcurrencyRetries(string fieldName) =>
        Error.Failure(
            $"{fieldName}.{nameof(MaxConcurrencyRetries)}",
            $"Unable to update {fieldName}, even with retries policy, due to concurrent access.");

    internal static Error MustBePositive(string fieldName) =>
        Error.Failure(
            $"{fieldName}.{nameof(MustBePositive)}",
            $"{fieldName} must be positive value.");
}
