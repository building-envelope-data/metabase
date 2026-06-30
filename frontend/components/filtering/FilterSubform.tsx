import {
  FilterDefinition,
  ListFilterDefinition,
  ListFilterInput,
} from "../../lib/filter";
import EnumFilterSubform from "./EnumFilterSubform";
import FloatFilterSubform from "./FloatFilterSubform";
import IntFilterSubform from "./IntFilterSubform";
import StringFilterSubform from "./StringFilterSubform";
import UrlFilterSubform from "./UrlFilterSubform";
import UuidFilterSubform from "./UuidFilterSubform";
import ListFilterSubform from "./ListFilterSubform";
import ObjectFilterSubform from "./ObjectFilterSubform";

export default function FilterSubform<TFilterInput>({
  name,
  ancestors,
  definition: definition,
}: {
  name: readonly (string | number)[];
  ancestors: readonly (string | number)[];
  definition: FilterDefinition<TFilterInput>;
}) {
  switch (definition.type) {
    case "enum":
      return (
        <EnumFilterSubform
          name={name}
          ancestors={ancestors}
          enumObject={definition.enumObject as any}
        />
      );
    case "float":
      return <FloatFilterSubform name={name} ancestors={ancestors} />;
    case "int":
      return <IntFilterSubform name={name} ancestors={ancestors} />;
    case "string":
      return <StringFilterSubform name={name} ancestors={ancestors} />;
    case "url":
      return <UrlFilterSubform name={name} ancestors={ancestors} />;
    case "uuid":
      return <UuidFilterSubform name={name} ancestors={ancestors} />;
    case "list":
      return (
        <ListFilterSubform
          name={name}
          ancestors={ancestors}
          definition={
            definition as ListFilterDefinition<
              ListFilterInput<unknown>,
              keyof ListFilterInput<unknown>
            >
          }
        />
      );
    case "object":
      return (
        <ObjectFilterSubform
          name={name}
          ancestors={ancestors}
          definition={definition}
        />
      );
    default:
      return assertNever(definition);
  }
}
