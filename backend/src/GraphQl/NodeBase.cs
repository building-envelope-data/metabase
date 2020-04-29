using Models = Icon.Models;
using DateTime = System.DateTime;
using Exception = System.Exception;

namespace Icon.GraphQl
{
    public abstract class NodeBase
      : Node
    {
        protected static ValueObjects.TimestampedId TimestampId(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp
            )
        {
            return ResultHelpers.HandleFailure(
                ValueObjects.TimestampedId.From(
                  id, timestamp
                  )
                );
        }

        public static Node FromModel(
            Models.IModel model,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            /* if (model is ComponentAssembly componentAssembly) */
            /*     return ComponentAssembly.FromModel(componentAssembly, requestTimestamp); */
            if (model is Models.Component component)
                return Component.FromModel(component, requestTimestamp);
            if (model is Models.ComponentManufacturer componentManufacturer)
                return ComponentManufacturer.FromModel(componentManufacturer, requestTimestamp);
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

        public ValueObjects.Id Id { get; }
        public ValueObjects.Timestamp Timestamp { get; }
        public ValueObjects.Timestamp RequestTimestamp { get; } // TODO? Better name it `requestTimestamp` or `fetchTimestamp` or `loadTimestamp` or `queryTimestamp` or ...

        public NodeBase(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            Id = id;
            Timestamp = timestamp;
            RequestTimestamp = requestTimestamp;
        }
    }
}