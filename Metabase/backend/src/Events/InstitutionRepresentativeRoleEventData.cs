using Exception = System.Exception;

namespace Metabase.Events
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
            return role switch
            {
                ValueObjects.InstitutionRepresentativeRole.OWNER
                    => InstitutionRepresentativeRoleEventData.OWNER,
                ValueObjects.InstitutionRepresentativeRole.MAINTAINER
                    => InstitutionRepresentativeRoleEventData.MAINTAINER,
                ValueObjects.InstitutionRepresentativeRole.ASSISTANT
                    => InstitutionRepresentativeRoleEventData.ASSISTANT,
                // God-damned C# does not have switch expression exhaustiveness for
                // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
                _ => throw new Exception($"The role {role} fell through")
            };
        }

        public static ValueObjects.InstitutionRepresentativeRole ToModel(this InstitutionRepresentativeRoleEventData role)
        {
            return role switch
            {
                InstitutionRepresentativeRoleEventData.OWNER
                    => ValueObjects.InstitutionRepresentativeRole.OWNER,
                InstitutionRepresentativeRoleEventData.MAINTAINER
                    => ValueObjects.InstitutionRepresentativeRole.MAINTAINER,
                InstitutionRepresentativeRoleEventData.ASSISTANT
                    => ValueObjects.InstitutionRepresentativeRole.ASSISTANT,
                // God-damned C# does not have switch expression exhaustiveness for
                // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
                _ => throw new Exception($"The role {role} fell through")
            };
        }
    }
}