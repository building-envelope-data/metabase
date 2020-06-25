using Exception = System.Exception;

namespace Icon.Events
{
    public enum InstitutionRepresentativeRoleEventData
    {
        OWNER,
        MAINTAINER,
        ASSISTANT
    }

    public static class InstitutionRepresentativeRoleEventDataExtensions
    {
        public static InstitutionRepresentativeRoleEventData FromModel(this ValueObjects.InstitutionRepresentativeRole role)
        {
            switch (role)
            {
                case ValueObjects.InstitutionRepresentativeRole.OWNER:
                    return InstitutionRepresentativeRoleEventData.OWNER;
                case ValueObjects.InstitutionRepresentativeRole.MAINTAINER:
                    return InstitutionRepresentativeRoleEventData.MAINTAINER;
                case ValueObjects.InstitutionRepresentativeRole.ASSISTANT:
                    return InstitutionRepresentativeRoleEventData.ASSISTANT;
            }
            // God-damned C# does not have switch expression exhaustiveness for
            // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
            throw new Exception($"The role {role} fell through");
        }

        public static ValueObjects.InstitutionRepresentativeRole ToModel(this InstitutionRepresentativeRoleEventData role)
        {
            switch (role)
            {
                case InstitutionRepresentativeRoleEventData.OWNER:
                    return ValueObjects.InstitutionRepresentativeRole.OWNER;
                case InstitutionRepresentativeRoleEventData.MAINTAINER:
                    return ValueObjects.InstitutionRepresentativeRole.MAINTAINER;
                case InstitutionRepresentativeRoleEventData.ASSISTANT:
                    return ValueObjects.InstitutionRepresentativeRole.ASSISTANT;
            }
            // God-damned C# does not have switch expression exhaustiveness for
            // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
            throw new Exception($"The role {role} fell through");
        }
    }
}