// HotChocolate's error codes can be found at
// https://github.com/ChilliCream/hotchocolate/blob/master/src/Core/Abstractions/ErrorCodes.cs

namespace Icon
{
    public static class ErrorCodes
    {
        public const string InvalidState = "INVALID_STATE";
        public const string InvalidValue = "INVALID_VALUE";
        public const string InvalidType = "INVALID_TYPE";
        public const string AlreadyExistingAssociation = "ALREADY_EXISTING_ASSOCIATION";
        public const string NonExistentModel = "NON_EXISTENT_MODEL";
        public const string OutOfDate = "OUT_OF_DATE";
        public const string IdCollision = "ID_COLLISION";
        public const string FileNotFound = "FILE_NOT_FOUND";
        public const string InvalidSyntax = "INVALID_SYNTAX";
        public const string GraphQlRequestFailed = "GRAPH_QL_REQUEST_FAILED";
    }
}