using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Query;
using IError = HotChocolate.IError;
using Models = Icon.Models;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Queries
{
    public abstract class GetManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>
      : GetAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>
    {
        protected GetManyToManyAssociatesOfModels(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
          : base(timestampedIds)
        {
        }
    }
}