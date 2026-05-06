import BaseFilterSubform from "./BaseFilterSubform";
import { ListFilterDefinition, ListFilterInput } from "../../lib/filter";
import FilterSubform from "./FilterSubform";
import { FilterTypeMap } from "../../lib/filter";

export default function ListFilterSubform<
  TItemFilterInput,
  TListFilterInput extends ListFilterInput<TItemFilterInput>,
>({
  name,
  ancestors,
  definition,
}: {
  name: readonly (string | number)[];
  ancestors: readonly (string | number)[];
  definition: ListFilterDefinition<TListFilterInput, keyof TListFilterInput>;
}) {
  const renderItem = (props: { name: (string | number)[] }) => (
    <FilterSubform
      name={props.name}
      ancestors={ancestors}
      definition={definition.item}
    />
  );
  return (
    <BaseFilterSubform<TListFilterInput>
      name={name}
      ancestors={ancestors}
      operators={FilterTypeMap["list"].operators}
      initialOperator={FilterTypeMap["list"].initialOperator}
      renderSingleFormItem={renderItem}
      renderMultipleFormItem={renderItem}
    />
  );
}
