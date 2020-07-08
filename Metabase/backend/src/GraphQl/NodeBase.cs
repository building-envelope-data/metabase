using Infrastructure.Models;
using Infrastructure.ValueObjects;
using Exception = System.Exception;

namespace Metabase.GraphQl
{
    public abstract class NodeBase
      : Node
    {
        protected static TimestampedId TimestampId(
            Id id,
            Timestamp timestamp
            )
        {
            return ResultHelpers.HandleFailure(
                TimestampedId.From(
                  id, timestamp
                  )
                );
        }

        public static Node FromModel(
            IModel model,
            Timestamp requestTimestamp
            )
        {
            if (model is Models.Component component)
                return Component.FromModel(component, requestTimestamp);
            if (model is Models.ComponentConcretization componentConcretization)
                return ComponentConcretization.FromModel(componentConcretization, requestTimestamp);
            if (model is Models.ComponentManufacturer componentManufacturer)
                return ComponentManufacturer.FromModel(componentManufacturer, requestTimestamp);
            if (model is Models.ComponentPart componentPart)
                return ComponentPart.FromModel(componentPart, requestTimestamp);
            if (model is Models.ComponentVariant componentVariant)
                return ComponentVariant.FromModel(componentVariant, requestTimestamp);
            if (model is Models.ComponentVersion componentVersion)
                return ComponentVersion.FromModel(componentVersion, requestTimestamp);
            if (model is Models.Database database)
                return Database.FromModel(database, requestTimestamp);
            if (model is Models.Institution institution)
                return Institution.FromModel(institution, requestTimestamp);
            if (model is Models.InstitutionRepresentative institutionRepresentative)
                return InstitutionRepresentative.FromModel(institutionRepresentative, requestTimestamp);
            if (model is Models.Method method)
                return Method.FromModel(method, requestTimestamp);
            if (model is Models.MethodDeveloper methodDeveloper)
                return MethodDeveloper.FromModel(methodDeveloper, requestTimestamp);
            if (model is Models.PersonAffiliation personAffiliation)
                return PersonAffiliation.FromModel(personAffiliation, requestTimestamp);
            if (model is Models.Person person)
                return Person.FromModel(person, requestTimestamp);
            if (model is Models.Standard standard)
                return Standard.FromModel(standard, requestTimestamp);
            if (model is Models.User user)
                return User.FromModel(user, requestTimestamp);
            throw new Exception($"The model {model} fell through");
        }

        public Id Id { get; }
        public Timestamp Timestamp { get; }
        public Timestamp RequestTimestamp { get; }

        protected NodeBase(
            Id id,
            Timestamp timestamp,
            Timestamp requestTimestamp
            )
        {
            Id = id;
            Timestamp = timestamp;
            RequestTimestamp = requestTimestamp;
        }
    }
}