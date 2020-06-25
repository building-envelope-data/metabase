namespace Icon.ValueObjects
{
    public abstract class AddOneToManyAssociationInput
      : AddAssociationInput
    {
        protected AddOneToManyAssociationInput(
            Id parentId,
            Id associateId
            )
          : base(
              parentId: parentId,
              associateId: associateId
              )
        {
        }
    }
}