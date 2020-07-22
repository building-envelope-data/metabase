using Infrastructure.GraphQl;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using Exception = System.Exception;

namespace Metabase.GraphQl
{
    public abstract class NodeBase
      : Infrastructure.GraphQl.NodeBase
    {
        public static Node FromModel(
            IModel model,
            Timestamp requestTimestamp
            )
        {
            return model switch
            {
                Models.Component component =>
                  Component.FromModel(component, requestTimestamp),
                Models.ComponentConcretization componentConcretization =>
                  ComponentConcretization.FromModel(componentConcretization, requestTimestamp),
                Models.ComponentManufacturer componentManufacturer =>
                  ComponentManufacturer.FromModel(componentManufacturer, requestTimestamp),
                Models.ComponentPart componentPart =>
                  ComponentPart.FromModel(componentPart, requestTimestamp),
                Models.ComponentVariant componentVariant =>
                  ComponentVariant.FromModel(componentVariant, requestTimestamp),
                Models.ComponentVersion componentVersion =>
                  ComponentVersion.FromModel(componentVersion, requestTimestamp),
                Models.Database database =>
                  Database.FromModel(database, requestTimestamp),
                Models.Institution institution =>
                  Institution.FromModel(institution, requestTimestamp),
                Models.InstitutionRepresentative institutionRepresentative =>
                  InstitutionRepresentative.FromModel(institutionRepresentative, requestTimestamp),
                Models.Method method =>
                  Method.FromModel(method, requestTimestamp),
                Models.MethodDeveloper methodDeveloper =>
                  MethodDeveloper.FromModel(methodDeveloper, requestTimestamp),
                Models.PersonAffiliation personAffiliation =>
                  PersonAffiliation.FromModel(personAffiliation, requestTimestamp),
                Models.Person person =>
                  Person.FromModel(person, requestTimestamp),
                Models.Standard standard =>
                  Standard.FromModel(standard, requestTimestamp),
                Models.User user =>
                  User.FromModel(user, requestTimestamp),
                _ =>
                  throw new Exception($"The model {model} fell through")
            };
        }

        protected NodeBase(
            Id id,
            Timestamp timestamp,
            Timestamp requestTimestamp
            )
          : base(
              id: id,
              timestamp: timestamp,
              requestTimestamp: requestTimestamp
              )
        {
        }
    }
}